using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NUnit.Framework;
using Market_System.Domain_Layer.Store_Component;
using Market_System.Domain_Layer.User_Component;
using Market_System.Domain_Layer;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class store_user_tests
    {
        private MarketSystem ms;
        private UserFacade user_facade;
        private StoreFacade store_facade;


        //i dont know how to do after each and not sure if this is before each so do setup and detroy after each

        //[TestInitialize] // this means before each

        [SetUp]
        public void Setup() 
        {
            user_facade= UserFacade.GetInstance();
            store_facade = StoreFacade.GetInstance();
        }

        [TearDown]
        public void TearDown()
        {
            store_facade.Destroy_me();
            user_facade.Destroy_me();
        }

        [Test]
        public void register_user_test_success()
        {
            Setup();
            user_facade.register("test1", "pass");
            Assert.AreEqual(true, user_facade.check_if_user_exists("test1", "pass"));
            TearDown();
        }

        [Test]
        public void register_user_test_failure()
        {
            Setup();
            user_facade.register("test1", "pass");
            try
            {
                user_facade.register("test1", "pass");
                Assert.Fail("Expected this test to fail, but was a success - the same user was registered twice");
            }

            catch (Exception e)
            {
                Assert.AreEqual("a user with same name exists, please change name!", e.Message);
            }

            TearDown();
        }

        [Test]
        public void Login_user_test_success()
        {
            Setup();
            user_facade.register("test1", "pass");
            try
            {
                user_facade.Login("test1", "pass");
                Assert.AreEqual("Member", user_facade.Get_User_State("test1"));
            }

            catch(Exception e)
            {
                Assert.Fail("This test shouldn't have failed, but received this error: " + e.Message);
            }

            TearDown();
        }

        [Test]
        public void Login_user_test_failure()
        {
            Setup();
            user_facade.register("test1", "pass");
            try
            {
                user_facade.Login("test2", "pass");
                Assert.Fail("This test should've failed - no such user as 'test2'");
            }

            catch (Exception e)
            {
                Assert.AreEqual("Incorrect login information has been provided", e.Message);
            }

            TearDown();
        }

        [Test]
        public void Logout_user_test_success()
        {
            Setup();
            try
            {
                user_facade.register("test1", "pass");
                user_facade.Login("test1", "pass");
                user_facade.Logout("test1");
                Assert.AreEqual("Guest", user_facade.Get_User_State("test1"));
            }

            catch (Exception e)
            {
                Assert.Fail("This test shouldn't have failed, but received this error: " + e.Message);
            }

            TearDown();
        }

        [Test]
        public void Logout_user_test_failure_1()
        {
            Setup();
            try
            {
                user_facade.register("test1", "pass");
                user_facade.Login("test1", "pass");
                user_facade.Logout("test1");
                Assert.Fail("This test should've failed - Already logged-out");
            }

            catch (Exception e)
            {
                Assert.AreEqual("You're already logged-out", e.Message);
            }

            TearDown();
        }

        [Test]
        public void Logout_user_test_failure_2()
        {
            Setup();
            try
            {
                user_facade.register("test1", "pass");
                user_facade.Logout("test1");
                Assert.Fail("This test should've failed - Already logged-out");
            }

            catch (Exception e)
            {
                Assert.AreEqual("You're already logged-out", e.Message);
            }

            TearDown();
        }

        [Test]
        public void Logout_user_test_failure_3()
        {
            Setup();
            try
            {
                user_facade.Logout("test1");
                Assert.Fail("This test should've failed - Already logged-out, no such user exists either.");
            }

            catch (Exception e)
            {
                Assert.AreEqual("You're already logged-out", e.Message);
            }

            TearDown();
        }

        [Test]
        public void Login_guest_test_success()
        {
            Setup();
            try
            {
                user_facade.Login_guset("test1");
                Assert.AreEqual("Guest", user_facade.Get_User_State("test1"));
            }

            catch (Exception e)
            {
                Assert.AreEqual("??????????", e.Message);
            }

            TearDown();
        }
    }
}
