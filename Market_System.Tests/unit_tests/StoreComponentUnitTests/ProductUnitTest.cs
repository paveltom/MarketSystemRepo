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
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DAL;


namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class ProductUnitTest
    {
        
        // fields          
        private Product testProduct0; // uses Builder of a new Product 
        private Product testProduct1; // uses Builder oa an existing Product 
        private Store testStore;

    // before each test 
    [TestInitialize()]
        public void Setup()
        {
            testStore = GetStore();
            testProduct0 = GetNewProduct();
            testProduct1 = GetExistingProduct();            
        }

        // after each test
        [TestCleanup()]
        public void TearDown()
        {
            StoreFacade.GetInstance().Destroy_me();
            StoreRepo.GetInstance().RemoveDataBase("qwe123");
            StoreRepo.GetInstance().destroy();
        }   

        [TestMethod]
        public void AddPurchasePolicyProductTestSuccess()
        {
            // Arrange
            this.testProduct0.RemovePurchasePolicy("policyTestsPolicyID1");
            Statement storeIDStatement = new EqualRelation("ProductID", this.testProduct0.Product_ID, false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });

            string samestatement = "[AtLeast[[1][Equal[[ProductID][" + this.testProduct0.Product_ID + "]]]]]";
            Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", samestatement, this.testProduct0.Product_ID);

            //Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", statement, this.testProduct0.Product_ID);

            // Act
            this.testProduct0.AddPurchasePolicy(purchase_Policy0);

            // Assert
            Assert.IsTrue(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy0.PolicyID));
        }

        [TestMethod]
        public void AddPurchasePolicyAlreadyExistProductTest()
        {
            // Arrange
            Statement storeIDStatement = new EqualRelation("ProductID", this.testProduct0.Product_ID, false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });
            string samestatement = "[AtLeast[[1][Equal[[ProductID][" + this.testProduct0.Product_ID + "]]]]]";

            // Already added in initialization
            Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", samestatement, this.testProduct0.Product_ID);
            //Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", statement, this.testProduct0.Product_ID);

            bool error0 = false;

            // Act
            try
            {
                this.testProduct0.AddPurchasePolicy(purchase_Policy0);
            } catch (Exception ex) { error0 = true; }

            // Assert
            Assert.IsTrue(error0);
        }

        [TestMethod]
        public void RemovePurchasePolicyProductTestSuccess()
        {
            // Arrange
            Statement storeIDStatement = new EqualRelation("ProductID", this.testProduct0.Product_ID, false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });
            string samestatement = "[AtLeast[[1][Equal[[ProductID][" + this.testProduct0.Product_ID + "]]]]]";

            Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", samestatement, this.testProduct0.Product_ID);
            //Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", statement, this.testProduct0.Product_ID);
            // already added into Product in init

            // Act        
            this.testProduct0.RemovePurchasePolicy(purchase_Policy0.PolicyID);

            // Assert
            Assert.IsFalse(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy0.PolicyID));
        }

        [TestMethod]
        public void RemovePurchasePolicyDoesntExistProductTestFail()
        {
            // Arrange
            Statement storeIDStatement = new EqualRelation("ProductID", this.testProduct0.Product_ID, false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });
            string samestatement = "[AtLeast[[1][Equal[[ProductID][" + this.testProduct0.Product_ID + "]]]]]";
            
            Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", samestatement, this.testProduct0.Product_ID);
            //Purchase_Policy purchase_Policy0 = new ProductPolicy("policyTestsPolicyID1", "productStoreIDEqualsStoreID", 50, "Test sale policy description.", statement, this.testProduct0.Product_ID);

            bool error0 = false;
            this.testProduct0.RemovePurchasePolicy(purchase_Policy0.PolicyID); // already was added in init

            // Act        
            try
            {
                this.testProduct0.RemovePurchasePolicy(purchase_Policy0.PolicyID);

            }
            catch (Exception ex) { error0 = true; }

            // Assert
            Assert.IsTrue(error0);
        }

        [TestMethod]
        public void AddPurchaseStrategyProductTest()
        {
            // Arrange
            this.testProduct0.RemovePurchaseStrategy("AddStoreStrategySuccessStrategyID1");
            string formula = "[   IfThen[ [Equal[ [Category] [Alcohol] ] ]  [GreaterThan[ [User.Age]  [18] ] ] ] ]";
            Purchase_Strategy alcoholAgeGreaterThan18 = new Purchase_Strategy("AddStoreStrategySuccessStrategyID1", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);

            // Act
            this.testProduct0.AddPurchaseStrategy(alcoholAgeGreaterThan18);

            // Assert
            Assert.IsTrue(testProduct0.PurchaseStrategies.ContainsKey(alcoholAgeGreaterThan18.StrategyID));
        }

        [TestMethod]
        public void AddPurchaseStrategyAlreadyExistProductTestFail()
        {
            // Arrange
            string formula = "[   IfThen[ [Equal[ [Category] [Alcohol] ] ]  [GreaterThan[ [User.Age]  [18] ] ] ] ]";
            Purchase_Strategy alcoholAgeGreaterThan18 = new Purchase_Strategy("AddStoreStrategySuccessStrategyID1", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);

            bool error0 = false;

            // Act
            try
            {
                this.testProduct0.AddPurchaseStrategy(alcoholAgeGreaterThan18);
            }
            catch (Exception ex) { error0 = true; }

            // Assert
            Assert.IsTrue(error0);
        }

        [TestMethod]
        public void RemovePurchaseStrategyProductTestSuccess()
        {
            // Arrange
            string formula = "[   IfThen[ [Equal[ [Category] [Alcohol] ] ]  [GreaterThan[ [User.Age]  [18] ] ] ] ]";
            Purchase_Strategy alcoholAgeGreaterThan18 = new Purchase_Strategy("RemovePurchaseStrategyProductTestSuccessStrategyID", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);

            this.testProduct0.AddPurchaseStrategy(alcoholAgeGreaterThan18);

            // Act
            this.testProduct0.RemovePurchaseStrategy(alcoholAgeGreaterThan18.StrategyID);

            // Assert
            Assert.IsFalse(testProduct0.PurchaseStrategies.ContainsKey(alcoholAgeGreaterThan18.StrategyID));
        }

        [TestMethod]
        public void RemovePurchaseStrategyDoesntExistProductTestFail()
        {
            // Arrange
            string formula = "[   IfThen[ [Equal[ [Category] [Alcohol] ] ]  [GreaterThan[ [User.Age]  [18] ] ] ] ]";
            Purchase_Strategy alcoholAgeGreaterThan18 = new Purchase_Strategy("RemovePurchaseStrategyDoesntExistProductTestFailStrategyID", "AddStoreStrategySuccessStrategyName1", "AddStoreStrategySuccessStrategyDescription1", formula);
            bool error0 = false;

            this.testProduct0.AddPurchaseStrategy(alcoholAgeGreaterThan18);
            this.testProduct0.RemovePurchaseStrategy(alcoholAgeGreaterThan18.StrategyID);

            // Act
            try
            {
                this.testProduct0.RemovePurchaseStrategy(alcoholAgeGreaterThan18.StrategyID);
            }
            catch (Exception ex) { error0 = true; }

            // Assert
            Assert.IsTrue(error0);
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
            string p0StoreID = this.testProduct0.StoreID; // init parameter

            // Act
            string out0 = this.testProduct0.GetStoreID();

            // Assert
            Assert.AreEqual(p0StoreID, out0);
        }

        [TestMethod]
        public void ImplementSaleProductTest()  // need to implement Purchase Policy / Strategy !!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            // Arrange
            double p0Sale = testProduct0.Sale; // init parameter
            double p0Price = testProduct0.Price; // init parameter

            double afterSale0 = this.testProduct0.Quantity * (p0Price - (p0Price / 100 * p0Sale)) / 2; // divide by 2 - due to init purchase policy

            // Act
            double out0 = this.testProduct0.ImplementSale(testProduct0.GetProductDTO());

            // Assert
            Assert.IsTrue((afterSale0 - out0) < 0.001);
        }

        [TestMethod]
        public void CalculatePriceWithSaleProductTestSuccess()
        {
            // Arrange
            int quantityToBuy0 = 10;
            ItemDTO item0 = testProduct0.GetProductDTO();
            item0.SetQuantity(quantityToBuy0);

            double p0Sale = testProduct0.Sale; // init parameter
            double p0Price = testProduct0.Price; // init parameter

            double priceAfterSale0 = (p0Price - (p0Price / 100 * p0Sale)) * quantityToBuy0 / 2; // due to purchase policy in init

            // Act
            double out0WithSale = this.testProduct0.CalculatePrice(item0);

            // Assert
            Assert.AreEqual(priceAfterSale0, out0WithSale);
        }

        [TestMethod]
        public void CalculatePriceWithoutSaleProductTestSuccess()
        {
            // Arrange
            int quantityToBuy0 = 10;
            ItemDTO item0 = testProduct0.GetProductDTO();
            item0.SetQuantity(quantityToBuy0);

            double p0Price = testProduct0.Price; // init parameter
            this.testProduct0.SetSale(0);

            double priceBeforeSale0 = p0Price * quantityToBuy0 / 2;  // Due to purchase policy in init

            // Act
            double out0WithoutSale = this.testProduct0.CalculatePrice(item0);

            // Assert
            Assert.AreEqual(priceBeforeSale0, out0WithoutSale);
        }

        [TestMethod]
        public void CalculatePriceBadQuantityProductTestFail()
        {
            // Arrange
            ItemDTO item0 = testProduct0.GetProductDTO();
            item0.SetQuantity(0);
            bool error0 = false;

            // Act
            try
            {
                this.testProduct0.CalculatePrice(item0);
            }
            catch (Exception ex) { error0 = true; }

            // Assert
            Assert.IsTrue(error0);
        }

        [TestMethod]
        public void prePurchaseProductTestSuccess()
        {
            // Arrange
            int quantityToBuy0 = this.testProduct0.Quantity / 2;
            ItemDTO item0 = this.testProduct0.GetProductDTO();
            item0.SetQuantity(quantityToBuy0);

            // Act
            bool out0 = this.testProduct0.prePurchase("", item0);

            // Assert
            Assert.IsTrue(out0);
        }

        [TestMethod]
        public void prePurchaseBadQuantityProductTestFail()
        {
            // Arrange
            int quantityToBuy0 = 0;
            ItemDTO item0 = this.testProduct0.GetProductDTO();
            item0.SetQuantity(quantityToBuy0);
            bool error0 = false;

            // Act
            try
            {
                bool out0 = this.testProduct0.prePurchase("", item0);
            }
            catch (Exception ex) { error0 = true;}

            // Assert
            Assert.IsTrue(error0);
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
            int quantityToReserve1 = initQuantity1 / 2;
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
                this.testProduct1.Reserve(quantityToReserve1);
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
                this.testProduct1.Reserve(quantityToReserve1);
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
            testProduct0.SetTimesBought(1);

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
            string newComment1 = userID1 + ": " + comment1 + ".\n Rating: _ .";

            // Act
            this.testProduct1.AddComment(userID1, comment1, rating1);

            // Assert
            Assert.IsTrue(this.testProduct1.Comments.ToList().Contains(newComment1));
            Assert.AreEqual(testProduct1.Rating, initRating1);
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
            Assert.AreEqual(testProduct0.ProductCategory.CategoryName, noCategory);
            Assert.AreEqual(testProduct1.ProductCategory.CategoryName, noCategory);
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

        private Store GetStore()
        {
            string founderID = "testStoreFounderID326";
            StoreDTO newStore = StoreFacade.GetInstance().AddNewStore(founderID, new List<string>() { "policyTestStoreName" });
            return StoreRepo.GetInstance().getStore(newStore.StoreID);
        }

        private Product GetNewProduct()
        {
            // productProperties = {Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale ,Weight, Dimenssions, PurchaseAttributes, ProductCategory}
            // ProductAttributes = atr1Name:atr1opt1_atr1opt2...atr1opti;atr2name:atr2opt1...
            List<String> productProperties = new List<String>() { "testProduct0Name", "testProduct0Desription", "123.5", "45", "0" , "0", "0", "67", "9.1_8.2_7.3",
                                                                   "testProduct0Atr1:testProduct0Atr1Opt1_testProduct0Atr1Opt2;testProduct0Atr2:testProduct0Atr2Opt1_testProduct0Atr2Opt2_testProduct0Atr2Opt3;",
                                                                   "testProduct0SomeCategory"};
            string storeID = this.testStore.Store_ID;
            ConcurrentDictionary<string, Purchase_Policy> defaultStorePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            ConcurrentDictionary<string, Purchase_Strategy> defaultStoreStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            
            Product newP0 = new Product(productProperties, storeID, defaultStorePolicies, defaultStoreStrategies);

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

        private Product GetExistingProduct()
        {
            String product_ID = this.testStore.Store_ID + "_tesProduct1ID";
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
                                dimenssions, comments, defaultStorePolicies, defaultStoreStrategies, product_Attributes, boughtTimes, category, 11, new KeyValuePair<string, List<string>>(product_ID, new List<string> { "-1.0", "" }), null);

            Statement storeIDStatement = new EqualRelation("Name", newp1.Name, false, false);
            Statement statement = new AtLeastStatement(1, new Statement[] { storeIDStatement });
            string samestatement = "[AtLeast[[1][Equal[[Name][" + newp1.Name + "]]]]]";

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