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

        public void setup()
        {
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

        //ClassInitialize runs before running the first test in the class
        [ClassInitialize()]
        public void ClassInitialize()
        {
            setup();
        }

        [TestInitialize()]
        public void TestInitialize() { setup(); }

        [TestCleanup()]
        public void TestCleanup()
        {
            service_Controller.destroy();
        }

        [TestMethod]
        public void openStoreSuccess()
        {
            //Setup: none

            //Action:
            Response<string> response = service_Controller.open_new_store(); ////todo store id? Store1
            service_Controller.open_new_store();

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if store exists now

            //tearDown: (TestCleanup())
        }

        /*[TestMethod]
        public void FailopenStoreGuest()
        {
            //Setup: none

            //Action:
            Response<string> response = service_Controller.open_new_store(); ////todo store id? Store1
            service_Controller.open_new_store();

            //Result:
            Assert.Equals(true, response2.ErrorOccured);

            //tearDown: (TestCleanup())
        }*/

        [TestMethod]
        public void addProduct()
        {
            //Setup: none
            Response<string> response = service_Controller.open_new_store(); ////todo store id? Store1

            //Action:
            Response<string> response2 = service_Controller.add_product_to_store("Stor1", "prod1","desc1", "1","1","1","","","","","",""); ////todo store id? Store1

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if store exists now

            //tearDown: (TestCleanup())
        }

        //maybe do another test for showing member purchase 
        //for that you need to rigister then login , should be an opened store with an product with quantity >0 , 

    }
}

