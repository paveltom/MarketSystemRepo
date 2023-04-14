using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market_System.Tests.SeviceLevelTests
{
    /// <summary>
    /// Summary description for ACCEPTANCE
    /// </summary>
    [TestClass]
    public class ACCEPTANCE
    {

        #region 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        ///
        #endregion


        private Service_Controller service;

        public void oneThreadSetUp()
        {
            service = new Service_Controller();
        }

        public void twoThreadSetUp()
        {

        }

        //(one thread)
        public void registeredLoggedInMemberSetUp(string username, string pass, string add)
        {
            oneThreadSetUp();
            service.register(username, pass, add);
            service.login_member(username, pass);
        }

        //(one thread)
        public void LoggedInOwnerWithOpenedOneProdStore(string username, string pass, string add)
        {
            registeredLoggedInMemberSetUp(username, pass, add);
            //todo:
            Response<string> response = service.open_new_store(); ////todo store id? Store1
            Response<string> resProdAdd = service.add_product_to_store("Store1", "prod1", "desc1", "1", "1", "1", "", "", "", "", "", ""); ////todo store id? Store1
        }

        public void oneThreadCleanup()
        {
            service.destroy();
        }






        /************TESTS***********/


        [TestMethod]
        public void UserRegistersAsMember()
        {
            //Setup: 
            oneThreadSetUp();

            //Action:
            Response<string> response = service.register("user1", "pass1", "add1");

            //Result:
            Assert.Equals(false, response.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }


        [TestMethod]
        public void LoginMember()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> response = service.register("user1", "pass1", "add1");

            //Action:
            Response<string> responseLogin = service.login_member("user1", "pass1");

            //Result:
            Assert.Equals(false, responseLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void FailUserRegistersUsedUserame()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> response = service.register("user1", "pass1", "add1");

            //Action:
            Response<string> responseReg2 = service.register("user1", "pass2", "add2");

            //Result:
            Assert.Equals(true, responseReg2.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void failLoginBadUsername()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> response = service.register("user1", "pass1", "add1");

            //Action:
            Response<string> responseLogin = service.login_member("user2142415", "pass1");

            //Result:
            Assert.Equals(true, responseLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void failLoginBadPass()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> response = service.register("user1", "pass1", "add1");

            //Action:
            Response<string> responseLogin = service.login_member("user1", "pass125256622222222");

            //Result:
            Assert.Equals(true, responseLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void successLogout()
        {
            //Setup: 
            registeredLoggedInMemberSetUp("user1", "pass1", "add1");

            //Action:
            Response<string> responseLogout = service.log_out();

            //Result:
            Assert.Equals(false, responseLogout.ErrorOccured);
            Response<string> responseLogoutAgain = service.log_out();
            Assert.Equals(true, responseLogoutAgain.ErrorOccured); //already logged out

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void openStoreSuccess()
        {
            //Setup: 
            registeredLoggedInMemberSetUp("user1", "pass1", "add1");

            //Action:
            Response<string> response = service.open_new_store(); ////todo store id? Store1

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if store was added:
            Response<ItemDTO> resGetStore = service.GetStore("Store1"); ////todo store id? Store1
            Assert.Equals(false, resGetStore.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void addproduct()
        {
            //Setup: 
            registeredLoggedInMemberSetUp("user1", "pass1", "add1");
            Response<string> response = service.open_new_store(); ////todo store id? Store1

            //Action:
            Response<string> resProdAdd = service.add_product_to_store("Store1", "prod1", "desc1", "1", "1", "1", "", "", "", "", "", ""); ////todo store id? Store1

            //Result:
            Assert.Equals(false, resProdAdd.ErrorOccured);
            //todo: check if prod was added
            // Response < List < ItemDTO > resProdAdded = service.get_products_from_shop("Store1");
            //Assert.Equals(false, resProdAdded.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }


        [TestMethod]
        public void changeProdName()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.ChangeProductName("Store1_Prod1", "newName");

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if prod name changed
            // Response < List < ItemDTO > resProdAdded = service.get_products_from_shop("Store1");
            //check if new name there

            //tearDown:
            oneThreadCleanup();
        }

        #region
        /*TODO: duplicate  changeProdName() test to all product atrributes that can be eddited by store owner.
        //change price,..........................
                [TestMethod]
        public void changeProdName()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.ChangeProductName("Store1_Prod1", "newName");

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if prod name changed
            // Response < List < ItemDTO > resProdAdded = service.get_products_from_shop("Store1");
            //check if new name there

            //tearDown:
            oneThreadCleanup();
        }
                [TestMethod]
        public void changeProdName()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.ChangeProductName("Store1_Prod1", "newName");

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if prod name changed
            // Response < List < ItemDTO > resProdAdded = service.get_products_from_shop("Store1");
            //check if new name there

            //tearDown:
            oneThreadCleanup();
        }
                [TestMethod]
        public void changeProdName()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.ChangeProductName("Store1_Prod1", "newName");

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if prod name changed
            // Response < List < ItemDTO > resProdAdded = service.get_products_from_shop("Store1");
            //check if new name there

            //tearDown:
            oneThreadCleanup();
        }
        */
        #endregion







    }

    /*
     * ideas for tests:
     * 1.fail open store user is guest please login as member first.
     * 2.edit(change) store name 
     * 3.manager actions
     */




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
}
