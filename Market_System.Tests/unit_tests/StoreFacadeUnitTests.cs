using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class StoreFacadeUnitTests
    {
        private Store testStore; // uses Builder of a new Product 
        private Product testProduct0;
        private Product testProduct1;
        private Employees testEmployees;
        private StoreFacade facade;


        [TestInitialize()]
        public void Setup()
        {
            facade = StoreFacade.GetInstance();

            testStore = GetStore("testStoreID326");
            StoreRepo.GetInstance().AddStore(testStore.founderID, testStore);

            testProduct0 = GetNewProduct("testStoreID326"); // storeID
            testProduct1 = GetExistingProduct("testStoreID326_tesProduct1ID"); // productID    
            StoreRepo.GetInstance().AddProduct(testStore.Store_ID, testStore.founderID, testProduct0, testProduct0.Quantity);
            StoreRepo.GetInstance().AddProduct(testStore.Store_ID, testStore.founderID, testProduct1, testProduct1.Quantity);

            ConcurrentDictionary<string, string> products = new ConcurrentDictionary<string, string>();
            products.TryAdd(this.testProduct0.Product_ID, this.testProduct0.Product_ID);
            products.TryAdd(this.testProduct1.Product_ID, this.testProduct1.Product_ID);
            testStore.allProducts = products;

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
            facade.Destroy_me();
        }




        // ================================================================================================
        // ========================================= Tests =========================================


        [TestMethod]
        public void CalculatePriceStoreFacadeTestSuccess() // no fail test
        {
            // Arrange            
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };
            double validPrice = this.testProduct0.Price * this.testProduct0.Quantity + this.testProduct1.Price * this.testProduct1.Quantity;    

            // Act
            double retPrice = this.facade.CalculatePrice(itemsToCalculate);

            // Assert
            Assert.AreEqual(validPrice, retPrice);    
        }

        [TestMethod]
        public void PurchaseStoreFacadeTestSuccess() 
        {
            // Arrange            
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };

            // Act
            this.facade.Purchase("PurchaseStoreFacadeTestUserID0", itemsToCalculate);

            // Assert
            List<ItemDTO> updatedProducts = this.testStore.GetItems();
            foreach(ItemDTO item in updatedProducts)
            {
                Assert.AreEqual(0, item.GetQuantity());
            }
        }

        [TestMethod]
        public void PurchaseCannotPurchaseStoreFacadeTestFail()
        {
            // Arrange
            // this.testEmployees.addAdmin(userID)
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };
            this.testStore.ChangeProductQuantity(this.testStore.founderID, this.testProduct0.Product_ID, 0);
            bool errorCannotPurchase = false;

            // Act
            try
            {
                this.facade.Purchase("PurchaseStoreFacadeTestUserID0", itemsToCalculate);
            }catch (Exception ex) { errorCannotPurchase = true; }

            // Assert
            Assert.IsTrue(errorCannotPurchase);
            List<ItemDTO> updatedProducts = this.testStore.GetItems();
            foreach (ItemDTO item in updatedProducts)
            {
                Assert.AreEqual(0, item.GetQuantity());
            }
        }

        [TestMethod]
        public void GatherStoresWithProductsByItemsStoreFacadeTestSuccess() // no fail test
        {
            // Arrange
            string newProductStoreID = "GatherStoresWithProductsByItemsStoreFacadeTestStoreID0";
            Store newProductStore = GetStore(newProductStoreID);
            StoreRepo.GetInstance().AddStore(newProductStore.founderID, newProductStore);
            Product testProduct2 = GetNewProduct(newProductStoreID);
            StoreRepo.GetInstance().AddProduct(newProductStore.Store_ID, newProductStore.founderID, testProduct2, testProduct2.Quantity);
            ConcurrentDictionary<string, string> products = new ConcurrentDictionary<string, string>();
            products.TryAdd(testProduct2.Product_ID, testProduct2.Product_ID);
            newProductStore.allProducts = products;

            List<ItemDTO> thisItemsToGather = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };
            List<ItemDTO> newItemsToGather = new List<ItemDTO>() { testProduct2.GetProductDTO() };
            List<ItemDTO> outThisStore;
            List<ItemDTO> outNewStore;

            // Act
            ConcurrentDictionary<string, List<ItemDTO>> retStoresWIthProduct = this.facade.GatherStoresWithProductsByItems(Enumerable.Concat(thisItemsToGather, newItemsToGather).ToList());

            // Assert
            retStoresWIthProduct.TryGetValue(this.testStore.Store_ID, out outThisStore);
            retStoresWIthProduct.TryGetValue(newProductStoreID, out outNewStore);

            Assert.IsTrue(retStoresWIthProduct.ContainsKey(this.testStore.Store_ID));
            Assert.IsTrue(retStoresWIthProduct.ContainsKey(newProductStoreID));

            
            
            Assert.IsTrue(thisItemsToGather.Any(x => outThisStore.Any(y => x.GetID() == y.GetID())) && 
                            thisItemsToGather.Count == outThisStore.Count);
            Assert.IsTrue(newItemsToGather.Any(x => outNewStore.Any(y => x.GetID() == y.GetID())) &&
                            newItemsToGather.Count == outNewStore.Count);
        }

        [TestMethod]
        public void AddNewStoreStoreFacadeTestSuccess()
        {
            string storeName = "AddNewStoreStoreFacadeTestStoreName";
            string founder = "AddNewStoreStoreFacadeTestFounderID";
            List<string> newStoreDetails = new List<string>() { storeName };

            // Act
            StoreDTO added = this.facade.AddNewStore(founder, newStoreDetails);

            // Assert
            Assert.AreEqual(StoreRepo.GetInstance().getStore(added.StoreID).Name, storeName);
        }

        public void AddNewStoreNoNameStoreFacadeTestFail()
        {
            // Arrange
            string founder = "AddNewStoreStoreFacadeTestFounderID";
            List<string> newStoreDetails = new List<string>() { "" };
            bool errorNoName = false;

            // Act
            try
            {
                this.facade.AddNewStore(founder, newStoreDetails);
            } catch (Exception ex) { errorNoName = true; }

            // Assert
            Assert.IsTrue(errorNoName);
        }

        [TestMethod]
        public void RemoveStoreStoreFacadeTestSuccess()
        {
            // Arrange
            string founder = "AddNewStoreStoreFacadeTestFounderID";
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };
            StoreDTO newStoreToAdd = this.facade.AddNewStore(founder, newStoreDetails);

            // Act
            this.facade.RemoveStore(founder, newStoreToAdd.StoreID);


            // Assert
            Assert.IsTrue(StoreRepo.GetInstance().getStore(newStoreToAdd.StoreID).is_closed_temporary());
        }

        [TestMethod]
        public void RemoveStoreNotFounderStoreFacadeTestFail()
        {
            // Arrange
            string founder = "AddNewStoreStoreFacadeTestFounderID";
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };
            StoreDTO newStoreToAdd  = this.facade.AddNewStore(founder, newStoreDetails);
            bool notFounderErrorCatched = false;

            // Act
            try
            {
                this.facade.RemoveStore("RemoveStoreStoreFacadeTestNotFounderUserID0", newStoreToAdd.StoreID);
            }
            catch (Exception ex) { notFounderErrorCatched = true; }

            // Assert
            Assert.IsFalse(StoreRepo.GetInstance().getStore(newStoreToAdd.StoreID).is_closed_temporary());
            Assert.IsTrue(notFounderErrorCatched);
        }

        [TestMethod]
        public void RestoreStoreStoreFacadeTestSuccess()
        {
            // Arrange
            string founder = "AddNewStoreStoreFacadeTestFounderID";
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };
            StoreDTO newStoreToAdd = this.facade.AddNewStore(founder, newStoreDetails);
            this.facade.RemoveStore(founder, newStoreToAdd.StoreID);

            // Act
            this.facade.RestoreStore(founder, newStoreToAdd.StoreID);

            // Assert
            Assert.IsFalse(StoreRepo.GetInstance().getStore(newStoreToAdd.StoreID).is_closed_temporary());
        }

        [TestMethod]
        public void RestoreStoreNotFounderStoreFacadeTestFail()
        {
            // Arrange
            string founder = "AddNewStoreStoreFacadeTestFounderID";
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };
            StoreDTO newStoreToAdd = this.facade.AddNewStore(founder, newStoreDetails);
            this.facade.RemoveStore(founder, newStoreToAdd.StoreID);
            bool notFounderErrorCatched = false;

            // Act
            try
            {
                this.facade.RestoreStore("RestoreStoreStoreFacadeTestNotFounderUserID0", newStoreToAdd.StoreID);
            }
            catch (Exception ex) { notFounderErrorCatched = true; }

            // Assert
            Assert.IsTrue(StoreRepo.GetInstance().getStore(newStoreToAdd.StoreID).is_closed_temporary());
            Assert.IsTrue(notFounderErrorCatched);
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
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "23", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"};
            string storeID = store;
            ConcurrentDictionary<string, Purchase_Policy> defaultStorePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            ConcurrentDictionary<string, Purchase_Strategy> defaultStoreStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            defaultStorePolicies.TryAdd(testProduct0Policy.GetID(), testProduct0Policy);
            defaultStoreStrategies.TryAdd(testProduct0Strategy.GetID(), testProduct0Strategy);
            return new Product(productProperties, storeID, defaultStorePolicies, defaultStoreStrategies);
        }

        private Product GetExistingProduct(string productid)
        {
            Purchase_Policy testProduct1Policy = new Purchase_Policy("testProduct1Policy1ID", "testProduct1Policy1Name");
            Purchase_Strategy testProduct1Strategy = new Purchase_Strategy("testProduct1Strategy1ID", "testProduct1StrategyName");
            String product_ID = productid;
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