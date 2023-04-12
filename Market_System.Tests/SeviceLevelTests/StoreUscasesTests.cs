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
        public void UserRegistersAsMemberAndLogin()
        {
            //Setup: none

            //Action:
            //Response<string> response = service_Controller.open_new_store();
            service_Controller.open_new_store();
            service_Controller.GetStore();

            //Result:
            //Assert.Equals(false, response.ErrorOccured);


            Response<string> responseLogin = service_Controller.login_member("user1", "pass1");
            Assert.Equals(false, responseLogin.ErrorOccured);

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void FailUserRegistersUsedUserame()
        {
            //Setup: none

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.register("user1", "pass2", "add2");

            //Result:
            Assert.Equals(true, response2.ErrorOccured);

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void failLoginBadUsername()
        {
            //Setup: none

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user@#$", "pass1");

            //Result:
            Assert.Equals(true, response2.ErrorOccured);

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void failLoginBadPass()
        {
            //Setup: none

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user1", "pass11111111111111");

            //Result:
            Assert.Equals(true, response2.ErrorOccured);

            //tearDown: (TestCleanup())
        }

        [TestMethod]
        public void successLogout()
        {
            //Setup: none

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user1", "pass1");
            Response<string> responseLogout = service_Controller.log_out();
            Response<string> secondLogin = service_Controller.login_member("user1", "pass1");


            //Result:
            Assert.Equals(false, responseLogout.ErrorOccured);
            Assert.Equals(false, secondLogin.ErrorOccured);

            //tearDown: (TestCleanup())
        }

        //maybe do another test for showing member purchase 
        //for that you need to rigister then login , should be an opened store with an product with quantity >0 , 

    }
}

/*
 * 1.register with mail to get notifications.
 * 2.add several threads tests about register, login, logout.
 * 3.add test of login the same user while he is already connected
 * 
 */