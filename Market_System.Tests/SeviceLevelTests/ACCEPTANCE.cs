﻿using Market_System.DomainLayer;
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
            Response<string> response = service.open_new_store(new List<string>{ "store_name"}); ////todo store id? Store1
            Response<string> resProdAdd = service.add_product_to_store("Store1", "Prod1", "desc1", "1", "1", "1", "", "", "", "", "", ""); ////todo store id? Store1
        }

        //(one thread)
        public void LoggedMemberOpenedOneProdStore(string username, string pass, string add)
        {
            registeredLoggedInMemberSetUp(username + "Owner", pass + "Owner", add);
            service.open_new_store(new List<string> { "store_name" }); ////todo store id? Store1 
            //todo:
            service.add_product_to_store("Store1", "prod1", "desc1", "1", "1", "1", "", "", "", "", "", ""); ////todo store id? Store1
            service.log_out(); //owner logs out
            registeredLoggedInMemberSetUp(username, pass, add); //other member logs in
        }

        public void oneThreadCleanup()
        {
            service.destroy();
        }






        /************TESTS***********/

        #region//Guest actions Tests 1.3, 1.4
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
        #endregion

        #region//Member purchase actions Tests 3.1, 3,2, 3.3
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
            Response<string> response = service.open_new_store(new List<string> { "store_name" }); ////todo store id? Store1

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if store was added:
            Response<ItemDTO> resGetStore = service.GetStore("Store1"); ////todo store id? Store1
            Assert.Equals(false, resGetStore.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void comment_on_product()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.comment_on_product("Store1_Prod1", "newName is very bad product", 0.5);

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            //todo: check if prod comment added
            //check if new name there

            //tearDown:
            oneThreadCleanup();
        }
        #endregion

        #region//Owner  actions Tests 4.1
        [TestMethod]
        public void addproduct()
        {
            //Setup: 
            registeredLoggedInMemberSetUp("user1", "pass1", "add1");

            //Action:
            Response<string> resProdAdd = service.add_product_to_store("Store1", "prod1", "desc1", "1", "1", "1", "", "", "", "", "", ""); ////todo store id? Store1

            //Result:
            Assert.Equals(false, resProdAdd.ErrorOccured);
            //todo: check if prod was added
            Response < List < ItemDTO >> resProdAdded = service.get_products_from_shop("Store1");

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
            //Response < List < ItemDTO >> resProdAdded = service.get_products_from_shop("Store1");

            //tearDown:
            oneThreadCleanup();
        }

        #region /*TODO: add more tests like  changeProdName() test for all product atrributes that can be eddited by store owner.
        /*
        [TestMethod]
        public void changePrice()
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
        .
        .
        .
        .
        .
        .
        .
        }

        */
        #endregion

        #endregion

        #region//Guest purchase Tests 2.1*, 2.2, 2.3, 2.4* 2.5*
        [TestMethod]
        public void searchProducts()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo:
            Response<List<ItemDTO>> response = service.search_product_by_name("Prod1");

            //Result:
            Assert.Equals(false, response.ErrorOccured);
            Assert.Equals(1, response.Value.Capacity);
            //tearDown:
            oneThreadCleanup();
        }

        //add more search options tests here:
        /*
         * 
         * 
         */

        [TestMethod]
        public void addProdToCartGuest()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo: fix Prod1-->prod_id
            Response<string> response = service.add_product_to_basket("Prod1", "1");

            //Result:
            Assert.Equals(false, response.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void viewBasketGuest()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");
            Response<string> response = service.add_product_to_basket("Prod1", "1");

            //Action:
            //todo: fix Prod1-->prod_id
            throw new NotImplementedException();

            //Result:
            //Assert.Equals(false, response2.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void checkoutCartGuest()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");
            //todo: fix Prod1-->prod_id
            Response<string> response = service.add_product_to_basket("Prod1", "1");

            //Action:
            //todo:
            //service.check_out("user1", "1234567812345678", DomainLayer.UserComponent.)
            //Response<string> response2 = service.c

            //Result:
            //Assert.Equals(false, response.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        #endregion












    }

    /*
     * ideas for tests:
     * 1.fail open store user is guest please login as member first.
     * 2.edit(change) store name 
     * 3.manager actions
     * דרישה 1.1 כניסה של אורח (התחברות של אורח)
    ///////////////דרישה 1.2 יציאה 
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

