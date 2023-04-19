using Microsoft.VisualStudio.TestTools.UnitTesting;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using Market_System.DomainLayer.UserComponent;
using System.Web.UI.WebControls;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class ProductUnitTest
    {
        // fields          
        private Product testProduct0; // uses Builder of a new Product 
        private Product testProduct1; // uses Builder oa an existing Product        
        StoreRepo repo;
        Store store0;
        Store store1;

    // before each test 
    [TestInitialize()]
        public void Setup()
        {
            store0 = GetStore("testProduct0StoreID789");
            store1 = GetStore("testProduct1StoreID465");
            this.repo = StoreRepo.GetInstance();
            repo.AddStore(store0.founderID, store0);
            repo.AddStore(store1.founderID, store1);
            testProduct0 = GetNewProduct();
            testProduct1 = GetExistingProduct();            
            ConcurrentDictionary<string, string> store0AllProducts = new ConcurrentDictionary<string, string>();
            ConcurrentDictionary<string, string> store1AllProducts = new ConcurrentDictionary<string, string>();
            store0AllProducts.TryAdd(this.testProduct0.Product_ID, this.testProduct0.Product_ID);
            store1AllProducts.TryAdd(this.testProduct1.Product_ID, this.testProduct1.Product_ID);
            store0.allProducts = store0AllProducts;
            store1.allProducts = store1AllProducts;
            this.repo.AddProduct(store0.Store_ID, store0.founderID,  this.testProduct0, testProduct0.Quantity);
            this.repo.AddProduct(store1.Store_ID, store1.founderID, this.testProduct1, testProduct1.Quantity);
        }

        // after each test
        [TestCleanup()]
        public void TearDown()
        {
            repo.destroy();
        }   

        [TestMethod]
        public void AddPurchasePolicyProductTestSuccess()
        {
            // Arrange
            Purchase_Policy purchase_Policy0 = new Purchase_Policy("AddPurchasePolicyProductTest_Policy0ID", "AddPurchasePolicyProductTest_Policy0Name");
            Purchase_Policy purchase_Policy1 = new Purchase_Policy("AddPurchasePolicyProductTest_Policy1ID", "AddPurchasePolicyProductTest_Policy1Name");

            // Act
            this.testProduct0.AddPurchasePolicy(purchase_Policy0);
            this.testProduct1.AddPurchasePolicy(purchase_Policy1);

            // Assert
            Assert.IsTrue(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy0.GetID()));
            Assert.IsTrue(testProduct1.PurchasePolicies.ContainsKey(purchase_Policy1.GetID()));

            Assert.IsFalse(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy1.GetID()));
            Assert.IsFalse(testProduct1.PurchasePolicies.ContainsKey(purchase_Policy0.GetID()));
        }

        [TestMethod]
        public void AddPurchasePolicyAlreadyExistProductTest()
        {
            // Arrange
            Purchase_Policy purchase_PolicyBoth = new Purchase_Policy("AddPurchasePolicyProductTest_Policy_BOTH_ID", "AddPurchasePolicyProductTest_Policy_BOTH_Name");
            this.testProduct0.AddPurchasePolicy(purchase_PolicyBoth);
            this.testProduct1.AddPurchasePolicy(purchase_PolicyBoth);
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct0.AddPurchasePolicy(purchase_PolicyBoth);
            } catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.AddPurchasePolicy(purchase_PolicyBoth);
            }
            catch (Exception ex) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void RemovePurchasePolicyProductTestSuccess()
        {
            // Arrange
            Purchase_Policy purchase_Policy0 = new Purchase_Policy("RemovePurchasePolicyProductTest_Policy0ID", "RemovePurchasePolicyProductTest_Policy0Name");
            Purchase_Policy purchase_Policy1 = new Purchase_Policy("RemovePurchasePolicyProductTest_Policy1ID", "RemovePurchasePolicyProductTest_Policy1Name");
            this.testProduct0.AddPurchasePolicy(purchase_Policy0);
            this.testProduct1.AddPurchasePolicy(purchase_Policy1);

            // Act        
            this.testProduct0.RemovePurchasePolicy(purchase_Policy0.GetID());
            this.testProduct1.RemovePurchasePolicy(purchase_Policy1.GetID());

            // Assert
            Assert.IsFalse(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy0.GetID()));
            Assert.IsFalse(testProduct1.PurchasePolicies.ContainsKey(purchase_Policy1.GetID()));
        }

        [TestMethod]
        public void RemovePurchasePolicyDoesntExistProductTestFail()
        {
            // Arrange
            Purchase_Policy purchase_PolicyBoth = new Purchase_Policy("RemovePurchasePolicyProductTest_Policy_BOTH_ID", "RemovePurchasePolicyProductTest_Policy_BOTH_Name");
            bool error0 = false;
            bool error1 = false;
            this.testProduct0.AddPurchasePolicy(purchase_PolicyBoth);
            this.testProduct1.AddPurchasePolicy(purchase_PolicyBoth);
            this.testProduct0.RemovePurchasePolicy(purchase_PolicyBoth.GetID());
            this.testProduct1.RemovePurchasePolicy(purchase_PolicyBoth.GetID());

            // Act        
            try
            {
                this.testProduct0.RemovePurchasePolicy(purchase_PolicyBoth.GetID());

            }
            catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.RemovePurchasePolicy(purchase_PolicyBoth.GetID());

            }
            catch (Exception ex) { error1 = true; }


            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void AddPurchaseStrategyProductTest()
        {
            // Arrange
            Purchase_Strategy purchase_Strategy0 = new Purchase_Strategy("AddPurchaseStrategyProductTest_Strategy0ID", "AddPurchaseStrategyProductTest_Strategy0Name");
            Purchase_Strategy purchase_Strategy1 = new Purchase_Strategy("AddPurchaseStrategyProductTest_Strategy1ID", "AddPurchaseStrategyProductTest_Strategy1Name");

            // Act
            this.testProduct0.AddPurchaseStrategy(purchase_Strategy0);
            this.testProduct1.AddPurchaseStrategy(purchase_Strategy1);

            // Assert
            Assert.IsTrue(testProduct0.PurchaseStrategies.ContainsKey(purchase_Strategy0.GetID()));
            Assert.IsTrue(testProduct1.PurchaseStrategies.ContainsKey(purchase_Strategy1.GetID()));

            Assert.IsFalse(testProduct0.PurchaseStrategies.ContainsKey(purchase_Strategy1.GetID()));
            Assert.IsFalse(testProduct1.PurchaseStrategies.ContainsKey(purchase_Strategy0.GetID()));
        }

        [TestMethod]
        public void AddPurchaseStrategyAlreadyExistProductTestFail()
        {
            // Arrange
            Purchase_Strategy purchase_StrategyBoth = new Purchase_Strategy("AddPurchaseStrategyProductTest_Strategy_BOTH_ID", "AddPurchaseStrategyProductTest_Strategy_BOTH_Name");
            bool error0 = false;
            bool error1 = false;
            this.testProduct0.AddPurchaseStrategy(purchase_StrategyBoth);
            this.testProduct1.AddPurchaseStrategy(purchase_StrategyBoth);

            // Act
            try
            {
                this.testProduct0.AddPurchaseStrategy(purchase_StrategyBoth);
            }
            catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.AddPurchaseStrategy(purchase_StrategyBoth);
            }
            catch (Exception ex) { error1 = true; }


            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void RemovePurchaseStrategyProductTestSuccess()
        {
            // Arrange
            Purchase_Strategy purchase_Strategy0 = new Purchase_Strategy("RemovePurchaseStrategyProductTest_Strategy0ID", "RemovePurchaseStrategyProductTest_Strategy0Name");
            Purchase_Strategy purchase_Strategy1 = new Purchase_Strategy("RemovePurchaseStrategyProductTest_Strategy1ID", "RemovePurchaseStrategyProductTest_Strategy1Name");

            this.testProduct0.AddPurchaseStrategy(purchase_Strategy0);
            this.testProduct1.AddPurchaseStrategy(purchase_Strategy1);

            // Act
            this.testProduct0.RemovePurchaseStrategy(purchase_Strategy0.GetID());
            this.testProduct1.RemovePurchaseStrategy(purchase_Strategy1.GetID());

            // Assert
            Assert.IsFalse(testProduct0.PurchaseStrategies.ContainsKey(purchase_Strategy0.GetID()));
            Assert.IsFalse(testProduct1.PurchaseStrategies.ContainsKey(purchase_Strategy1.GetID()));
        }

        [TestMethod]
        public void RemovePurchaseStrategyDoesntExistProductTestFail()
        {
            // Arrange
            Purchase_Strategy purchase_StrategyBoth = new Purchase_Strategy("RemovePurchaseStrategyProductTest_Strategy_BOTH_ID", "RemovePurchaseStrategyProductTest_Strategy_BOTH_Name");
            bool error0 = false;
            bool error1 = false;
            this.testProduct0.AddPurchaseStrategy(purchase_StrategyBoth);
            this.testProduct1.AddPurchaseStrategy(purchase_StrategyBoth);
            this.testProduct0.RemovePurchaseStrategy(purchase_StrategyBoth.GetID());
            this.testProduct1.RemovePurchaseStrategy(purchase_StrategyBoth.GetID());

            // Act
            try
            {
                this.testProduct0.RemovePurchaseStrategy(purchase_StrategyBoth.GetID());
            }
            catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.RemovePurchaseStrategy(purchase_StrategyBoth.GetID());
            }
            catch (Exception ex) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void AddAttributeProductTestSuccess()
        {
            // Arrange
            string attr0 = "AddAttributeProductTestAtr0";
            string attr1 = "AddAttributeProductTestAtr1";
            List<string> attr0Opts = new List<string>() { "AddAttributeProductTestAtr0Opt1", "AddAttributeProductTestAtr0Opt2" };
            List<string> attr1Opts = new List<string>() { "AddAttributeProductTestAtr1Opt1", "AddAttributeProductTestAtr1Opt2", "AddAttributeProductTestAtr1Opt3" };

            // Act
            this.testProduct0.AddAtribute(attr0, attr0Opts);
            this.testProduct1.AddAtribute(attr1, attr1Opts);

            // Assert
            Assert.IsFalse(testProduct0.PurchaseAttributes.ContainsKey(attr1));
            Assert.IsFalse(testProduct1.PurchaseAttributes.ContainsKey(attr0));

            Assert.IsTrue(testProduct0.PurchaseAttributes.ContainsKey(attr0));
            Assert.IsTrue(testProduct1.PurchaseAttributes.ContainsKey(attr1));
        }

        [TestMethod]
        public void AddAttributeProductTestFail()
        {
            // Arrange
            string attr0 = "AddAttributeProductTestAtr0";
            string attr1 = "AddAttributeProductTestAtr1";
            List<string> attr0Opts = new List<string>() { "AddAttributeProductTestAtr0Opt1", "AddAttributeProductTestAtr0Opt2" };
            List<string> attr1Opts = new List<string>() { "AddAttributeProductTestAtr1Opt1", "AddAttributeProductTestAtr1Opt2", "AddAttributeProductTestAtr1Opt3" };
            bool error0 = false;
            bool error1 = false;
            this.testProduct0.AddAtribute(attr0, attr0Opts);
            this.testProduct1.AddAtribute(attr1, attr1Opts);

            // Act
            try
            {
                this.testProduct0.AddAtribute(attr0, attr0Opts);
            }
            catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.AddAtribute(attr1, attr1Opts);
            }
            catch (Exception ex) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void RemoveAttributeProductTestSuccess()
        {
            // Arrange
            string attr0 = "RemoveAttributeProductTestAtr0";
            string attr1 = "RemoveAttributeProductTestAtr1";
            List<string> attr0Opts = new List<string>() { "RemoveAttributeProductTestAtr0Opt1", "RemoveAttributeProductTestAtr0Opt2" };
            List<string> attr1Opts = new List<string>() { "RemoveAttributeProductTestAtr1Opt1", "RemoveAttributeProductTestAtr1Opt2", "RemoveAttributeProductTestAtr1Opt3" };
            this.testProduct0.AddAtribute(attr0, attr0Opts);
            this.testProduct1.AddAtribute(attr1, attr1Opts);

            // Act
            this.testProduct0.RemoveAttribute(attr0);
            this.testProduct1.RemoveAttribute(attr1);


            // Assert
            Assert.IsFalse(testProduct0.PurchaseAttributes.ContainsKey(attr0));
            Assert.IsFalse(testProduct1.PurchaseAttributes.ContainsKey(attr1));

            Assert.IsTrue(testProduct0.PurchaseAttributes.ContainsKey("testProduct0Atr1")); // init attribute
            Assert.IsTrue(testProduct1.PurchaseAttributes.ContainsKey("testProduct1Atr1")); // init attribute
        }

        [TestMethod]
        public void RemoveAttributeDoesntExistProductTestFail()
        {
            // Arrange
            string attr0 = "RemoveAttributeProductTestAtr0";
            string attr1 = "RemoveAttributeProductTestAtr1";
            List<string> attr0Opts = new List<string>() { "RemoveAttributeProductTestAtr0Opt1", "RemoveAttributeProductTestAtr0Opt2" };
            List<string> attr1Opts = new List<string>() { "RemoveAttributeProductTestAtr1Opt1", "RemoveAttributeProductTestAtr1Opt2", "RemoveAttributeProductTestAtr1Opt3" };
            bool error0 = false;
            bool error1 = false;
            this.testProduct0.AddAtribute(attr0, attr0Opts);
            this.testProduct1.AddAtribute(attr1, attr1Opts);
            this.testProduct0.RemoveAttribute(attr0);
            this.testProduct1.RemoveAttribute(attr1);

            // Act
            try
            {
                this.testProduct0.RemoveAttribute(attr0);
            }
            catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.RemoveAttribute(attr1);
            }
            catch (Exception ex) { error1 = true; }

            // Assert
            Assert.IsTrue(error0); // init attribute
            Assert.IsTrue(error1); // init attribute
        }

        [TestMethod]
        public void GetStoreIDProductTest() // no fail test for this
        {
            // Arrange
            string p0StoreID = "testProduct0StoreID789"; // init parameter
            string p1StoreID = "testProduct1StoreID465"; // init parameter

            // Act
            string out0 = this.testProduct0.GetStoreID();
            string out1 = this.testProduct1.GetStoreID();


            // Assert
            Assert.AreEqual(p0StoreID, out0);
            Assert.AreEqual(p1StoreID, out1);
        }

        [TestMethod]
        public void ImplementSaleProductTest()  // need to implement Purchase Policy / Strategy !!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            // Arrange
            double p0Sale = testProduct0.Sale; // init parameter
            double p1Sale = testProduct1.Sale; // init parameter
            double p0Price = testProduct0.Price; // init parameter
            double p1Price = testProduct1.Price; // init parameter


            List<string> fakeChosenAttributes = new List<string>() { "attr1" }; // this functionality doesn't support / consider attributes choice yet

            double afterSale0 = p0Price - (p0Price / 100 * p0Sale);
            double afterSale1 = p1Price - (p1Price / 100 * p1Sale);

            // Act
            double out0 = this.testProduct0.ImplementSale(fakeChosenAttributes);
            double out1 = this.testProduct1.ImplementSale(fakeChosenAttributes);


            // Assert
            Assert.AreEqual(afterSale0, out0);
            Assert.AreEqual(afterSale1, out1);
        }

        [TestMethod]
        public void CalculatePriceWithSaleProductTestSuccess()
        {
            // Arrange
            int quantityToBuy0 = 10;
            int quantityToBuy1 = 5;

            double p0Sale = testProduct0.Sale; // init parameter
            double p1Sale = testProduct1.Sale; // init parameter
            double p0Price = testProduct0.Price; // init parameter
            double p1Price = testProduct1.Price; // init parameter

            double priceAfterSale0 = (p0Price - (p0Price / 100 * p0Sale)) * quantityToBuy0;
            double priceAfterSale1 = (p1Price - (p1Price / 100 * p1Sale)) * quantityToBuy1;


            // Act
            double out0WithSale = this.testProduct0.CalculatePrice(quantityToBuy0, true);
            double out1WithSale = this.testProduct1.CalculatePrice(quantityToBuy1, true); ;


            // Assert
            Assert.AreEqual(priceAfterSale0, out0WithSale);
            Assert.AreEqual(priceAfterSale1, out1WithSale);
        }

        [TestMethod]
        public void CalculatePriceWithoutSaleProductTestSuccess()
        {
            // Arrange
            int quantityToBuy0 = 10;
            int quantityToBuy1 = 5;
            double p0Price = testProduct0.Price; // init parameter
            double p1Price = testProduct1.Price; // init parameter

            double priceBeforeSale0 = p0Price * quantityToBuy0;
            double priceBeforeSale1 = p1Price * quantityToBuy1;

            // Act
            double out0WithoutSale = this.testProduct0.CalculatePrice(quantityToBuy0, false);
            double out1WithoutSale = this.testProduct1.CalculatePrice(quantityToBuy1, false);

            // Assert
            Assert.AreEqual(priceBeforeSale0, out0WithoutSale);
            Assert.AreEqual(priceBeforeSale1, out1WithoutSale);
        }

        [TestMethod]
        public void CalculatePriceBadQuantityProductTestFail()
        {
            // Arrange
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct0.CalculatePrice(0, false);
            }
            catch (Exception ex) { error0 = true; }

            try
            {
                this.testProduct1.CalculatePrice(0, false);
            }
            catch (Exception ex) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void prePurchaseProductTestSuccess()
        {
            // Arrange
            int quantityToBuy0 = this.testProduct0.Quantity + 2;
            int quantityToBuy1 = this.testProduct1.Quantity / 2;

            // Act
            bool out0 = this.testProduct0.prePurchase(quantityToBuy0);
            bool out1 = this.testProduct1.prePurchase(quantityToBuy1);

            // Assert
            Assert.IsFalse(out0);
            Assert.IsTrue(out1);
        }

        [TestMethod]
        public void prePurchaseBadQuantityProductTestFail()
        {
            // Arrange
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                bool out0 = this.testProduct0.prePurchase(0);
            }
            catch (Exception ex) { error0 = true; error1 = true; }

            try
            {
                bool out1 = this.testProduct1.prePurchase(-2);
            }
            catch (Exception ex) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
        }

        [TestMethod]
        public void EnoughQuantityPurchaseProductTestSuccess()
        {
            // Arrange
            int initQuantity0 = this.testProduct0.Quantity; // init parameter
            int initQuantity1 = this.testProduct1.Quantity; // init parameter
            int quantityToBuy0 = initQuantity0 / 2;
            int quantityToBuy1 = initQuantity1 / 2;
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct0.Purchase(quantityToBuy0);

            }
            catch (Exception e) { error0 = true; }

            try
            {
                this.testProduct1.Purchase(quantityToBuy1);
            }
            catch (Exception e) { error1 = true; }

            // Assert
            Assert.IsFalse(error0);
            Assert.IsFalse(error1);
            Assert.AreEqual(initQuantity0 - quantityToBuy0, this.testProduct0.Quantity);
            Assert.AreEqual(initQuantity1 - quantityToBuy1, this.testProduct1.Quantity);
        }

        [TestMethod]
        public void NOTEnoughQuantityPurchaseProductTestFail()
        {
            // Arrange
            int initQuantity0 = this.testProduct0.Quantity; // init parameter
            int initQuantity1 = this.testProduct1.Quantity; // init parameter
            int quantityToBuy0 = initQuantity0 + 2;
            int quantityToBuy1 = initQuantity1 + 2;
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct0.Purchase(quantityToBuy0);

            }
            catch (Exception e) { error0 = true; }

            try
            {
                this.testProduct1.Purchase(quantityToBuy1);
            }
            catch (Exception e) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
            Assert.AreEqual(initQuantity0, this.testProduct0.Quantity);
            Assert.AreEqual(initQuantity1, this.testProduct1.Quantity);
        }

        [TestMethod]
        public void EnoughQuantityToReserveProductTestSuccess()
        {
            // Arrange
            int initQuantity0 = this.testProduct0.Quantity; // init parameter
            int initQuantity1 = this.testProduct1.Quantity; // init parameter            
            int initReservedQuantity0 = this.testProduct0.ReservedQuantity; // = 0: init parameter
            int initReservedQuantity1 = this.testProduct1.ReservedQuantity; // = 12: init parameter
            int quantityToReserve0 = initQuantity0 / 2;
            int quantityToReserve1 = 2;
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct0.Reserve(quantityToReserve0);

            }
            catch (Exception e) { error0 = true; }

            try
            {
                this.testProduct1.Purchase(quantityToReserve1);
            }
            catch (Exception e) { error1 = true; }

            // Assert
            Assert.IsFalse(error0);
            Assert.IsFalse(error1);
            Assert.AreEqual(initReservedQuantity0 + quantityToReserve0, this.testProduct0.ReservedQuantity);
            Assert.AreEqual(initReservedQuantity1 + quantityToReserve1, this.testProduct1.ReservedQuantity);
        }

        [TestMethod]
        public void NOTEnoughQuantityToReserveProductTestFail()
        {
            // Arrange
            int initQuantity0 = this.testProduct0.Quantity; // init parameter
            int initQuantity1 = this.testProduct1.Quantity; // init parameter            
            int initReservedQuantity0 = this.testProduct0.ReservedQuantity; // = 0: init parameter
            int initReservedQuantity1 = this.testProduct1.ReservedQuantity; // = 12: init parameter
            int quantityToReserve0 = initQuantity0 - initReservedQuantity0 + 2;
            int quantityToReserve1 = initQuantity1 - initReservedQuantity1 + 2;
            bool error0 = false;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct0.Reserve(quantityToReserve0);

            }
            catch (Exception e) { error0 = true; }

            try
            {
                this.testProduct1.Purchase(quantityToReserve1);
            }
            catch (Exception e) { error1 = true; }

            // Assert
            Assert.IsTrue(error0);
            Assert.IsTrue(error1);
            Assert.AreEqual(initReservedQuantity0, this.testProduct0.ReservedQuantity);
            Assert.AreEqual(initReservedQuantity1, this.testProduct1.ReservedQuantity);
        }

        [TestMethod]
        public void LetGoProductTestSuccess()
        {
            // Arrange         
            int initReservedQuantity1 = this.testProduct1.ReservedQuantity; // = 12: init parameter
            int quantityToRelease1 = initReservedQuantity1 - 1;
            bool error1 = false;

            // Act
            try
            {
                this.testProduct1.LetGoProduct(quantityToRelease1);
            }
            catch (Exception e) { error1 = true; }

            // Assert
            Assert.IsFalse(error1); // everythin fine
            Assert.AreEqual(initReservedQuantity1 - quantityToRelease1, this.testProduct1.ReservedQuantity);
        }

        [TestMethod]
        public void LetGoTooManyUnitaProductTestFail()
        {
            // Arrange         
            int initReservedQuantity0 = this.testProduct0.ReservedQuantity; // = 0: init parameter
            int quantityToRelease0 = initReservedQuantity0 + 2;
            bool error0 = false;

            // Act
            try
            {
                this.testProduct0.LetGoProduct(quantityToRelease0);

            }
            catch (Exception e) { error0 = true; }

            // Assert
            Assert.IsTrue(error0); // cannot release more than reserved
            Assert.AreEqual(initReservedQuantity0, this.testProduct0.ReservedQuantity);
        }

        [TestMethod]
        public void AddCommentAndUpdateRatingProductTestSuccess()
        {
            // Arrange         
            string comment0 = "AddCommentAndUpdateRatingProductTestProduct0Comment";
            double rating0 = 8;
            string userID0 = "AddCommentAndUpdateRatingProductTestProduct0UserID0";
            double newRating0 = rating0;
            string newComment0 = userID0 + ": " + comment0 + ".\n Rating: " + rating0 + ".";

            // Act
            this.testProduct0.AddComment(userID0, comment0, rating0);

            // Assert
            Assert.IsTrue(this.testProduct0.Comments.ToList().Contains(newComment0));
            Assert.AreEqual(testProduct0.Rating, newRating0);
        }

        [TestMethod]
        public void AddCommentWithoutRatingProductTestSuccess()
        {
            // Arrange         
            string comment1 = "AddCommentAndUpdateRatingProductTestProduct0Comment";
            double rating1 = 0;
            string userID1 = "AddCommentAndUpdateRatingProductTestProduct1UserID1";
            double initRating1 = this.testProduct1.Rating;
            double newRating1 = (initRating1 * this.testProduct1.timesBought + rating1) / (this.testProduct1.timesBought + 1);
            string newComment1 = userID1 + ": " + comment1 + ".\n Rating: " + rating1 + ".";

            // Act
            this.testProduct1.AddComment(userID1, comment1, rating1);

            // Assert
            Assert.IsTrue(this.testProduct1.Comments.ToList().Contains(newComment1));
            Assert.AreEqual(testProduct1.Rating, newRating1);
        }

        [TestMethod]
        public void AddNoCommentWithoutRatingProductTestFail()
        {
            // Arrange         
            string comment1 = "      ";
            double rating1 = 0;
            string userID1 = "AddCommentAndUpdateRatingProductTestProduct1UserID1";
            double initRating1 = this.testProduct1.Rating;
            double newRating1 = (initRating1 * this.testProduct1.timesBought + rating1) / (this.testProduct1.timesBought + 1);
            string newComment1 = userID1 + ": " + comment1 + ".\n Rating: " + rating1 + ".";
            bool errorNoCommentNoRating = false;

            // Act
            try
            {
                this.testProduct1.AddComment(userID1, comment1, rating1);
            }
            catch (Exception e) { errorNoCommentNoRating = true; }

            // Assert
            Assert.IsFalse(this.testProduct1.Comments.ToList().Contains(newComment1));
            Assert.AreNotEqual(testProduct1.Rating, newRating1);
            Assert.IsTrue(errorNoCommentNoRating);
        }

        [TestMethod]
        public void GetProductDTOProductTestSuccess() // no fail test
        {
            // Arrange
            ItemDTO item = new ItemDTO(this.testProduct0);

            // Act
            ItemDTO retItem = this.testProduct0.GetProductDTO();

            // Assert
            Assert.AreEqual(retItem.GetID(), item.GetID()); // the builder in ItemDTO does not implemented yet
        }

        public void SetNameProductTestSuccess()
        {
            // Arrange         
            string name0 = "SetNameProductTestName0";

            // Act
            this.testProduct0.SetName(name0);

            // Assert
            Assert.AreEqual(testProduct0.Name, name0);
        }

        [TestMethod]
        public void SetEmptyNameProductTestFail()
        {
            // Arrange         
            string name1 = "           ";
            bool errorEmptyName = false;

            // Act
            try
            {
                this.testProduct1.SetName(name1);
            }
            catch (Exception e) { errorEmptyName = true; }

            // Assert
            Assert.IsTrue(errorEmptyName);
        }

        [TestMethod]
        public void SetDescriptionProductTestSuccess()
        {
            // Arrange         
            string Description0 = "SetNameProductTestDescription0";

            // Act
            this.testProduct0.SetDescription(Description0);

            // Assert
            Assert.AreEqual(testProduct0.Description, Description0);
        }

        [TestMethod]
        public void SetEmptyDescriptionProductTest()
        {
            // Arrange         
            string Description1 = "        ";
            bool errorEmptyDescription = false;

            // Act
            try
            {
                this.testProduct1.SetDescription(Description1);
            }
            catch (Exception e) { errorEmptyDescription = true; }

            // Assert
            Assert.IsTrue(errorEmptyDescription);
        }

        [TestMethod]
        public void SetPriceProductTestSuccess()
        {
            // Arrange         
            double Price0 = this.testProduct0.Price + 76;
            double Price1 = this.testProduct1.Price + 12;

            // Act
            this.testProduct0.SetPrice(Price0);
            this.testProduct1.SetPrice(Price1);

            // Assert
            Assert.AreEqual(testProduct0.Price, Price0);
            Assert.AreEqual(testProduct1.Price, Price1);
        }

        [TestMethod]
        public void SetNegativePriceProductTestFail()
        {
            // Arrange         
            bool negativePrice = false;

            // Act
            try
            {
                this.testProduct1.SetPrice(-1);
            }
            catch (Exception ex) { negativePrice = true; }

            // Assert
            Assert.IsTrue(negativePrice);
        }

        [TestMethod]
        public void SetRatingProductTestSuccess()
        {
            // Arrange         
            double Rating0 = 1;
            double Rating1 = 10;

            // Act
            this.testProduct0.SetRating(Rating0);
            this.testProduct1.SetRating(Rating1);

            // Assert
            Assert.AreEqual(testProduct0.Rating, Rating0);
            Assert.AreEqual(testProduct1.Rating, Rating1);
        }

        [TestMethod]
        public void SetSmallRatingProductTestFail()
        {
            // Arrange         
            bool badRatingLow = false;
            bool badRatingHigh = false;

            // Act
            try
            {
                this.testProduct1.SetRating(0.5);
            }
            catch (Exception ex) { badRatingLow = true; }
            try
            {
                this.testProduct0.SetRating(10.5);
            }
            catch (Exception ex) { badRatingHigh = true; }

            // Assert
            Assert.IsTrue(badRatingLow);
            Assert.IsTrue(badRatingHigh);
        }

        [TestMethod]
        public void SetQuantityProductTestSuccess()
        {
            // Arrange         
            int Quantity0 = this.testProduct0.Quantity + 10;
            int Quantity1 = this.testProduct1.Quantity + 15;

            // Act
            this.testProduct0.SetQuantity(Quantity0);
            this.testProduct1.SetQuantity(Quantity1);

            // Assert
            Assert.AreEqual(testProduct0.Quantity, Quantity0);
            Assert.AreEqual(testProduct1.Quantity, Quantity1);
        }

        [TestMethod]
        public void SetNegativeQuantityProductTestFail()
        {
            // Arrange         
            bool negativeQuantity = false;

            // Act
            try
            {
                this.testProduct1.SetQuantity(-1);
            }
            catch (Exception ex) { negativeQuantity = true; }

            // Assert
            Assert.IsTrue(negativeQuantity);
        }

        [TestMethod]
        public void SetWeightProductTestSuccess()
        {
            // Arrange         
            double weight0 = this.testProduct0.Weight + 10;
            double weight1 = this.testProduct1.Weight + 15;

            // Act
            this.testProduct0.SetWeight(weight0);
            this.testProduct1.SetWeight(weight1);

            // Assert
            Assert.AreEqual(testProduct0.Weight, weight0);
            Assert.AreEqual(testProduct1.Weight, weight1);
        }

        [TestMethod]
        public void SetNegativeWeightProductTestFail()
        {
            // Arrange         
            bool negative = false;

            // Act
            try
            {
                this.testProduct1.SetWeight(-1);
            }
            catch (Exception ex) { negative = true; }

            // Assert
            Assert.IsTrue(negative);
        }

        [TestMethod]
        public void SetSaleProductTestSuccess()
        {
            // Arrange         
            double Sale0 = Math.Min(this.testProduct0.Sale + 10, 99);
            double Sale1 = Math.Min(this.testProduct1.Sale + 15, 99);

            // Act
            this.testProduct0.SetSale(Sale0);
            this.testProduct1.SetSale(Sale1);

            // Assert
            Assert.AreEqual(testProduct0.Sale, Sale0);
            Assert.AreEqual(testProduct1.Sale, Sale1);
        }

        [TestMethod]
        public void SetNegativeSaleProductTestFail()
        {
            // Arrange         
            bool negative = false;

            // Act
            try
            {
                this.testProduct1.SetSale(-1);
            }
            catch (Exception ex) { negative = true; }

            // Assert
            Assert.IsTrue(negative);
        }

        [TestMethod]
        public void SetTimesBoughtProductTestSuccess()
        {
            // Arrange         
            long times0 = this.testProduct0.timesBought + 10;
            long times1 = this.testProduct1.timesBought + 15;

            // Act
            this.testProduct0.SetTimesBought(times0);
            this.testProduct1.SetTimesBought(times1);

            // Assert
            Assert.AreEqual(testProduct0.timesBought, times0);
            Assert.AreEqual(testProduct1.timesBought, times1);
        }

        [TestMethod]
        public void SetNegativeTimesBoughtProductTestFail()
        {
            // Arrange         
            bool negative = false;

            // Act
            try
            {
                this.testProduct1.SetTimesBought(-1);
            }
            catch (Exception ex) { negative = true; }

            // Assert
            Assert.IsTrue(negative);
        }

        [TestMethod]
        public void SetProductCategoryProductTestSuccess() // no fail test
        {
            // Arrange         
            Category cat0 = new Category("SetProductCategoryProductTestCategory0");
            Category cat1 = new Category("SetProductCategoryProductTestCategory1");

            // Act
            this.testProduct0.SetProductCategory(cat0);
            this.testProduct1.SetProductCategory(cat1);

            // Assert
            Assert.AreEqual(testProduct0.ProductCategory, cat0);
            Assert.AreEqual(testProduct1.ProductCategory, cat1);
        }

        [TestMethod]
        public void SetDefaultProductCategoryProductTestSuccess()
        {
            // Arrange         
            string noCategory = "NoCategory";

            // Act
            this.testProduct0.SetProductCategory(null);
            this.testProduct1.SetProductCategory(null);

            // Assert
            Assert.AreEqual(testProduct0.ProductCategory, noCategory);
            Assert.AreEqual(testProduct1.ProductCategory, noCategory);
        }

        [TestMethod]
        public void SetDimenssionsProductTestSuccess()
        {
            // Arrange         
            double[] dims0 = new double[] { 1.2, 2.3, 3.4 };
            double[] dims1 = new double[] { 4.5, 5.6, 6.7 };

            // Act
            this.testProduct0.SetDimenssions(dims0);
            this.testProduct1.SetDimenssions(dims1);

            // Assert
            Assert.AreEqual(testProduct0.Dimenssions, dims0);
            Assert.AreEqual(testProduct1.Dimenssions, dims1);
        }

        [TestMethod]
        public void SetBadDimenssionsProductTestFail()
        {
            // Arrange         
            double[] badDims0 = new double[] { 1.2, -2.3, 3.4 };
            double[] badDims1 = new double[] { 4.5, 5.6, -6.7 };
            bool negative0 = false;
            bool negative1 = false;

            // Act
            try
            {
                this.testProduct0.SetDimenssions(badDims0);
            }
            catch (Exception ex) { negative0 = true; }
            try
            {
                this.testProduct1.SetDimenssions(badDims1);
            }
            catch (Exception ex) { negative1 = true; }

            // Assert
            Assert.IsTrue(negative0);
            Assert.IsTrue(negative1);
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

            List<string> allProductsIDS = new List<string>();

            return new Store(founderID, storeID, policies, strategies, allProductsIDS, false);
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


        private class TestRepo : IStoreRepoMock
        {
            public ConcurrentDictionary<string, Product> productCache = new ConcurrentDictionary<string, Product>();
            private static object saveLock = new object();
            public void Save(Product toSave)
            {
                lock (saveLock)
                {
                    this.productCache.TryRemove(toSave.Product_ID, out _);
                    this.productCache.TryAdd(toSave.Product_ID, toSave);
                }
            }

            public void AddProduct(Product toAdd)
            {
                lock (saveLock)
                {
                    this.productCache.TryAdd(toAdd.Product_ID, toAdd);
                }
            }

            public void RemoveProduct(string toRemove)
            {
                lock (saveLock)
                {
                    this.productCache.TryRemove(toRemove, out _);
                }
            }

            public void Save(Store toSave)
            {
                throw new NotImplementedException();
            }
        }


    }

}




// ================ Mock example ================ 
/*
[TestMethod]
public void WhenUserDoesNotAgreeToContinueItDoesNotSendMessage()
{
    //Arrange
    var dialogServiceMock = new Mock<IDialogService>();
    var messengerMock = new Mock<IMessenger>();
    var viewModel = new ViewModel(dialogServiceMock.Object, messengerMock.Object);

    dialogServiceMock
     .Setup(x => x.ShowMessage(It.IsAny<string>()))
     .Returns(false);

    //Act
    viewModel.Execute();

    //Assert
    messengerMock.Verify(x => x.Send(It.IsAny<string>()), Times.Never());
}
*/