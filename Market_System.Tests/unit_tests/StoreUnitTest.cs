using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;


namespace Market_System.Tests.unit_tests
{
    public class StoreUnitTest
    {
        // fields          
        private Store testStore; // uses Builder of a new Product 
        private Product testProduct0;
        private Product testProduct1;
        private Employees testEmployees;

        // before each test 
        [TestInitialize()]
        public void Setup()
        {
            testStore = GetStore();            
            testProduct0 = GetNewProduct();
            testProduct1 = GetExistingProduct();
            
            StoreRepo.GetInstance().AddStore(testStore.founderID, testStore.Store_ID);
            StoreRepo.GetInstance().AddProduct(testStore.Store_ID, testStore.founderID, testProduct1, testProduct1.Quantity);
            
            testEmployees = new Employees();
            testEmployees.AddNewManagerEmpPermissions(testStore.founderID, "testStockManagerID0", testStore.Store_ID, new List<Permission>() { Permission.STOCK });
            testEmployees.AddNewManagerEmpPermissions(testStore.founderID, "testInfoManagerID0", testStore.Store_ID, new List<Permission>() { Permission.INFO });            
            testEmployees.AddNewOwnerEmpPermissions(testStore.founderID, "testOwnerID0", testStore.Store_ID);
            testEmployees.AddNewManagerEmpPermissions("testOwnerID0", "testAllManagerID0", testStore.Store_ID, new List<Permission>() { Permission.INFO, Permission.STOCK });
            testStore.SetTestEmployees(testEmployees);

        }

        // after each test
        [TestCleanup()]
        public void TearDown()
        {
            // destroy StoreRepo here cause it is singletone after it is done
            EmployeeRepo.GetInstance().destroy_me();
        }


        public void TransferFoundershipStoreTest()
        {
            // Arrange
            string newFounder = "TransferFoundershipStroeTestNewFounderID1";

            // Act
            this.testStore.TransferFoundership(this.testStore.founderID, newFounder);


            // Assert
            Assert.Equals(newFounder, this.testStore.founderID);
        }

        public void ChangeNameStoreTest()
        {
            // Arrange
            string newName = "ChangeNameStroeTestName1";
            bool errorCatched = false;

            // Act
            this.testStore.ChangeName(this.testStore.founderID, newName);
            try
            {
                this.testStore.ChangeName("notFounderUser", "badStoreNameForTest");
            } catch (Exception ex) { errorCatched = true; }

            // Assert
            Assert.Equals(newName, this.testStore.Name);
            Assert.IsTrue(errorCatched);
        }

        public void ManagePermissionsStoreTest()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter - is not 'employeeID' assigner
            string employeeID = "testStockManagerID0"; // init parameter
            string otherManager = "testInfoManagerID0"; // init parameter
            List<Permission> permissions = new List<Permission>() { Permission.INFO, Permission.STOCK};  
            bool errorCatchedNotMyAssignee = false;
            bool errorCatchedNotOwner = false;

            // Act
            this.testStore.ManagePermissions(this.testStore.founderID, employeeID, permissions);
            
            try
            {
                this.testStore.ManagePermissions(storeOwner, employeeID, permissions);
            }
            catch (Exception ex) { errorCatchedNotMyAssignee = true; }

            try
            {
                this.testStore.ManagePermissions(otherManager, employeeID, permissions);
            }
            catch (Exception ex) { errorCatchedNotOwner = true; }

            // Assert
            Assert.IsTrue(this.testEmployees.confirmPermission(employeeID, this.testStore.Store_ID, Permission.INFO));
            Assert.IsTrue(this.testEmployees.confirmPermission(employeeID, this.testStore.Store_ID, Permission.STOCK));
            Assert.IsTrue(errorCatchedNotOwner);
            Assert.IsTrue(errorCatchedNotMyAssignee);
        }

        public void AssignNewOwnerStoreTest()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter
            string newOwnerID = "AssignNewOwnerStoreTestOwnerID0"; 
            string stockManager = "testStockManagerID0";
            string infoManager = "testInfoManagerID0"; // init parameter            
            bool errorCatchedNotOwner = false;
            bool errorCatchedAlreadyOwner = false;

