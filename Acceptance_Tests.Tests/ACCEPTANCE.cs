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




        [ClassInitialize()]
        public static void runs_before_first_test_runs(TestContext testContext)
        {
            Logger.get_instance().change_logger_path_to_tests();
        }

        [ClassCleanup()]
        public static void runs_after_last_test_finishes_running()
        {
            Logger.get_instance().change_logger_path_to_regular();
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
            Response<StoreDTO> response = service.open_new_store(new List<string> { "store_123" });
            //the bug is response.Value.StoreID
            Response<ItemDTO> resProdAdd = service.add_product_to_store(response.Value.StoreID, "Prod1", "desc1", "1", "1", "1", "2.0", "1.0", "5.0", "5.0_2.0", "attr", "new_category");
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
            Response<string> response = service.register("user5", "pass1", "add1");

            //Result:
            Assert.AreEqual(false, response.ErrorOccured);

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
            Assert.AreEqual(false, responseLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        //not sign in 
        [TestMethod]
        public void LogoutMemberfailed()
        {
            //Setup: 
            oneThreadSetUp();
            //Action:
            Response<string> responseLogout = service.log_out();

            //Result:
            Assert.AreEqual(true, responseLogout.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }
        //check this test
        [TestMethod]
        public void AssignNewMannegerSuccess()
        {
            //Setup: 
            //LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");
            oneThreadSetUp();
            //Action:
            Response<string> responseLogout = service.assign_new_manager("Store1", "user1");
            //Result:
            Assert.AreEqual(false, responseLogout.ErrorOccured);
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
            Assert.AreEqual(true, responseReg2.ErrorOccured);

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
            Assert.AreEqual(true, responseLogin.ErrorOccured);

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
            Assert.AreEqual(true, responseLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }
        //Assign request failed because the user must be registered to the system so he can sign in 
        [TestMethod]
        public void failLoginNotRegister()
        {
            //Setup: 
            oneThreadSetUp();
            //Action:
            Response<string> responseLogin = service.login_member("user1", "pass1");

            //Result:
            Assert.AreEqual(true, responseLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void LoginAdminSuccess()
        {
            //Setup: 
            oneThreadSetUp();

            //Action:
            Response<string> responseAdminLogin = service.login_member("admin", "admin");

            //Result:
            Assert.AreEqual(false, responseAdminLogin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void AddNewAdminSuccess()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseAdminLogin = service.login_member("admin", "admin");

            //Action:
            Response<string> responseRegisterNewMember = service.register("bobinka", "1234567", "addrr123");
            Response<string> responseAddNewAdmin = service.AddNewAdmin("bobinka");

            //Result:
            Assert.AreEqual(false, responseAddNewAdmin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void AddNewAdminFailure()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseRegisterUser = service.register("user1", "admin", "bulyard123");
            Response<string> responseUserLogin = service.login_member("user1", "admin");

            //Action:
            Response<string> responseRegisterNewMember = service.register("bobinka", "1234567", "addrr123");
            Response<string> responseAddNewAdmin = service.AddNewAdmin("bobinka");

            //Result:
            Assert.AreEqual(true, responseAddNewAdmin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void CheckIfAdminSucceess_returnsTrue()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseAdminLogin = service.login_member("admin", "admin");

            //Action:
            Response<string> responseCheckIfAdmin = service.CheckIfAdmin("admin");

            //Result:
            Assert.AreEqual(false, responseCheckIfAdmin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void CheckIfAdminSucceess_returnsFalse()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseAdminLogin = service.login_member("admin", "admin");
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");

            //Action:
            Response<string> responseCheckIfAdmin = service.CheckIfAdmin("yotam");

            //Result:
            Assert.AreEqual(false, responseCheckIfAdmin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void CheckIfAdminFailure()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");
            Response<string> responseAdminLogin = service.login_member("yotam", "123554");

            //Action:
            Response<string> responseCheckIfAdmin = service.CheckIfAdmin("yotam");

            //Result:
            Assert.AreEqual(true, responseCheckIfAdmin.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void RemoveNewOwnerSuccess()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");
            Response<string> responseotherUserRegister = service.register("layan", "123554", "meonot g");
            Response<string> responseUserLogin = service.login_member("yotam", "123554");
            Response<StoreDTO> responseOpenStoreUser = service.open_new_store(new List<string> { "store_123" });
            Response<string> assignOtherAsOwner = service.assign_new_owner(responseOpenStoreUser.Value.StoreID, "layan");

            //Action:
            Response<string> responseRemoveOtherOwner = service.Remove_Store_Owner(responseOpenStoreUser.Value.StoreID, "layan");

            //Result:
            Assert.AreEqual(false, responseRemoveOtherOwner.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void RemoveNewOwnerFailure()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");
            Response<string> responseanotherUserRegister = service.register("bayanka", "12345", "meonot g");
            Response<string> responseotherUserRegister = service.register("layan", "123554", "meonot g");
            Response<string> responseUserLogin = service.login_member("yotam", "123554");
            Response<StoreDTO> responseOpenStoreUser = service.open_new_store(new List<string> { "store_123" });
            Response<string> assignOtherAsOwner = service.assign_new_owner(responseOpenStoreUser.Value.StoreID, "layan");
            service.assign_new_owner(responseOpenStoreUser.Value.StoreID, "bayanka");
            service.log_out();
            service.login_member("bayanka", "12345");

            //Action:
            Response<string> responseRemoveOtherOwner = service.Remove_Store_Owner(responseOpenStoreUser.Value.StoreID, "layan");

            //Result:
            Assert.AreEqual(true, responseRemoveOtherOwner.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void RemoveAMemberSuccess()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseotherUserRegister = service.register("layan", "123554", "meonot g");
            Response<string> responseAdminLogin = service.login_member("admin", "admin");

            //Action:
            Response<string> responseRemoveAMember = service.Remove_A_Member("layan");

            //Result:
            Assert.AreEqual(false, responseRemoveAMember.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void RemoveAMemberFailure()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");
            Response<string> responseotherUserRegister = service.register("layan", "123554", "meonot g");
            Response<string> responseUserLogin = service.login_member("yotam", "123554");
            Response<StoreDTO> responseOpenStoreUser = service.open_new_store(new List<string> { "store_123" });
            Response<string> assignOtherAsOwner = service.assign_new_owner(responseOpenStoreUser.Value.StoreID, "layan");
            service.log_out();
            Response<string> responseAdminLogin = service.login_member("admin", "admin");

            //Action:
            Response<string> responseRemoveAMember = service.Remove_A_Member("layan");

            //Result:
            Assert.AreEqual(true, responseRemoveAMember.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void GetMemberSuccess()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseotherUserRegister = service.register("layan", "123554", "meonot g");
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");
            Response<string> responseUserLogin = service.login_member("yotam", "123554");
            Response<StoreDTO> responseOpenStoreUser = service.open_new_store(new List<string> { "store_123" });
            Response<string> assignOtherAsOwner = service.assign_new_owner(responseOpenStoreUser.Value.StoreID, "layan");
            service.log_out();

            Response<string> responseAdminLogin = service.login_member("admin", "admin");
            service.AddNewAdmin("layan");

            //Action:
            Response<MemberDTO> responseGetMemberInfo = service.GetMemberInfo("layan");

            //Result:
            Assert.AreEqual(false, responseGetMemberInfo.ErrorOccured);
            Assert.AreEqual(true, responseGetMemberInfo.Value.IsAdmin());

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void GetMemberFailure()
        {
            //Setup: 
            oneThreadSetUp();
            Response<string> responseotherUserRegister = service.register("layan", "123554", "meonot g");
            Response<string> responseRegisterUser = service.register("yotam", "123554", "meonot g");
            Response<string> responseUserLogin = service.login_member("yotam", "123554");
            Response<StoreDTO> responseOpenStoreUser = service.open_new_store(new List<string> { "store_123" });
            Response<string> assignOtherAsOwner = service.assign_new_owner(responseOpenStoreUser.Value.StoreID, "layan");

            //Action:
            Response<MemberDTO> responseGetMemberInfo = service.GetMemberInfo("layan");

            //Result:
            Assert.AreEqual(null, responseGetMemberInfo);

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
            //oneThreadSetUp();
            //Action:
            Response<string> responseLogout = service.log_out();

            //Result:
            Assert.AreEqual(false, responseLogout.ErrorOccured);
            Response<string> responseLogoutAgain = service.log_out();
            Assert.AreEqual(true, responseLogoutAgain.ErrorOccured); //already logged out

            //tearDown:
            oneThreadCleanup();
        }

        //TODO:: Uncomment this when @Pasha fixes this - problem with Employees... when opening a new store 
        
        [TestMethod]
        public void openStoreSuccess()
        {
            //Setup: 
            registeredLoggedInMemberSetUp("user1", "pass1", "add1");

            //Action:
            Response<StoreDTO> response = service.open_new_store(new List<string> { "Store_123" }); //TODO:: Should get STOREDTO here which contains the storeID

            //Result:
            Assert.AreEqual(false, response.ErrorOccured);
            //todo: check if store was added:
            Response<StoreDTO> resGetStore = service.GetStore(response.Value.StoreID);
            Assert.AreEqual(response.Value.StoreID, resGetStore.Value.StoreID);

            //tearDown:
            oneThreadCleanup();
        }

        [TestMethod]
        public void comment_on_product()
        {
            //Setup: 
            //LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");
            oneThreadSetUp();

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.comment_on_product("Store1_Prod1", "newName is very bad product", 0.5);

            //Result:
            Assert.AreEqual(false, response.ErrorOccured);
            //todo: check if prod comment added
            //check if new name there

            //tearDown:
            oneThreadCleanup();
        }
        #endregion

        #region//Owner  actions Tests 4.1
        [TestMethod]
        public void addproduct_success()
        {
            //Setup: 
            registeredLoggedInMemberSetUp("user1", "pass1", "add1");
            //oneThreadSetUp();

            //Action:
            Response<StoreDTO> response = service.open_new_store(new List<string> { "Store_ 123"});
            Response<ItemDTO> resProdAdd = service.add_product_to_store(response.Value.StoreID, "prod1", "desc1", "1", "1", "1", "2.0", "2.0", "5.0", "5.0_2.0", "attr", "catg"); 

            //Result:
            Assert.AreEqual(false, resProdAdd.ErrorOccured);
            //todo: check if prod was added
            //Response < List < ItemDTO >> resProdAdded = service.get_products_from_shop("Store1");

            //Assert.AreEqual(false, resProdAdded.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }
        [TestMethod]
        public void addproduct_failure()
        {
            //Setup: 
            //registeredLoggedInMemberSetUp("user1", "pass1", "add1");
            oneThreadSetUp();

            //Action:
            Response<StoreDTO> response = service.open_new_store(new List<string> { "Store_ 123" });
            Response<ItemDTO> resProdAdd = service.add_product_to_store("fake_ID_123", "prod1", "desc1", "1", "1", "1", "2.0", "2.0", "5.0", "5.0_2.0", "attr", "catg");

            //Result:
            Assert.AreEqual(null, resProdAdd);
            //todo: check if prod was added
            //Response < List < ItemDTO >> resProdAdded = service.get_products_from_shop("Store1");

            //Assert.AreEqual(false, resProdAdded.ErrorOccured);

            //tearDown:
            oneThreadCleanup();
        }


        [TestMethod]
        public void changeProdName()
        {
            //Setup: 
            //LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");
            oneThreadSetUp();

            //Action:
            //todo: what is the prod id?
            Response<string> response = service.ChangeProductName("Store1_Prod1", "newName");

            //Result:
            Assert.AreEqual(false, response.ErrorOccured);
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
            Assert.AreEqual(false, response.ErrorOccured);
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
        //TODO:: Uncomment this test when @Pasha fixes The Employees() function... fails when opening a new store
        /*
        [TestMethod]
        public void searchProducts()
        {
            //Setup: 
            LoggedInOwnerWithOpenedOneProdStore("user1", "pass1", "add1");

            //Action:
            //todo:
            Response<List<ItemDTO>> response = service.search_product_by_name("Prod1");

            //Result:
            Assert.AreEqual(false, response.ErrorOccured);
            Assert.AreEqual(1, response.Value.Capacity);
            //tearDown:
            oneThreadCleanup();
        } */

        //add more search options tests here:
        /*
         * 
         * 
         */

        //TODO:: Uncomment this test when @Pasha fixes The Employees() function... fails when opening a new store
        /*
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
        */

        //TODO:: Uncomment this test when @Pasha fixes The Employees() function... fails when opening a new store
        /*
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
            //oneThreadCleanup();
        }
        */


        //TODO:: Uncomment this test when @Pasha fixes The Employees() function... fails when opening a new store
        /*
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
        */

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

