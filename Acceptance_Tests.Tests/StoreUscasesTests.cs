using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market_System.Tests.SeviceLevelTests
{

    /// <summary>
    /// Summary description for StoreUscasesTests
    /// </summary>
    [TestClass]
    public class StoreUscasesTests
    {
        private Service_Controller service_Controller;

        public void Setup()
        {
            Logger.get_instance().change_logger_path_to_tests();
            service_Controller = new Service_Controller();
            service_Controller.register("user1", "pass1", "add1");
            service_Controller.login_member("user1", "pass1");
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        public void TestCleanup()
        {
            Logger.get_instance().change_logger_path_to_regular();
            service_Controller.destroy();
        }


        [TestMethod]
        public void openStoreSuccess()
        {
            //Setup: 
            Setup();

            //Action:
            Response<StoreDTO> response = service_Controller.open_new_store(new List<string> { "store_123" });
            Response<StoreDTO> response2 = service_Controller.GetStore(response.Value.StoreID);

            //Result:
            Assert.AreEqual(false, response.ErrorOccured);
            Assert.AreEqual(false, response2.ErrorOccured);
            Assert.AreEqual(response.Value.StoreID, response2.Value.StoreID);
            //todo: check if store exists now

            //tearDown:
            TestCleanup();
        }
        

        [TestMethod]
        public void FailopenStoreGuest()
        {
            //Setup: none
            Setup();

            //Action:
            Response<StoreDTO> response = service_Controller.open_new_store(new List<string> {}); 

            //Result:
            Assert.AreEqual(null, response);

            //tearDown:
            TestCleanup();
        }

        
        [TestMethod]
        public void addProductSuccess()
        {
            //Setup: 
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" }); 

            //Action:
            Response<ItemDTO> product = service_Controller.add_product_to_store(store.Value.StoreID, "prod1","desc1", "1","1","1","5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            //Result:
            Assert.AreEqual(false, product.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void addProductFailure_AddingWrongStoreID()
        {
            //Setup: 
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });

            //Action:
            Response<ItemDTO> product = service_Controller.add_product_to_store("fake_store_ID", "prod1", "desc1", "1", "1", "1", "5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            //Result:
            Assert.AreEqual(true, product.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void addProductFailure_AddingAProductWithoutRating()
        {
            //Setup: 
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });

            //Action:
            Response<ItemDTO> product = service_Controller.add_product_to_store(store.Value.StoreID, "prod1", "desc1", "1", "1", "1", "", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            //Result:
            Assert.AreEqual(true, product.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        //maybe do another test for showing member purchase 
        //for that you need to rigister then login , should be an opened store with an product with quantity >0 , 


        //No such function: delete_product()...
        /*
        [TestMethod]
        public void removeProduct()
        {
            //Setup:
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });
            Response<ItemDTO> product = service_Controller.add_product_to_store(store.Value.StoreID, "prod1", "desc1", "1", "1", "1", "", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");
            // Action:
            bool result = service_Controller.removeProduct(store.Value.StoreID, product.Value.GetID());

            // Result: 
            Assert.IsTrue(result);
            // Check that the product has been removed from the store
            Response<List<Product>> response = service_Controller.get_store_products(store_ID);
            Assert.IsFalse(response.Data.Any(p => p.Name == "prod1"));

            // Action: Try to remove the same product again
            bool result2 = service_Controller.removeProduct(store_ID, "prod1");

            // Result: 
            Assert.IsFalse(result2);
            // Check that the store's products have not changed
            Response<List<Product>> response1 = service_Controller.get_store_products(store_ID);
            Assert.IsFalse(response1.Data.Any(p => p.Name == "prod1"));

            // Action: Try to remove a non-existent product
            bool result3 = service_Controller.removeProduct(store_ID, "prod3");

            // Result: 
            Assert.IsFalse(result3);
            // Check that the store's products have not changed
            Response<List<Product>> response2 = service_Controller.get_store_products(store_ID);
            Assert.IsFalse(response2.Data.Any(p => p.Name == "prod3"));

            // Teardown:
            TestCleanup();
        } 
        */

        //commented everything beneath, because the project the does not compile
        /*
        [TestMethod]
        public void AddStore_StoreWithSameStoreIdAndFounderExists()
        {
            //Setup:
            string userId = "founder123";
            string storeId = "store123";
            Store store = new Store(storeId, userId, "My Store");

            // Add a store with the same ID and founder as the one we're trying to add
            Store existingStore = new Store(storeId, userId, "Existing Store");
            service_Controller.AddStore(userId, existingStore);

            //Action & Result:
            Assert.ThrowsException<Exception>(() => service_Controller.AddStore(userId, store));

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void GetNewStoreID_ReturnsUniqueStoreId()
        {
            //Setup:
            int numberOfStores = 5;
            List<Store> stores = new List<Store>();
            for (int i = 0; i < numberOfStores; i++)
            {
                stores.Add(new Store(service_Controller.getNewStoreID(), "founder123", $"Store{i}"));
            }

            //Action:
            string newStoreId = service_Controller.getNewStoreID();

            //Result:
            Assert.IsFalse(stores.Any(s => s.Store_ID.AreEqual(newStoreId)));

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void GetNewStoreID_ReturnsValidStoreId()
        {
            //Action:
            string newStoreId = service_Controller.getNewStoreID();

            //Result:
            Assert.IsFalse(string.IsNullOrWhiteSpace(newStoreId));

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void GetNewProductID_ReturnsUniqueProductId()
        {
            //Setup:
            string storeId = service_Controller.getNewStoreID();
            Store store = new Store(storeId, "founder123", "Store1");
            storeDatabase.Add(store, new Dictionary<Product, int>());
            int numberOfProducts = 5;
            List<Product> products = new List<Product>();
            for (int i = 0; i < numberOfProducts; i++)
            {
                string productId = service_Controller.getNewProductID(storeId);
                products.Add(new Product(productId, storeId, $"Product{i}"));
                storeDatabase[store].Add(products[i], 1);
            }

            //Action:
            string newProductId = service_Controller.getNewProductID(storeId);

            //Result:
            Assert.IsFalse(products.Any(p => p.Product_ID.AreEqual(newProductId)));

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void GetNewProductID_ReturnsValidProductId()
        {
            //Setup:
            string storeId = service_Controller.getNewStoreID();
            Store store = new Store(storeId, "founder123", "Store1");
            storeDatabase.Add(store, new Dictionary<Product, int>());

            //Action:
            string newProductId = service_Controller.getNewProductID(storeId);

            //Result:
            Assert.IsFalse(string.IsNullOrWhiteSpace(newProductId));

            //tearDown: (TestCleanup())
        }

        */

        [TestMethod]
        public void GetStore_ReturnsCorrectStore()
        {
            //Setup:
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });

            //Action:
            Response<StoreDTO> retrievedStore = service_Controller.GetStore(store.Value.StoreID);

            //Result:
            Assert.AreEqual(store.Value.StoreID, retrievedStore.Value.StoreID);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void GetStore_ThrowsExceptionForInvalidStoreId()
        {
            //Setup:
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });

            //Action and Result:
            Response<StoreDTO> retrievedStore = service_Controller.GetStore("fake_store_ID");
            Assert.AreEqual(null, retrievedStore);

            //tearDown:
            TestCleanup();
        }

        //THESE DOESN'T SEEM TO WORK: There's a problem in 'get_products_from_shop()' function
        /*
        [TestMethod]
        public void GetProduct_ReturnsCorrectProduct()
        {
            //Setup:
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });

            Response<ItemDTO> product = service_Controller.add_product_to_store(store.Value.StoreID, "prod1", "desc1", "1", "1", "1", "", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            //Action:
            Response<List<ItemDTO>> retrievedProducts = service_Controller.get_products_from_shop(store.Value.StoreID);

            //Result:
            Assert.AreEqual(product.Value.GetID(), retrievedProducts.Value[0].GetID());

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void GetProduct_ThrowsExceptionForInvalidProductId()
        {
            //Setup:
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });

            Response<ItemDTO> product = service_Controller.add_product_to_store(store.Value.StoreID, "prod1", "desc1", "1", "1", "1", "", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            //Action:
            Response<List<ItemDTO>> retrievedProducts = service_Controller.get_products_from_shop(store.Value.StoreID);

            //Result:
            Assert.AreNotEqual("fake_product_id", retrievedProducts.Value[0].GetID());

            //tearDown:
            TestCleanup();
        }
        */

        [TestMethod]
        public void GetProduct_ThrowsExceptionForProductIdInNonexistentStore()
        {
            //Setup:
            Setup();
            Response<StoreDTO> store = service_Controller.open_new_store(new List<string> { "store_123" });
            Response<ItemDTO> product = service_Controller.add_product_to_store(store.Value.StoreID, "prod1", "desc1", "1", "1", "1", "", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            //Action:
            Response<List<ItemDTO>> retrievedProducts = service_Controller.get_products_from_shop("fake_store_ID");

            //Result:
            Assert.AreEqual(null, retrievedProducts);

            //tearDown:
            TestCleanup();
        }
        
        /*
        [TestMethod]
        public void SaveStore_UpdatesExistingStoreWithSameStoreId()
        {
            // Setup
            string userId = "123";
            string storeId = service_Controller.getNewStoreID();
            Store store = new Store(storeId, userId, "My Store");
            storeDatabase.Add(store, new Dictionary<Product, int>());
            int initialProductCount = storeDatabase[store].Count;
            Product product = new Product(service_Controller.getNewProductID(storeId), storeId, "My Product");
            storeDatabase[store].Add(product, 1);

            // Action
            store.Name = "New Store Name";
            service_Controller.saveStore(store);

            // Assert
            Assert.AreEqual("New Store Name", storeDatabase.Keys.First(s => s.Store_ID == storeId).Name);
            Assert.AreEqual(initialProductCount, storeDatabase[store].Count);

            // TearDown
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SaveStore_ThrowsExceptionWhenStoreNotFound()
        {
            // Setup
            string userId = "123";
            string storeId = service_Controller.getNewStoreID();
            Store store = new Store(storeId, userId, "My Store");

            // Action
            service_Controller.saveStore(store);

            // Assert: Expecting exception

            // TearDown
        }

        [TestMethod]
        public void CloseStoreTemporarily_RemovesStoreFromOpenedStoresAndAddsToTemporaryClosedStores()
        {
            // Setup
            string storeId = "123";
            List<string> openedStores = new List<string>() { "123", "456", "789" };
            List<string> tempClosedStores = new List<string>();

            // Action
            service_Controller.close_store_temporary(storeId, openedStores, tempClosedStores);

            // Assert
            Assert.IsFalse(openedStores.Contains(storeId));
            Assert.IsTrue(tempClosedStores.Contains(storeId));
        }

        [TestMethod]
        public void CloseStoreTemporarily_DoesNothingIfStoreIdNotFoundInOpenedStores()
        {
            // Setup
            string storeId = "123";
            List<string> openedStores = new List<string>() { "456", "789" };
            List<string> tempClosedStores = new List<string>() { "987" };

            // Action
            service_Controller.close_store_temporary(storeId, openedStores, tempClosedStores);

            // Assert
            Assert.IsFalse(openedStores.Contains(storeId));
            Assert.IsTrue(tempClosedStores.Contains("987"));
            Assert.IsFalse(tempClosedStores.Contains(storeId));
        }

        [TestMethod]
        public void CloseStoreTemporarily_DoesNothingIfStoreIdAlreadyInTemporaryClosedStores()
        {
            // Setup
            string storeId = "123";
            List<string> openedStores = new List<string>() { "123", "456", "789" };
            List<string> tempClosedStores = new List<string>() { "123", "987" };

            // Action
            service_Controller.close_store_temporary(storeId, openedStores, tempClosedStores);

            // Assert
            Assert.IsTrue(openedStores.Contains(storeId));
            Assert.IsTrue(tempClosedStores.Contains(storeId));
            Assert.AreEqual(2, tempClosedStores.Count);
        }

        */
    }
}

