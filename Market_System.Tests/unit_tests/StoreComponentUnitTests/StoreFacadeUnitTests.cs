using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market_System.DAL;


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

            testStore = GetStore("testStoreFounderID326");
            testProduct0 = GetNewProduct(testStore); // storeID
            testProduct1 = GetExistingProduct(testStore); // productID    
            //StoreRepo.GetInstance().AddProduct(testStore.Store_ID, testStore.founderID, testProduct0, testProduct0.Quantity);
            //StoreRepo.GetInstance().AddProduct(testStore.Store_ID, testStore.founderID, testProduct1, testProduct1.Quantity);

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
            ItemDTO item0 = this.testProduct0.GetProductDTO();
            ItemDTO item1 = this.testProduct1.GetProductDTO();

            
            double preStorePolicyPrice = (this.testProduct0.Price - this.testProduct0.Price / 100 * this.testProduct0.Sale) * this.testProduct0.Quantity / 2; // divide by 2 due to product purchase policy
            item0.SetPrice(preStorePolicyPrice);
            preStorePolicyPrice = (this.testProduct1.Price - this.testProduct1.Price / 100 * this.testProduct1.Sale) * this.testProduct1.Quantity / 2; // divide by 2 due to product purchase policy
            item1.SetPrice(preStorePolicyPrice);
            double finalPrice = 0;
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { item0, item1 };
            foreach (Purchase_Policy p in testStore.storePolicies.Values)
                itemsToCalculate = p.ApplyPolicy(itemsToCalculate);

            finalPrice = itemsToCalculate.Aggregate(0.0, (acc, i) => acc += i.Price);


            // Act
            double retPrice = this.facade.CalculatePrice(itemsToCalculate);

            // Assert
            Assert.IsTrue((finalPrice - retPrice) < 0.01);    
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
            this.testStore.ChangeProductQuantity(this.testStore.founderID, this.testProduct0.Product_ID, 0);
            this.testProduct0.SetQuantity(0);
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO(), this.testProduct1.GetProductDTO() };
            bool errorCannotPurchase = false;

            // Act
            try
            {
                this.facade.Purchase("PurchaseStoreFacadeTestUserID0", itemsToCalculate);
            }catch (Exception ex) { errorCannotPurchase = true; }

            // Assert
            Assert.IsTrue(errorCannotPurchase);
        }

        [TestMethod]
        public void GatherStoresWithProductsByItemsStoreFacadeTestSuccess() // no fail test
        {
            // Arrange
            Store newProductStore = GetStore("testStoreFounderID971");
            Product testProduct2 = GetNewProduct(newProductStore);
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
            retStoresWIthProduct.TryGetValue(newProductStore.Store_ID, out outNewStore);

            Assert.IsTrue(retStoresWIthProduct.ContainsKey(this.testStore.Store_ID));
            Assert.IsTrue(retStoresWIthProduct.ContainsKey(newProductStore.Store_ID));

            
            
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



        private Store GetStore(string founder)
        {
            string founderID = founder;
            StoreDTO newStore = StoreFacade.GetInstance().AddNewStore(founderID, new List<string>() { "policyTestStoreName" });
            return StoreRepo.GetInstance().getStore(newStore.StoreID);
        }

        private Product GetNewProduct(Store productStore)
        {
            // productProperties = {Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale ,Weight, Dimenssions, PurchaseAttributes, ProductCategory}
            // ProductAttributes = atr1Name:atr1opt1_atr1opt2...atr1opti;atr2name:atr2opt1...
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"};
            ConcurrentDictionary<string, Purchase_Policy> defaultStorePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            ConcurrentDictionary<string, Purchase_Strategy> defaultStoreStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();

            Product newP0 = new Product(productProperties, productStore.Store_ID, defaultStorePolicies, defaultStoreStrategies);

            Statement storeIDStatement = new EqualRelation("Name", "testProduct0Name", false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });
            string samestatement = "[AtLeast[[1][Equal[[Name][testProduct0Name]]]]]";
            Purchase_Policy testProduct0Policy = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", samestatement, newP0.Product_ID);

            //Purchase_Policy testProduct0Policy = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", statement, newP0.Product_ID);

            string formula = "[   IfThen[ [Equal[ [Category] [WhateverCategory] ] ]  [GreaterThan[ [Quantity]  [1] ] ] ] ]";
            Purchase_Strategy testProduct0Strategy = new Purchase_Strategy("AddStoreStrategySuccessStrategyID1", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);

            newP0.AddPurchasePolicy(testProduct0Policy);
            newP0.AddPurchaseStrategy(testProduct0Strategy);
            return newP0;
        }

        private Product GetExistingProduct(Store productStore)
        {
            String product_ID = productStore.Store_ID + "_tesProduct1ID";
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

            Dictionary<string, List<string>> product_Attributes = new Dictionary<string, List<string>>();
            List<string> attr1 = new List<string>() { "testProduct1Atr1Opt1", "testProduct1Atr1Opt2" };
            List<string> attr2 = new List<string>() { "testProduct1Atr2Opt1", "testProduct1Atr2Opt2", "testProduct1Atr2Opt3" };
            product_Attributes.Add("testProduct1Atr1", attr1);
            product_Attributes.Add("testProduct1Atr2", attr2);

            int boughtTimes = 11;
            Category category = new Category("testProduct1SomeCategory");
            Product newp1 = new Product(product_ID, name, description, price, initQuantity, reservedQuantity, rating, sale, weight,
                                dimenssions, comments, defaultStorePolicies, defaultStoreStrategies, product_Attributes, boughtTimes, category, 11, new KeyValuePair<string, double>(product_ID, -1.0));

            Statement storeIDStatement = new EqualRelation("Name", newp1.Name, false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });
            string samestatement = "[AtLeast[[1][Equal[[Name][" + newp1.Name  + "]]]]]";
            Purchase_Policy testProduct1Policy = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", samestatement, newp1.Product_ID);

            //Purchase_Policy testProduct1Policy = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", statement, newp1.Product_ID);

            string formula = "[   IfThen[ [Equal[ [Category] [WhateverCategory] ] ]  [GreaterThan[ [Quantity]  [1] ] ] ] ]";
            Purchase_Strategy testProduct1Strategy = new Purchase_Strategy("AddStoreStrategySuccessStrategyID1", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);

            newp1.AddPurchasePolicy(testProduct1Policy);
            newp1.AddPurchaseStrategy(testProduct1Strategy);

            return newp1;
        }

    }
}