            // Act
            this.testStore.AssignNewOwner(storeOwner, newOwnerID);

            try
            {
                this.testStore.AssignNewOwner(storeOwner, newOwnerID);
            }
            catch (Exception ex) { errorCatchedAlreadyOwner = true; }

            try
            {
                this.testStore.AssignNewOwner(infoManager, stockManager);
            }
            catch (Exception ex) { errorCatchedNotOwner = true; }

            // Assert
            Assert.IsTrue(this.testEmployees.isOwner(newOwnerID, this.testStore.Store_ID));
            Assert.IsTrue(errorCatchedNotOwner);
            Assert.IsTrue(errorCatchedAlreadyOwner);
        }

        public void AssignNewManagerStoreTest()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter
            string newManager = "AssignNewManagerStoreTestID0"; 
            string stockManager = "testStockManagerID0";
            string infoManager = "testInfoManagerID0"; // init parameter            
            bool errorCatchedNotOwner = false;
            bool errorCatchedAlreadyManager = false;

            // Act
            this.testStore.AssignNewManager(storeOwner, newManager);

            try
            {
                this.testStore.AssignNewOwner(storeOwner, newManager);
            }
            catch (Exception ex) { errorCatchedAlreadyManager = true; }

            try
            {
                this.testStore.AssignNewOwner(infoManager, stockManager);
            }
            catch (Exception ex) { errorCatchedNotOwner = true; }

            // Assert
            Assert.IsTrue(this.testEmployees.isManager(newManager, this.testStore.Store_ID));
            Assert.IsTrue(errorCatchedNotOwner);
            Assert.IsTrue(errorCatchedAlreadyManager);
        }

        public void GetOwnersOfTheStoreTest()
        {
            // Arrange
            List<string> owners = new List<string>() { "testStoreFounderID326", "testOwnerID0" }; // init values
            bool errorCatchedNoInfoPermission = false;

            // Act
            List<string> retOwners = this.testStore.GetOwnersOfTheStore("testStoreFounderID326");

            try
            {
                this.testStore.GetOwnersOfTheStore("testStockManagerID0");
            }
            catch (Exception ex) { errorCatchedNoInfoPermission = true; }

            // Assert
            Assert.IsTrue((!owners.Except(retOwners).Any()) && (!retOwners.Except(owners).Any()));
            Assert.IsTrue(errorCatchedNoInfoPermission);
        }

        public void GetManagersOfTheStoreTest()
        {
            // Arrange
            List<string> managers = new List<string>() { "testStockManagerID0", "testInfoManagerID0" }; // init values
            bool errorCatchedNoInfoPermission = false;

            // Act
            List<string> retManagers = this.testStore.GetManagersOfTheStore("testStoreFounderID326");

            try
            {
                this.testStore.GetManagersOfTheStore("testStockManagerID0");
            }
            catch (Exception ex) { errorCatchedNoInfoPermission = true; }

            // Assert
            Assert.IsTrue((!managers.Except(retManagers).Any()) && (!retManagers.Except(managers).Any()));
            Assert.IsTrue(errorCatchedNoInfoPermission);
        }

        public void AddEmployeePermissionStoreTest()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init value

            // Act
            this.testStore.AddEmployeePermission(this.testStore.founderID, employeeID, Permission.INFO);

            // Assert
            Assert.IsTrue(this.testEmployees.confirmPermission(this.testStore.founderID, this.testStore.Store_ID, Permission.INFO));
        }

        public void RemoveEmployeePermissionStoreTest()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init value
            this.testStore.AddEmployeePermission(this.testStore.founderID, employeeID, Permission.INFO);

            // Act
            this.testStore.RemoveEmployeePermission(this.testStore.founderID, employeeID, Permission.STOCK);

            // Assert
            Assert.IsFalse(this.testEmployees.confirmPermission(this.testStore.founderID, this.testStore.Store_ID, Permission.STOCK));
        }

        public void GetPurchaseHistoryOfTheStoreTest()
        {
            Assert.IsFalse(true); // implement when StorePurchaseHistory is implemented!!!!!!!!!!!!!!!!!!!!!!!!!!
        }


        public void GetStoreDTOStoreTest()
        {
            // Arrange
            StoreDTO dataStore = new StoreDTO(this.testStore);

            // Act
            StoreDTO ret = this.testStore.GetStoreDTO();

            // Assert
            Assert.Equals(dataStore.Name, ret.Name);
            Assert.Equals(dataStore.StoreID, ret.StoreID);
            Assert.Equals(dataStore.AllProducts, ret.AllProducts);
            Assert.Equals(dataStore.owners, ret.managers);
            Assert.Equals(dataStore.FounderID, ret.FounderID);
            Assert.Equals(dataStore.DefaultPolicies, ret.DefaultPolicies);
            Assert.Equals(dataStore.DefaultStrategies, ret.DefaultStrategies);
        }

        public void GetItemsStoreTest()
        {
            // Arrange
            ItemDTO productInStore = new ItemDTO(this.testProduct1);
            List<ItemDTO> items = new List<ItemDTO>() { productInStore};

            // Act
            List<ItemDTO> retItems = this.testStore.GetItems();

            // Assert
            Assert.IsTrue((!items.Except(retItems).Any()) && (!retItems.Except(items).Any()));
        }

        public void RemoveStoreTest()
        {
            // Arrange
            bool errorCatchedNotAFounder = false;
            bool noStore = false;

            // Act
            try
            {
                this.testStore.RemoveStore("testOwnerID0");
            }
            catch (Exception ex) { errorCatchedNotAFounder = true; }

            this.testStore.RemoveStore(this.testStore.founderID);

            try
            {
                StoreRepo.GetInstance().getStore(this.testStore.Store_ID); // change when StoreRepo is implemented!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            catch (Exception ex) { noStore = true; }

            // Assert
            Assert.IsTrue(noStore);
            Assert.IsTrue(errorCatchedNotAFounder);
        }

        public void CalculatePriceStoreTest()
        {
            // Arrange
            ItemDTO p1 = this.testProduct0.GetProductDTO();
            int quantity = p1.GetQuantity();
            double price = quantity * this.testProduct0.Price;
            List<ItemDTO> items = new List<ItemDTO>() {p1 };
            double result = 0;
            bool error = false;

            // Act
            try
            {
                result = this.testStore.CalculatePrice(items);
            }
            catch (Exception ex) { error = true; }

            // Assert
            Assert.Equals(price, result);
            Assert.IsFalse(error);
        }

        public void PurchaseStoreTest()
        {
            // Arrange
            ItemDTO p1 = this.testProduct0.GetProductDTO();
            List<ItemDTO> items = new List<ItemDTO>() { p1 };
            bool errorNotEnoughInStock = false;

            // Act
            this.testStore.Purchase("PurchaseStoreTestUserID0", items);
            try
            {
                this.testStore.Purchase("PurchaseStoreTestUserID0", items);
            }
            catch (Exception ex) { errorNotEnoughInStock = true; }

            // Assert
            Assert.Equals(0, ((Product)StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID)).Quantity);
            Assert.IsTrue(errorNotEnoughInStock);
        }


        public void AddStorePurchasePolicyStoreTest()
        {
            // Arrange
            Purchase_Policy newPolicy = new Purchase_Policy("AddStorePurchasePolicyStoreTestPolicyID0", "AddStorePurchasePolicyStoreTestPolicyName0");
            string stockManagerID = "testStockManagerID0"; // init value
            bool errorNoPolicyPermission = false;
            bool errorPolicyAlreadyExists = false;

            // Act
            try
            {
                this.testStore.AddStorePurchasePolicy(stockManagerID, newPolicy);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            this.testStore.AddStorePurchasePolicy(this.testStore.founderID, newPolicy);

            try
            {
                this.testStore.AddStorePurchasePolicy(this.testStore.founderID, newPolicy);
            }
            catch (Exception ex) { errorPolicyAlreadyExists = true; }

            // Assert
            Assert.IsTrue(this.testStore.defaultPolicies.ContainsKey(newPolicy.GetID()));
            Assert.IsTrue(errorNoPolicyPermission);
            Assert.IsTrue(errorPolicyAlreadyExists);
        }

        public void AddStorePurchaseStrategyStoreTest()
        {
            // Arrange
            Purchase_Strategy newStrategy = new Purchase_Strategy("AddStorePurchaseStrategyStoreTestPolicyID0", "AddStorePurchaseStrategyStoreTestPolicyName0");
            string infoManagerID = "testInfoManagerID0"; // init value
            bool errorNoPolicyPermission = false;
            bool errorStrategyAlreadyExists = false;

            // Act
            try
            {
                this.testStore.AddStorePurchaseStrategy(infoManagerID, newStrategy);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            this.testStore.AddStorePurchaseStrategy(this.testStore.founderID, newStrategy);

            try
            {
                this.testStore.AddStorePurchaseStrategy(this.testStore.founderID, newStrategy);
            }
            catch (Exception ex) { errorStrategyAlreadyExists = true; }

            // Assert
            Assert.IsTrue(this.testStore.defaultStrategies.ContainsKey(newStrategy.GetID()));
            Assert.IsTrue(errorNoPolicyPermission);
            Assert.IsTrue(errorStrategyAlreadyExists);
        }

        public void RemoveStorePurchasePolicyStoreTest()
        {
            // Arrange
            string policyToRemove = "testStorePolicyID";
            string infoManagerID = "testInfoManagerID0"; // init value
            bool errorNoPolicyPermission = false;
            bool errorPolicyDoesntExist = false;

            // Act
            try
            {
                this.testStore.RemoveStorePurchasePolicy(infoManagerID, policyToRemove);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            this.testStore.RemoveStorePurchasePolicy(this.testStore.founderID, policyToRemove);

            try
            {
                this.testStore.RemoveStorePurchasePolicy(this.testStore.founderID, policyToRemove);
            }
            catch (Exception ex) { errorPolicyDoesntExist = true; }

            // Assert
            Assert.IsTrue(this.testStore.defaultStrategies.ContainsKey(policyToRemove));
            Assert.IsTrue(errorNoPolicyPermission);
            Assert.IsTrue(errorPolicyDoesntExist);
        }

        public void RemoveStorePurchaseStrategyStoreTest()
        {
            // Arrange
            string strategyToRemove = "testStoreStrategyID";
            string infoManagerID = "testInfoManagerID0"; // init value
            bool errorNoPolicyPermission = false;
            bool errorStrategyDoesntExist = false;

            // Act
            try
            {
                this.testStore.RemoveStorePurchaseStrategy(infoManagerID, strategyToRemove);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            this.testStore.RemoveStorePurchaseStrategy(this.testStore.founderID, strategyToRemove);

            try
            {
                this.testStore.RemoveStorePurchaseStrategy(this.testStore.founderID, strategyToRemove);
            }
            catch (Exception ex) { errorStrategyDoesntExist = true; }

            // Assert
            Assert.IsTrue(this.testStore.defaultStrategies.ContainsKey(strategyToRemove));
            Assert.IsTrue(errorNoPolicyPermission);
            Assert.IsTrue(errorStrategyDoesntExist);
        }

        public void AddProductStoreTest()
        {
            // Arrange            
            string infoManager = "testInfoManagerID0"; // init value
            bool errorNoStockPermission = false;
            bool itemAdded = false;
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"}; // init value

            // Act
            try
            {
                this.testStore.AddProduct(infoManager, productProperties);
            }
            catch (Exception ex) { errorNoStockPermission = true; }

            this.testStore.AddProduct(this.testStore.founderID, productProperties);



            // Assert
            foreach (ItemDTO item in this.testStore.GetItems())
                if (item.GetID() == this.testProduct0.Product_ID)
                    itemAdded = true;
            Assert.IsTrue(itemAdded);
            Assert.IsTrue(errorNoStockPermission);
        }


        public void RemoveProductStoreTest()
        {
            // Arrange            
            string infoManager = "testInfoManagerID0"; // init value
            bool errorNoStockPermission = false;
            bool itemRemoved = true;

            // Act
            try
            {
                this.testStore.RemoveProduct(infoManager, this.testProduct1.Product_ID);
            }
            catch (Exception ex) { errorNoStockPermission = true; }

            this.testStore.RemoveProduct(this.testStore.founderID, this.testProduct1.Product_ID);

            // Assert
            foreach (ItemDTO item in this.testStore.GetItems())
                if (item.GetID() == this.testProduct1.Product_ID)
                    itemRemoved = false;
            Assert.IsTrue(itemRemoved);
            Assert.IsTrue(errorNoStockPermission);
        }

        // ========================================= END of Tests =========================================
        // ================================================================================================


        private Store GetStore()
        {
            string founderID = "testStoreFounderID326";
            string storeID = "testStoreID326";

            Purchase_Policy testStorePolicy = new Purchase_Policy("testStorePolicyID", "testStorePolicyName");
            List<Purchase_Policy> policies = new List<Purchase_Policy>() { testStorePolicy };
            Purchase_Strategy testStoreStrategy = new Purchase_Strategy("testStoreStrategyID", "testStoreStrategyName");
            List<Purchase_Strategy> strategies = new List<Purchase_Strategy>() { testStoreStrategy };

            List<string> allProductsIDS = new List<string>() { "testProduct1StoreID465_tesProduct1ID" };

            return new Store(founderID, storeID, policies, strategies, allProductsIDS);
        }

        private Product GetNewProduct()
        {
            Purchase_Policy testProduct0Policy = new Purchase_Policy("testProduct0Policy1ID", "testProduct0Policy1Name");
            Purchase_Strategy testProduct0Strategy = new Purchase_Strategy("testProduct0Strategy1ID", "testProduct0StrategyName");
            // productProperties = {Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale ,Weight, Dimenssions, PurchaseAttributes, ProductCategory}
            // ProductAttributes = atr1Name:atr1opt1_atr1opt2...atr1opti;atr2name:atr2opt1...
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"};
            string storeID = "testProduct0StoreID789";
            ConcurrentDictionary<string, Purchase_Policy> defaultStorePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            ConcurrentDictionary<string, Purchase_Strategy> defaultStoreStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            defaultStorePolicies.TryAdd(testProduct0Policy.GetID(), testProduct0Policy);
            defaultStoreStrategies.TryAdd(testProduct0Strategy.GetID(), testProduct0Strategy);
            return new Product(productProperties, storeID, defaultStorePolicies, defaultStoreStrategies);
        }

        private Product GetExistingProduct()
        {
            Purchase_Policy testProduct1Policy = new Purchase_Policy("testProduct1Policy1ID", "testProduct1Policy1Name");
            Purchase_Strategy testProduct1Strategy = new Purchase_Strategy("testProduct1Strategy1ID", "testProduct1StrategyName");
            String product_ID = "testProduct1StoreID465_tesProduct1ID";
            String name = "testProduct1Name";
            String description = "testProduct1Description";
            double price = 678.9;
            int initQuantity = 89;
            int reservedQuantity = 12;
            double rating = 7.8; // 1-10 
            double sale = 15; // % 
            double weight = 5.1;
            double[] dimenssions = new double[] { 10.2, 30.4, 50.6 };
            List<String> comments = new List<string> { "testProduct1UserID123 + testProduct1Comment1 + Rating: testProduct1Comment1Rating.", "testProduct1UserID456 + testProduct1Comment2 + Rating: testProduct1Comment2Rating." };
            ConcurrentDictionary<string, Purchase_Policy> defaultStorePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            ConcurrentDictionary<string, Purchase_Strategy> defaultStoreStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            defaultStorePolicies.TryAdd(testProduct1Policy.GetID(), testProduct1Policy);
            defaultStoreStrategies.TryAdd(testProduct1Strategy.GetID(), testProduct1Strategy);

            Dictionary<string, List<string>> product_Attributes = new Dictionary<string, List<string>>();
            List<string> attr1 = new List<string>() { "testProduct1Atr1Opt1", "testProduct1Atr1Opt2" };
            List<string> attr2 = new List<string>() { "testProduct1Atr2Opt1", "testProduct1Atr2Opt2", "testProduct1Atr2Opt3" };
            product_Attributes.Add("testProduct1Atr1", attr1);
            product_Attributes.Add("testProduct1Atr2", attr2);

            int boughtTimes = 11;
            Category category = new Category("testProduct1SomeCategory");
            return new Product(product_ID, name, description, price, initQuantity, reservedQuantity, rating, sale, weight,
                                dimenssions, comments, defaultStorePolicies, defaultStoreStrategies, product_Attributes, boughtTimes, category);
        }

    }    
}
