using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Tests.unit_tests.StoreComponentUnitTests
{
    [TestClass]
    public class PurchasePolicyUnitTest
    {



        private Store testStore; // uses Builder of a new Product 
        private Product testProduct0;
        //private Product testProduct1;
        private Employees testEmployees;
        private StoreFacade facade;
        private String legitTestUser1 = "legitTestUser1";
        private String legitTestUser2 = "legitTestUser2";



        [TestInitialize()]
        public void Setup()
        {
            facade = StoreFacade.GetInstance();
            testStore = GetStore();
            testProduct0 = GetNewProduct(testStore.Store_ID, testStore.founderID); // added via facadee with storeID         

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
        public void ValidateUserAllowedToCalculateStoreStrategyTestSuccess() 
        {
            // Arrange
            this.testProduct0.SetSale(50);
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO() };
            double price = this.testProduct0.Price / 2 * this.testProduct0.Quantity / 2; // divide by 2 for sale and by another 2 for store sale policy
            double retPrice = 0;
            bool error = false;

            // Act
            try
            {
                retPrice = this.facade.CalculatePrice(itemsToCalculate);
            } catch (Exception ex) { error = true; }

            // Assert
            Assert.IsFalse(error);
            Assert.IsTrue((price - retPrice) < 0.001);
        }

        [TestMethod]
        public void ValidateUserAllowedToPurchaseStoreStrategyTestSuccess()
        {
            // Arrange            
            this.testProduct0.SetSale(50);
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO() };
            double price = this.testProduct0.Price / 2 * this.testProduct0.Quantity;
            bool error = false;

            // Act
            try
            {
                this.facade.Purchase(this.legitTestUser1, itemsToCalculate);
            }
            catch (Exception ex) { error = true; }
            this.testProduct0 = StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID);
            
            // Assert
            Assert.IsFalse(error);
            Assert.AreEqual(this.testProduct0.Quantity, 0);
        }

        [TestMethod]
        public void ValidateUserAllowedToPurchaseStoreStrategyTestFail()
        {
            // Arrange            
            this.testProduct0.SetSale(50);
            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO() };
            double price = this.testProduct0.Price / 2 * this.testProduct0.Quantity;
            bool error = false;
            Exception exc = null;

            // Act
            try
            {
                this.facade.Purchase("someUser", itemsToCalculate);
            }
            catch (Exception ex) { error = true; exc = ex; }
            this.testProduct0 = StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID);

            // Assert
            Assert.IsTrue(error);
            Assert.AreEqual(this.testProduct0.Quantity, itemsToCalculate[0].GetQuantity());
            Assert.AreEqual(exc.InnerException.Message, "Restrictions violated: Test strategy policy description.");
        }


        [TestMethod]
        public void ValidateUserAllowedToPurchaseProductStrategyTestSuccess()
        {
            // Arrange            
            this.testProduct0.SetSale(50);




            List<ItemDTO> itemsToCalculate = new List<ItemDTO>() { this.testProduct0.GetProductDTO() };
            double price = this.testProduct0.Price / 2 * this.testProduct0.Quantity;
            bool error = false;
            Exception exc = null;

            // Act
            try
            {
                this.facade.Purchase("someUser", itemsToCalculate);
            }
            catch (Exception ex) { error = true; exc = ex; }
            this.testProduct0 = StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID);

            // Assert
            Assert.IsTrue(error);
            Assert.AreEqual(this.testProduct0.Quantity, itemsToCalculate[0].GetQuantity());
            Assert.AreEqual(exc.InnerException.Message, "Restrictions violated: Test strategy policy description.");
        }




        // ========================================= Formula as String Tests =========================================

        [TestMethod]
        public void AlcoholAgeStoreStrategySuccess()
        {
            // Arrange
            facade.ChangeProductCategory(this.testStore.founderID, this.testProduct0.Product_ID, new Category("Alcohol"));
            List<ItemDTO> itemsToPurchase = new List<ItemDTO>() { StoreRepo.GetInstance().GetProduct(this.testProduct0.Product_ID).GetProductDTO() };
            string formula = "[   IfThen[ [Equal[ [Category] [Alcohol] ] ]  [GreaterThan[ [User.Age]  [18] ] ] ] ]";
            Purchase_Strategy alcoholAgeGreaterThan18 = new Purchase_Strategy("AddStoreStrategySuccessStrategyID1", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);
            facade.AddStorePurchaseStrategy(this.testStore.founderID, this.testStore.Store_ID, alcoholAgeGreaterThan18);
            bool error = false;

            // Act
            try
            {
                this.facade.Purchase("legitTestUser1", itemsToPurchase);
            }
            catch (Exception ex) { error = true;}
            this.testProduct0 = StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID);

            // Assert
            Assert.IsTrue(error);
            Assert.AreEqual(this.testProduct0.Quantity, itemsToPurchase[0].GetQuantity());
        }

        [TestMethod]
        public void QuantityLessThanStoreStrategySuccess()
        {
            // Arrange
            facade.ChangeProductCategory(this.testStore.founderID, this.testProduct0.Product_ID, new Category("Alcohol"));
            List<ItemDTO> itemsToPurchase = new List<ItemDTO>() { StoreRepo.GetInstance().GetProduct(this.testProduct0.Product_ID).GetProductDTO() };
            string formula = "[AND[     [ IfThen[ [Equal[ [Name] [testProduct0Name] ] ]  [SmallerThan[ [Quantity]  [500] ] ] ] ]    [AtLeast[ [9] [Equal[[Category][testProduct0SomeCategory]]]]]    ]]";
            Purchase_Strategy quantityAndCategory = new Purchase_Strategy("AddStoreStrategySuccessStrategyID1", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);
            facade.AddStorePurchaseStrategy(this.testStore.founderID, this.testStore.Store_ID, quantityAndCategory);
            bool error = false;

            // Act
            try
            {
                this.facade.Purchase("legitTestUser1", itemsToPurchase);
            }
            catch (Exception ex) { error = true; }
            this.testProduct0 = StoreRepo.GetInstance().getProduct(this.testProduct0.Product_ID);

            // Assert
            Assert.IsTrue(error);
            Assert.AreEqual(this.testProduct0.Quantity, itemsToPurchase[0].GetQuantity());
        }








        // ========================================= END of Tests =========================================
        // ================================================================================================



        private Store GetStore()
        {
            string founderID = "testStoreFounderID326";

            StoreDTO newStore = this.facade.AddNewStore(founderID, new List<string>() { "policyTestStoreName"});

            Statement storeIDStatement = new EqualRelation("StoreID", newStore.StoreID, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });

            Purchase_Policy testStorePolicy = new StorePolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", newStore.StoreID, statement);

            this.facade.AddStorePurchasePolicy(newStore.FounderID, newStore.StoreID, testStorePolicy);

            Statement userIDStatement1 = new EqualRelation("Username", this.legitTestUser1, true);
            Statement userIDStatement2 = new EqualRelation("Username", this.legitTestUser2, true);
            Statement[] usersFormula = new Statement[] { userIDStatement1, userIDStatement2};
            Statement logicOrFormula = new LogicOR(usersFormula);
            Purchase_Strategy testStoreStrategy = new Purchase_Strategy("policyTestsStrategyID1", "userIDEqualslegitUsersIDs", "Test strategy policy description.", logicOrFormula);
            this.facade.AddStorePurchaseStrategy(founderID, newStore.StoreID, testStoreStrategy);

            return StoreRepo.GetInstance().getStore(newStore.StoreID);
        }

        private Product GetNewProduct(string store, string founder)
        {
            // productProperties = {Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale ,Weight, Dimenssions, PurchaseAttributes, ProductCategory}
            // ProductAttributes = atr1Name:atr1opt1_atr1opt2...atr1opti;atr2name:atr2opt1...
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "23", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"};
            ItemDTO newItem = facade.AddProductToStore(store, testStore.founderID, productProperties);
            return StoreRepo.GetInstance().getProduct(newItem.GetID());
        }

        /*
        private Product GetExistingProduct(string productid)
        {
            Purchase_Policy testProduct1Policy = new Purchase_Policy("testProduct1Policy1ID", "testProduct1Policy1Name", 100, 0, 50);
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
        */
    }
}


