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
            testStore = GetStore("testStoreID326");            
            testProduct0 = GetNewProduct("testStoreID326");
            testProduct1 = GetExistingProduct();
            
            StoreRepo.GetInstance().AddStore(testStore.founderID, testStore);
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
            StoreRepo.GetInstance().destroy();
            EmployeeRepo.GetInstance().destroy_me();            
        }


        public void TransferFoundershipWithFounderStoreTestSuccess()
        {
            // Arrange
            string newFounder = "TransferFoundershipStroeTestNewFounderID1";

            // Act
            this.testStore.TransferFoundership(this.testStore.founderID, newFounder);


            // Assert
            Assert.Equals(newFounder, this.testStore.founderID);
        }

        public void TransferFoundershipWithNotFounderStoreTesFailt()
        {
            // Arrange
            string newFounder = "TransferFoundershipStroeTestNewFounderID1";

            // Act
            this.testStore.TransferFoundership(newFounder + "NotFounder", newFounder);



            // Assert
            Assert.AreNotEqual(newFounder, this.testStore.founderID);
        }

        public void ChangeNameByFounderStoreTestSuccess()
        {
            // Arrange
            string newName = "ChangeNameStroeTestName1";
            bool errorCatched = false;

            // Act
            this.testStore.ChangeName(this.testStore.founderID, newName);
            try
            {
                this.testStore.ChangeName("notFounderUser", "badStoreNameForTest");
            }
            catch (Exception ex) { errorCatched = true; }

            // Assert
            Assert.Equals(newName, this.testStore.Name);
            Assert.IsTrue(errorCatched);
        }

        public void ChangeNameByNOTFounderStoreTestFail()
        {
            // Arrange
            bool errorCatched = false;

            // Act
            try
            {
                this.testStore.ChangeName("notFounderUser", "badStoreNameForTest");
            }
            catch (Exception ex) { errorCatched = true; }

            // Assert
            Assert.IsTrue(errorCatched);
        }



        public void ManagePermissionsStoreTestSuccess()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init parameter
            List<Permission> permissions = new List<Permission>() { Permission.INFO, Permission.STOCK};  

            // Act
            this.testStore.ManagePermissions(this.testStore.founderID, employeeID, permissions);

            // Assert
            Assert.IsTrue(this.testEmployees.confirmPermission(employeeID, this.testStore.Store_ID, Permission.INFO));
            Assert.IsTrue(this.testEmployees.confirmPermission(employeeID, this.testStore.Store_ID, Permission.STOCK));
        }

        public void ManagePermissionsWithBadAssigneeStoreTestFail()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter - is not 'employeeID' assigner
            string employeeID = "testStockManagerID0"; // init parameter
            List<Permission> permissions = new List<Permission>() { Permission.INFO, Permission.STOCK };
            bool errorCatchedNotMyAssignee = false;

            // Act
            try
            {
                this.testStore.ManagePermissions(storeOwner, employeeID, permissions);
            }
            catch (Exception ex) { errorCatchedNotMyAssignee = true; }

            // Assert
            Assert.IsTrue(errorCatchedNotMyAssignee);
        }

        public void ManagePermissionsWithNotOwnerStoreTestFail()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init parameter
            string otherManager = "testInfoManagerID0"; // init parameter
            List<Permission> permissions = new List<Permission>() { Permission.INFO, Permission.STOCK };
            bool errorCatchedNotOwner = false;

            // Act
            try
            {
                this.testStore.ManagePermissions(otherManager, employeeID, permissions);
            }
            catch (Exception ex) { errorCatchedNotOwner = true; }

            // Assert           
            Assert.IsTrue(errorCatchedNotOwner);
        }

        public void AssignNewOwnerStoreTestSuccess()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter
            string newOwnerID = "AssignNewOwnerStoreTestOwnerID0";

            // Act
            this.testStore.AssignNewOwner(storeOwner, newOwnerID);

            // Assert
            Assert.IsTrue(this.testEmployees.isOwner(newOwnerID, this.testStore.Store_ID));
        }

        public void AssignNewOwnerWithNoOwnerStoreTestFail()
        {
            // Arrange
            string stockManager = "testStockManagerID0";
            string infoManager = "testInfoManagerID0"; // init parameter            
            bool errorCatchedNotOwner = false;

            // Act
            try
            {
                this.testStore.AssignNewOwner(infoManager, stockManager);
            }
            catch (Exception ex) { errorCatchedNotOwner = true; }

            // Assert
            Assert.IsTrue(errorCatchedNotOwner);
        }

        public void AssignNewOwnerAlreadyOwnerStoreTestFail()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter
            string newOwnerID = "AssignNewOwnerStoreTestOwnerID0";
            bool errorCatchedAlreadyOwner = false;

            // Act
            try
            {
                this.testStore.AssignNewOwner(storeOwner, newOwnerID);
            }
            catch (Exception ex) { errorCatchedAlreadyOwner = true; }

            // Assert
            Assert.IsTrue(errorCatchedAlreadyOwner);
        }

        public void AssignNewManagerStoreTestSuccess()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter
            string newManager = "AssignNewManagerStoreTestID0";

            // Act
            this.testStore.AssignNewManager(storeOwner, newManager);

            // Assert
            Assert.IsTrue(this.testEmployees.isManager(newManager, this.testStore.Store_ID));
        }

        public void AssignNewManagerWithNotOwnerStoreTestFail()
        {
            // Arrange
            string stockManager = "testStockManagerID0";
            string infoManager = "testInfoManagerID0"; // init parameter            
            bool errorCatchedNotOwner = false;

            // Act
            try
            {
                this.testStore.AssignNewOwner(infoManager, stockManager);
            }
            catch (Exception ex) { errorCatchedNotOwner = true; }

            // Assert
            Assert.IsTrue(errorCatchedNotOwner);
        }

        public void AssignNewManagerAlreadyManagerStoreTestFail()
        {
            // Arrange
            string storeOwner = "testOwnerID0"; // init parameter
            string newManager = "AssignNewManagerStoreTestID0";
            bool errorCatchedAlreadyManager = false;

            // Act
            try
            {
                this.testStore.AssignNewOwner(storeOwner, newManager);
            }
            catch (Exception ex) { errorCatchedAlreadyManager = true; }

            // Assert
            Assert.IsTrue(errorCatchedAlreadyManager);
        }

        public void GetOwnersOfTheStoreTestSuccess()
        {
            // Arrange
            List<string> owners = new List<string>() { "testStoreFounderID326", "testOwnerID0" }; // init values

            // Act
            List<string> retOwners = this.testStore.GetOwnersOfTheStore("testStoreFounderID326");

            // Assert
            Assert.IsTrue((!owners.Except(retOwners).Any()) && (!retOwners.Except(owners).Any()));
        }

        public void GetOwnersOfTheStoreNoPermissionTestFail()
        {
            // Arrange
            List<string> owners = new List<string>() { "testStoreFounderID326", "testOwnerID0" }; // init values
            bool errorCatchedNoInfoPermission = false;

            // Act
            try
            {
                this.testStore.GetOwnersOfTheStore("testStockManagerID0");
            }
            catch (Exception ex) { errorCatchedNoInfoPermission = true; }

            // Assert
            Assert.IsTrue(errorCatchedNoInfoPermission);
        }

        public void GetManagersOfTheStoreTestSuccess()
        {
            // Arrange
            List<string> managers = new List<string>() { "testStockManagerID0", "testInfoManagerID0" }; // init values

            // Act
            List<string> retManagers = this.testStore.GetManagersOfTheStore("testStoreFounderID326");

            // Assert
            Assert.IsTrue((!managers.Except(retManagers).Any()) && (!retManagers.Except(managers).Any()));
        }

        public void GetManagersOfTheStoreNoPermissionTestFail()
        {
            // Arrange
            List<string> managers = new List<string>() { "testStockManagerID0", "testInfoManagerID0" }; // init values
            bool errorCatchedNoInfoPermission = false;

            // Act
            try
            {
                this.testStore.GetManagersOfTheStore("testStockManagerID0");
            }
            catch (Exception ex) { errorCatchedNoInfoPermission = true; }

            // Assert
            Assert.IsTrue(errorCatchedNoInfoPermission);
        }

        public void AddEmployeePermissionStoreTestSuccess()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init value

            // Act
            this.testStore.AddEmployeePermission(this.testStore.founderID, employeeID, Permission.INFO);

            // Assert
            Assert.IsTrue(this.testEmployees.confirmPermission(this.testStore.founderID, this.testStore.Store_ID, Permission.INFO));
        }

        public void AddEmployeePermissionNoPermissionStoreTestFail()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init value
            bool errorCatchedNoPermission = false;

            // Act
            try
            {
                this.testStore.AddEmployeePermission(employeeID + "badOwner", employeeID, Permission.INFO);
            }
            catch (Exception ex) { errorCatchedNoPermission = true; }

            // Assert
            Assert.IsTrue(errorCatchedNoPermission);
        }

        public void RemoveEmployeePermissionStoreTestSuccess()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init value
            this.testStore.AddEmployeePermission(this.testStore.founderID, employeeID, Permission.INFO);

            // Act
            this.testStore.RemoveEmployeePermission(this.testStore.founderID, employeeID, Permission.STOCK);

            // Assert
            Assert.IsFalse(this.testEmployees.confirmPermission(this.testStore.founderID, this.testStore.Store_ID, Permission.STOCK));
        }

        public void RemoveEmployeePermissionNoPermissionStoreTestFail()
        {
            // Arrange
            string employeeID = "testStockManagerID0"; // init value
            this.testStore.AddEmployeePermission(this.testStore.founderID, employeeID, Permission.INFO);
            bool errorCatchedNoPermission = false;

            // Act            
            try
            {
                this.testStore.RemoveEmployeePermission(this.testStore.founderID, employeeID, Permission.STOCK);
            }
            catch (Exception ex) { errorCatchedNoPermission = true; }


            // Assert
            Assert.IsTrue(errorCatchedNoPermission);
        }

        public void GetPurchaseHistoryOfTheStoreTestSuccess()
        {
            // Arrange
            string buyerID = "GetPurchaseHistoryOfTheStoreTestSuccessID0"; // init value
            ItemDTO productInStore = new ItemDTO(this.testProduct1);
            List<ItemDTO> items = new List<ItemDTO>() { productInStore };
            this.testStore.Purchase(buyerID, items);
            string history = StoreRepo.GetInstance().getPurchaseHistoryOfTheStore(this.testStore.Store_ID);

            // Act
            string retHistory = this.testStore.GetPurchaseHistoryOfTheStore(this.testStore.founderID);

            // Assert
            Assert.Equals(history, retHistory);
        }

        public void GetPurchaseHistoryOfTheStoreNoPermissionTestFail()
        {
            // Arrange
            string buyerID = "GetPurchaseHistoryOfTheStoreTestFailID0"; // init value
            bool errorCatchedNoPermission = false;

            // Act
            try
            {
                this.testStore.GetPurchaseHistoryOfTheStore(buyerID);
            }
            catch (Exception ex) { errorCatchedNoPermission = true; }

            // Assert
            Assert.IsTrue(errorCatchedNoPermission);
        }

        public void GetStoreDTOStoreTestSuccess() // no fail test for this 
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

        public void GetItemsStoreTestSuccess()
        {
            // Arrange
            ItemDTO productInStore = new ItemDTO(this.testProduct1);
            List<ItemDTO> items = new List<ItemDTO>() { productInStore };

            // Act
            List<ItemDTO> retItems = this.testStore.GetItems();

            // Assert
            Assert.IsTrue((!items.Except(retItems).Any()) && (!retItems.Except(items).Any()));
        }

        public void GetItemsNoProductsInStoreStoreTest()
        {
            // Arrange
            this.testStore.RemoveProduct(this.testStore.founderID, this.testProduct1.Product_ID);

            // Act
            List<ItemDTO> retItems = this.testStore.GetItems();

            // Assert
            Assert.Equals(retItems.Count(), 0);
        }

        public void RemoveStoreTestSuccess()
        {
            // Arrange
            bool noStore = false;

            // Act
            this.testStore.RemoveStore(this.testStore.founderID);

            try
            {
                StoreRepo.GetInstance().getStore(this.testStore.Store_ID); // change when StoreRepo is implemented!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            catch (Exception ex) { noStore = true; }

            // Assert
            Assert.IsTrue(noStore);
        }

        public void RemoveStoreNoFounderTest()
        {
            // Arrange
            bool errorCatchedNotAFounder = false;

            // Act
            try
            {
                this.testStore.RemoveStore("testOwnerID0");
            }
            catch (Exception ex) { errorCatchedNotAFounder = true; }

            // Assert
            Assert.IsTrue(errorCatchedNotAFounder);
        }

        public void CalculatePriceStoreTestSuccess()
        {
            // Arrange
            ItemDTO p1 = this.testProduct1.GetProductDTO();
            int quantity = p1.GetQuantity();
            double price = quantity * this.testProduct1.Price;
            List<ItemDTO> items = new List<ItemDTO>() { p1 };
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

        public void CalculatePriceNoSuchProductStoreTestFail()
        {
            // Arrange
            ItemDTO p1 = this.testProduct0.GetProductDTO();
            List<ItemDTO> items = new List<ItemDTO>() { p1 };
            this.testStore.RemoveProduct(this.testStore.founderID, this.testProduct1.Product_ID);
            bool errorCatchedNoSuchProductInThisStore = false;

            // Act
            try
            {
                this.testStore.CalculatePrice(items);
            }
            catch (Exception ex) { errorCatchedNoSuchProductInThisStore = true; }

            // Assert
            Assert.IsTrue(errorCatchedNoSuchProductInThisStore);
        }

        public void PurchaseStoreTestSuccess()
        {
            // Arrange
            ItemDTO p1 = this.testProduct0.GetProductDTO();
            List<ItemDTO> items = new List<ItemDTO>() { p1 };

            // Act
            this.testStore.Purchase("PurchaseStoreTestUserID0", items);

            // Assert
            Assert.Equals(0, ((Product)StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID)).Quantity);
        }

        public void PurchaseNorEnoughInStockStoreTestFail()
        {
            // Arrange
            ItemDTO p1 = this.testProduct0.GetProductDTO();
            List<ItemDTO> items = new List<ItemDTO>() { p1 };
            bool errorNotEnoughInStock = false;
            this.testStore.Purchase("PurchaseStoreTestUserID0", items);

            // Act
            try
            {
                this.testStore.Purchase("PurchaseStoreTestUserID0", items);
            }
            catch (Exception ex) { errorNotEnoughInStock = true; }

            // Assert
            Assert.IsTrue(errorNotEnoughInStock);
        }

        public void AddStorePurchasePolicyStoreTestSuccess()
        {
            // Arrange
            Purchase_Policy newPolicy = new Purchase_Policy("AddStorePurchasePolicyStoreTestPolicyID0", "AddStorePurchasePolicyStoreTestPolicyName0");

            // Act
            this.testStore.AddStorePurchasePolicy(this.testStore.founderID, newPolicy);

            // Assert
            Assert.IsTrue(this.testStore.defaultPolicies.ContainsKey(newPolicy.GetID()));
        }

        public void AddStorePurchasePolicyNoPermissionStoreTestFail()
        {
            // Arrange
            Purchase_Policy newPolicy = new Purchase_Policy("AddStorePurchasePolicyStoreTestPolicyID0", "AddStorePurchasePolicyStoreTestPolicyName0");
            string stockManagerID = "testStockManagerID0"; // init value
            bool errorNoPolicyPermission = false;

            // Act
            try
            {
                this.testStore.AddStorePurchasePolicy(stockManagerID, newPolicy);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            // Assert
            Assert.IsTrue(errorNoPolicyPermission);
        }

        public void AddStorePurchasePolicyAlreadyExistsStoreTestFail()
        {
            // Arrange
            Purchase_Policy newPolicy = new Purchase_Policy("AddStorePurchasePolicyStoreTestPolicyID0", "AddStorePurchasePolicyStoreTestPolicyName0");
            bool errorPolicyAlreadyExists = false;

            // Act
            this.testStore.AddStorePurchasePolicy(this.testStore.founderID, newPolicy);

            try
            {
                this.testStore.AddStorePurchasePolicy(this.testStore.founderID, newPolicy);
            }
            catch (Exception ex) { errorPolicyAlreadyExists = true; }

            // Assert
            Assert.IsTrue(errorPolicyAlreadyExists);
        }

        public void AddStorePurchaseStrategyStoreTestSuccess()
        {
            // Arrange
            Purchase_Strategy newStrategy = new Purchase_Strategy("AddStorePurchaseStrategyStoreTestPolicyID0", "AddStorePurchaseStrategyStoreTestPolicyName0");

            // Act
            this.testStore.AddStorePurchaseStrategy(this.testStore.founderID, newStrategy);


            // Assert
            Assert.IsTrue(this.testStore.defaultStrategies.ContainsKey(newStrategy.GetID()));
        }

        public void AddStorePurchaseStrategyNoPermissionStoreTestFail()
        {
            // Arrange
            Purchase_Strategy newStrategy = new Purchase_Strategy("AddStorePurchaseStrategyStoreTestPolicyID0", "AddStorePurchaseStrategyStoreTestPolicyName0");
            string infoManagerID = "testInfoManagerID0"; // init value
            bool errorNoPolicyPermission = false;

            // Act
            try
            {
                this.testStore.AddStorePurchaseStrategy(infoManagerID, newStrategy);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            // Assert
            Assert.IsTrue(errorNoPolicyPermission);
        }

        public void AddStorePurchaseStrategyStoreTestFail()
        {
            // Arrange
            Purchase_Strategy newStrategy = new Purchase_Strategy("AddStorePurchaseStrategyStoreTestPolicyID0", "AddStorePurchaseStrategyStoreTestPolicyName0");
            bool errorStrategyAlreadyExists = false;
            this.testStore.AddStorePurchaseStrategy(this.testStore.founderID, newStrategy);

            // Act
            try
            {
                this.testStore.AddStorePurchaseStrategy(this.testStore.founderID, newStrategy);
            }
            catch (Exception ex) { errorStrategyAlreadyExists = true; }

            // Assert
            Assert.IsTrue(errorStrategyAlreadyExists);
        }

        public void RemoveStorePurchasePolicyStoreTestSuccess()
        {
            // Arrange
            string policyToRemove = "testStorePolicyID";

            // Act
            this.testStore.RemoveStorePurchasePolicy(this.testStore.founderID, policyToRemove);

            // Assert
            Assert.IsTrue(this.testStore.defaultStrategies.ContainsKey(policyToRemove));
        }

        public void RemoveStorePurchasePolicySNopermissiontoreTestFail()
        {
            // Arrange
            string policyToRemove = "testStorePolicyID";
            string infoManagerID = "testInfoManagerID0"; // init value
            bool errorNoPolicyPermission = false;
            // Act
            try
            {
                this.testStore.RemoveStorePurchasePolicy(infoManagerID, policyToRemove);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            // Assert
            Assert.IsTrue(errorNoPolicyPermission);
        }

        public void RemoveStorePurchasePolicyDoesntExistStoreTestFail()
        {
            // Arrange
            string policyToRemove = "testStorePolicyID";
            bool errorPolicyDoesntExist = false;
            this.testStore.RemoveStorePurchasePolicy(this.testStore.founderID, policyToRemove);

            // Act
            try
            {
                this.testStore.RemoveStorePurchasePolicy(this.testStore.founderID, policyToRemove);
            }
            catch (Exception ex) { errorPolicyDoesntExist = true; }

            // Assert
            Assert.IsTrue(errorPolicyDoesntExist);
        }

        public void RemoveStorePurchaseStrategyStoreTest()
        {
            // Arrange
            string strategyToRemove = "testStoreStrategyID";
            string infoManagerID = "testInfoManagerID0"; // init value

            // Act
            this.testStore.RemoveStorePurchaseStrategy(this.testStore.founderID, strategyToRemove);

            // Assert
            Assert.IsTrue(this.testStore.defaultStrategies.ContainsKey(strategyToRemove));
        }

        public void RemoveStorePurchaseStrategyNoPermissionStoreTestFail()
        {
            // Arrange
            string strategyToRemove = "testStoreStrategyID";
            string infoManagerID = "testInfoManagerID0"; // init value
            bool errorNoPolicyPermission = false;

            // Act
            try
            {
                this.testStore.RemoveStorePurchaseStrategy(infoManagerID, strategyToRemove);
            }
            catch (Exception ex) { errorNoPolicyPermission = true; }

            // Assert
            Assert.IsTrue(errorNoPolicyPermission);
        }

        public void RemoveStorePurchaseStrategyDoesntExistStoreTestFail()
        {
            // Arrange
            string strategyToRemove = "testStoreStrategyID";
            bool errorStrategyDoesntExist = false;
            this.testStore.RemoveStorePurchaseStrategy(this.testStore.founderID, strategyToRemove);

            // Act
            try
            {
                this.testStore.RemoveStorePurchaseStrategy(this.testStore.founderID, strategyToRemove);
            }
            catch (Exception ex) { errorStrategyDoesntExist = true; }

            // Assert
            Assert.IsTrue(errorStrategyDoesntExist);
        }

        public void AddProductStoreTestSuccess()
        {
            // Arrange            
            bool itemAdded = false;
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"}; // init value

            // Act
            this.testStore.AddProduct(this.testStore.founderID, productProperties);



            // Assert
            foreach (ItemDTO item in this.testStore.GetItems())
                if (item.GetID() == this.testProduct0.Product_ID)
                    itemAdded = true;
            Assert.IsTrue(itemAdded);
        }

        public void AddProductNoPermissionStoreTestFail()
        {
            // Arrange            
            string infoManager = "testInfoManagerID0"; // init value
            bool errorNoStockPermission = false;
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"}; // init value

            // Act
            try
            {
                this.testStore.AddProduct(infoManager, productProperties);
            }
            catch (Exception ex) { errorNoStockPermission = true; }


            // Assert
            Assert.IsTrue(errorNoStockPermission);
        }

        public void RemoveProductStoreTestSuccess()
        {
            // Arrange            
            bool itemRemoved = true;

            // Act
            this.testStore.RemoveProduct(this.testStore.founderID, this.testProduct1.Product_ID);

            // Assert
            foreach (ItemDTO item in this.testStore.GetItems())
                if (item.GetID() == this.testProduct1.Product_ID)
                    itemRemoved = false;
            Assert.IsTrue(itemRemoved);
        }

        public void RemoveProductNoPermissionStoreTestFail()
        {
            // Arrange            
            string infoManager = "testInfoManagerID0"; // init value
            bool errorNoStockPermission = false;

            // Act
            try
            {
                this.testStore.RemoveProduct(infoManager, this.testProduct1.Product_ID);
            }
            catch (Exception ex) { errorNoStockPermission = true; }

            // Assert
            Assert.IsTrue(errorNoStockPermission);
        }

        public void RemoveProductNoSuchProductStoreTestFail()
        {
            // Arrange            
            bool errorNoSuchProduct = false;

            // Act
            try
            {
                this.testStore.RemoveProduct(this.testStore.founderID, this.testProduct0.Product_ID);
            }
            catch (Exception ex) { errorNoSuchProduct = true; }

            // Assert
            Assert.IsTrue(errorNoSuchProduct);
        }


        // ========================================= END of Tests =========================================
        // ================================================================================================


        private Store GetStore(string newStoreID)
        {
            string founderID = "testStoreFounderID326";
            string storeID = newStoreID;

            Purchase_Policy testStorePolicy = new Purchase_Policy("testStorePolicyID", "testStorePolicyName");
            List<Purchase_Policy> policies = new List<Purchase_Policy>() { testStorePolicy };
            Purchase_Strategy testStoreStrategy = new Purchase_Strategy("testStoreStrategyID", "testStoreStrategyName");
            List<Purchase_Strategy> strategies = new List<Purchase_Strategy>() { testStoreStrategy };

            List<string> allProductsIDS = new List<string>() { "testProduct1StoreID465_tesProduct1ID" };

            return new Store(founderID, storeID, policies, strategies, allProductsIDS, false);
        }

        private Product GetNewProduct(string store)
        {
            Purchase_Policy testProduct0Policy = new Purchase_Policy("testProduct0Policy1ID", "testProduct0Policy1Name");
            Purchase_Strategy testProduct0Strategy = new Purchase_Strategy("testProduct0Strategy1ID", "testProduct0StrategyName");
            // productProperties = {Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale ,Weight, Dimenssions, PurchaseAttributes, ProductCategory}
            // ProductAttributes = atr1Name:atr1opt1_atr1opt2...atr1opti;atr2name:atr2opt1...
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"};
            string storeID = store;
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
            String product_ID = "testStoreID326_tesProduct1ID";
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
