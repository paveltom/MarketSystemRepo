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
    internal class StoreFacadeUnitTests
    {
        private Store testStore; // uses Builder of a new Product 
        private Product testProduct0;
        private Product testProduct1;
        private Employees testEmployees;
        private StoreFacade facade;

        public void Setup()
        {
            facade = StoreFacade.GetInstance();
            testStore = GetStore("testStoreID326");
            testProduct0 = GetNewProduct("testStoreID326");
            testProduct0.SetQuantity(23);
            testProduct1 = GetExistingProduct();

            StoreRepo.GetInstance().AddStore(testStore.founderID, testStore);
            StoreRepo.GetInstance().AddProduct(testStore.Store_ID, testStore.founderID, testProduct0, testProduct0.Quantity);
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
            facade.Destroy_me();
        }




        // ================================================================================================
        // ========================================= Tests =========================================


        public void CalculatePriceStoreFacadeTest()
        {
            // Arrange            
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };
            double validPrice = this.testProduct0.Price * this.testProduct0.Quantity + this.testProduct1.Price * this.testProduct1.Quantity;    

            // Act
            double retPrice = this.facade.CalculatePrice(itemsToCalculate);

            // Assert
            Assert.Equals(validPrice, retPrice);    
        }

        public void PurchaseStoreFacadeTest()
        {
            // Arrange            
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };

            // Act
            this.facade.Purchase("PurchaseStoreFacadeTestUserID0", itemsToCalculate);

            // Assert
            List<ItemDTO> updatedProducts = this.testStore.GetItems();
            foreach(ItemDTO item in updatedProducts)
            {
                Assert.Equals(0, item.GetQuantity());
            }
        }

        public void GatherStoresWithProductsByItemsStoreFacadeTest()
        {
            // Arrange
            string newProductStoreID = "GatherStoresWithProductsByItemsStoreFacadeTestStoreID0";
            Product testProduct2 = GetNewProduct(newProductStoreID);
            List<ItemDTO> thisItemsToGather = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };
            List<ItemDTO> newItemsToGather = new List<ItemDTO>() { testProduct2.GetProductDTO() };
            List<ItemDTO> outThisStore;
            List<ItemDTO> outNewStore;

            // Act
            ConcurrentDictionary<string, List<ItemDTO>> retStoresWIthProduct =  this.facade.GatherStoresWithProductsByItems(Enumerable.Concat(thisItemsToGather, newItemsToGather).ToList());

            // Assert
            retStoresWIthProduct.TryGetValue(this.testStore.Store_ID, out outThisStore);
            retStoresWIthProduct.TryGetValue(newProductStoreID, out outNewStore);

            Assert.IsTrue(retStoresWIthProduct.ContainsKey(this.testStore.Store_ID));
            Assert.IsTrue(retStoresWIthProduct.ContainsKey(newProductStoreID));
            Assert.Equals(thisItemsToGather, outThisStore);
            Assert.Equals(newItemsToGather, outNewStore);
        }

        public void AddNewStoreStoreFacadeTest()
        {
            // Arrange
            Store newStoreToAdd = GetStore("AddNewStoreStoreFacadeTestStoreID0");
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };

            // Act
            this.facade.AddNewStore(newStoreToAdd.founderID, newStoreDetails);

            // Assert
            Assert.Equals(StoreRepo.GetInstance().getStore(newStoreToAdd.Store_ID).Name, newStoreToAdd.Name);
        }

        public void RemoveStoreStoreFacadeTest()
        {
            // Arrange
            Store newStoreToAdd = GetStore("AddNewStoreStoreFacadeTestStoreID0");
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };
            this.facade.AddNewStore(newStoreToAdd.founderID, newStoreDetails);
            bool notFounderErrorCatched = false;

            // Act
            try
            {
                this.facade.RemoveStore("RemoveStoreStoreFacadeTestNotFounderUserID0", newStoreToAdd.Store_ID);
            }catch (Exception ex) { notFounderErrorCatched = true; }

            this.facade.RemoveStore(newStoreToAdd.founderID, newStoreToAdd.Store_ID);


            // Assert
            Assert.Equals(StoreRepo.GetInstance().getStore(newStoreToAdd.Store_ID).Name, newStoreToAdd.Name);
            Assert.IsTrue(notFounderErrorCatched);
        }

        public void RestoreStoreStoreFacadeTest()
        {
            // Arrange
            Store newStoreToAdd = GetStore("AddNewStoreStoreFacadeTestStoreID0");
            List<string> newStoreDetails = new List<string>() { "AddNewStoreStoreFacadeTestStoreName" };
            this.facade.AddNewStore(newStoreToAdd.founderID, newStoreDetails);
            this.facade.RemoveStore(newStoreToAdd.founderID, newStoreToAdd.Store_ID);
            bool notFounderErrorCatched = false;

            // Act
            try
            {
                this.facade.RestoreStore("RestoreStoreStoreFacadeTestNotFounderUserID0", newStoreToAdd.Store_ID);
            }
            catch (Exception ex) { notFounderErrorCatched = true; }

            this.facade.RestoreStore(newStoreToAdd.founderID, newStoreToAdd.Store_ID);

            // Assert
            Assert.Equals(StoreRepo.GetInstance().getStore(newStoreToAdd.Store_ID).Name, newStoreToAdd.Name);
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