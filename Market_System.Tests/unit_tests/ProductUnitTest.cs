using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class ProductUnitTest
    {
        // fields          
        private Product testProduct0; // uses Builder of a new Product 
        private Product testProduct1; // uses Builder oa an existing Product        
        TestRepo repo;
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
        }

        // before each test 
        [TestInitialize()]
        public void Setup()
        {
            testProduct0 = GetNewProduct();
            testProduct1 = GetExistingProduct();
            this.repo = new TestRepo();
            testProduct0.testRepo = repo;
            testProduct1.testRepo = repo;
        }

        // after each test
        [TestCleanup()]
        public void TearDown()
        {
            // nothing to cleanup
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
            List<string> attr2 = new List<string>() { "testProduct1Atr2Opt1", "testProduct1Atr2Opt2", "testProduct1Atr2Opt3"};
            product_Attributes.Add("testProduct1Atr1", attr1);
            product_Attributes.Add("testProduct1Atr2", attr2);

            int boughtTimes = 11;
            Category category = new Category("testProduct1SomeCategory");
            return new Product(product_ID, name, description, price, initQuantity, reservedQuantity, rating, sale, weight, 
                                dimenssions, comments, defaultStorePolicies, defaultStoreStrategies, product_Attributes, boughtTimes, category);
        }

        [TestMethod]
        public void AddPurchasePolicyProductTest()
        {
            // Arrange
            Purchase_Policy purchase_Policy0 = new Purchase_Policy("AddPurchasePolicyProductTest_Policy0ID", "AddPurchasePolicyProductTest_Policy0Name");
            Purchase_Policy purchase_Policy1 = new Purchase_Policy("AddPurchasePolicyProductTest_Policy1ID", "AddPurchasePolicyProductTest_Policy1Name");
            Purchase_Policy purchase_PolicyBoth = new Purchase_Policy("AddPurchasePolicyProductTest_Policy_BOTH_ID", "AddPurchasePolicyProductTest_Policy_BOTH_Name");

            // Act
            this.testProduct0.AddPurchasePolicy(purchase_Policy0);
            this.testProduct0.AddPurchasePolicy(purchase_PolicyBoth);
            this.testProduct1.AddPurchasePolicy(purchase_Policy1);
            this.testProduct1.AddPurchasePolicy(purchase_PolicyBoth);

            // Assert
            Assert.IsTrue(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy0.GetID()));
            Assert.IsTrue(testProduct0.PurchasePolicies.ContainsKey(purchase_PolicyBoth.GetID()));
            Assert.IsTrue(testProduct1.PurchasePolicies.ContainsKey(purchase_Policy1.GetID()));
            Assert.IsTrue(testProduct1.PurchasePolicies.ContainsKey(purchase_PolicyBoth.GetID()));

            Assert.IsFalse(testProduct0.PurchasePolicies.ContainsKey(purchase_Policy1.GetID()));
            Assert.IsFalse(testProduct1.PurchasePolicies.ContainsKey(purchase_Policy0.GetID()));
        }
    
    }

    /*
     * 
     * 

    public void RemovePurchasePolicy(String policyID)

    public void AddPurchaseStrategy(Purchase_Strategy newStrategy)

    public void RemovePurchaseStrategy(String strategyID)

    public void AddAtribute(string attribute, List<string> options)

    public void RemoveAttribute(string attribute, List<string> options)

    public double ImplementSale(List<String> chosenAttributes)

    public string GetStoreID()

    public double CalculatePrice(int quantity, Boolean implementSale) // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)

    public void Purchase(int quantity) // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)

    public void Reserve(int quantity)

    public void LetGoProduct(int quantity)

    private void UpdateRating(double rating)

    public void AddComment(string userID, string comment, double rating)

    public Boolean prePurchase(int quantity)

    public ItemDTO GetProductDTO()

    private void Save()

    // =============================== set functions ======================================
    public void SetName(string name)

    public void SetDescription(string description)

    public void SetPrice(double price)

    public void SetRating(double raring)

    public void SetQuantity(int quantity)

    public void SetWeight(double weight)

    public void SetSale(double sale)

    public void SetTimesBought(int times)

    public void SetProductCategory(Category category)

    public void SetDimenssions(double[] dims)
        */

}


// Mock example:
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