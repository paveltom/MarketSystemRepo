﻿
using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;



namespace Market_System.Tests.ServiceLevelTests
{
    /// <summary>
    /// tests in this class:
    /// 1.regetretion.
    ///     1.1 Successful registration
    ///     1.2 Failed registration - used username
    ///     1.4 
    ///     1.5 
    /// 2.login
    ///     2.1 fail login
    /// 3.logout
    ///     3.1 success Logout
    ///     

    /// </summary>

    [TestClass]
    public class UserUscasesTests
    {
        private Service_Controller service_Controller;

        public void Setup()
        {
            Logger.get_instance().change_logger_path_to_tests();
            service_Controller = new Service_Controller();
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

        #region

        [TestMethod]
        public void UserRegistersAsMemberAndLogin()
        {
            //Setup: 
            Setup();

            //Action:
            Response<string> response = service_Controller.register("user1", "pass1", "add1");

            //Result:
            Assert.IsNotNull(response.Value);
            Assert.AreEqual(false, response.ErrorOccured);

            Response<string> responseLogin = service_Controller.login_member("user1", "pass1");
            Assert.AreEqual(false, responseLogin.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void FailUserRegistersUsedUserame()
        {
            //Setup: 
            Setup();

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.register("user1", "pass2", "add2");

            //Result:
            Assert.AreEqual(true, response2.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void failLoginBadUsername()
        {
            //Setup: 
            Setup();

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user@#$", "pass1");

            //Result:
            Assert.AreEqual(true, response2.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void failLoginBadPass()
        {
            //Setup: 
            Setup();

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user1", "pass11111111111111");

            //Result:
            Assert.AreEqual(true, response2.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void SuccessLogout()
        {
            //Setup: 
            Setup();

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user1", "pass1");
            Response<string> responseLogout =service_Controller.log_out(); 
            Response<string> secondLogin = service_Controller.login_member("user1", "pass1");


            //Result:
            Assert.AreEqual(false, responseLogout.ErrorOccured);
            Assert.AreEqual(false, secondLogin.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        [TestMethod]
        public void FailureLogout()
        {
            //Setup: 
            Setup();

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.login_member("user1", "pass1");
            Response<string> responseLogout1 = service_Controller.log_out();
            Response<string> responseLogout2 = service_Controller.log_out();


            //Result:
            Assert.AreEqual(false, responseLogout1.ErrorOccured);
            Assert.AreEqual(true, responseLogout2.ErrorOccured);

            //tearDown:
            TestCleanup();
        }

        //maybe do another test for showing member purchase 
        //for that you need to rigister then login , should be an opened store with an product with quantity >0 , 

        #endregion

    }
}

/*
 * 1.register with mail to get notifications.
 * 2.add several threads tests about register, login, logout.
 * 3.add test of login the same user while he is already connected
 * 
 */