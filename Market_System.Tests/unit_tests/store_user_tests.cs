using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Threading;
using Market_System.DomainLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Generic;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class store_user_tests
    {
        private Market_System.DomainLayer.MarketSystem ms;
        private UserFacade user_facade;
        private StoreFacade store_facade;



        // Use TestInitialize to run code before running each test 
         [TestInitialize()]
        public void Setup() {
            ms = Market_System.DomainLayer.MarketSystem.GetInstance();
            user_facade = UserFacade.GetInstance();
            store_facade = StoreFacade.GetInstance();
        }
        
       // Use TestCleanup to run code after each test has run
         [TestCleanup()]
        public void TearDown()
        {
            ms.destroy_me();
            store_facade.Destroy_me();
            user_facade.Destroy_me();
            UserRepo.GetInstance().destroy_me();
            PurchaseRepo.GetInstance().destroy_me();
        }


 
        [TestMethod]
        public void register_user_test_success()
        {
            
            user_facade.register("test1", "pass","address");
            Assert.AreEqual(true, user_facade.check_if_user_exists("test1", "pass"));
            
        }

        [TestMethod]
        public void register_user_test_failure()
        {
            
            user_facade.register("test1", "pass", "address");
            try
            {
                user_facade.register("test1", "pass", "address");
                Assert.Fail("Expected this test to fail, but was a success - the same user was registered twice");
            }

            catch (Exception e)
            {
                Assert.AreEqual("a user with same name exists, please change name!", e.Message);
            }

            
        }

        [TestMethod]
        public void Login_user_test_success()
        {
            
            user_facade.register("test1", "pass", "address");
            try
            {
                user_facade.Login("test1", "pass");
                Assert.AreEqual("Member", user_facade.Get_User_State("test1"));
            }

            catch(Exception e)
            {
                Assert.Fail("This test shouldn't have failed, but received this error: " + e.Message);
            }

            
        }

        [TestMethod]
        public void Login_user_test_failure()
        {
            
            user_facade.register("test1", "pass", "address");
            try
            {
                user_facade.Login("test2", "pass");
                Assert.Fail("This test should've failed - no such user as 'test2'");
            }

            catch (Exception e)
            {
                Assert.AreEqual("Incorrect login information has been provided", e.Message);
            }

            
        }

        [TestMethod]
        public void Logout_user_test_success()
        {
           
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.Logout("test1");
                Assert.AreEqual("Guest", user_facade.Get_User_State("test1"));
            }

            catch (Exception e)
            {
                Assert.Fail("This test shouldn't have failed, but received this error: " + e.Message);
            }

            
        }

        [TestMethod]
        public void Logout_user_test_failure_1()
        {
            
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.Logout("test1");
                user_facade.Logout("test1");
                Assert.Fail("This test should've failed - Already logged-out");
            }

            catch (Exception e)
            {
                Assert.AreEqual("You're already logged-out", e.Message);
            }

           
        }
                

        [TestMethod]
        public void Logout_user_test_failure_2()
        {
            
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Logout("test1");
                Assert.Fail("This test should've failed - Already logged-out");
            }

            catch (Exception e)
            {
                Assert.AreEqual("You're already logged-out", e.Message);
            }

        }

        [TestMethod]
        public void Logout_user_test_failure_3()
        {
            
            try
            {
                user_facade.Logout("test1");
                Assert.Fail("This test should've failed - Already logged-out, no such user exists either.");
            }

            catch (Exception e)
            {
                Assert.AreEqual("user does not exists", e.Message);
            }

        }

        [TestMethod]
        public void Login_guest_test_success()
        {
            try
            {
                user_facade.Login_guset("test1");
                Assert.AreEqual("Guest", user_facade.Get_User_State("test1"));
            }

            catch (Exception e)
            {
                Assert.AreEqual("for shakuras", e.Message);
            }

        }

        


        [TestMethod]
        public void user_changes_password_success()
        {

            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.change_password("test1","newpass");
                user_facade.Logout("test1");
                user_facade.Login("test1", "newpass");
                Assert.AreEqual("Member", user_facade.Get_User_State("test1"));
            }

            catch (Exception e)
            {
                Assert.Fail("This test shouldn't have failed, but received this error: " + e.Message);
            }


        }


        [TestMethod]
        public void user_changes_password_fail()
        {

            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.change_password("test1", "newpass");
                user_facade.Logout("test1");
                user_facade.Login("test1", "pass");
                Assert.Fail("This test should've failed - test1 changed password");
            }

            catch (Exception e)
            {
                Assert.AreEqual("Incorrect login information has been provided", e.Message);
            }

           



        }



        [TestMethod]
        public void add_product_to_basket_success()
        {
            user_facade.register("test1", "pass", "address");
            user_facade.Login("test1", "pass");
            user_facade.add_product_to_basket("123_456", "test1",1);
            Assert.AreEqual(true, user_facade.get_cart("test1").get_basket("123").check_if_product_exists("123_456"));
        }

        [TestMethod]
        public void remove_product_from_basket_and_remains_some_products()
        {
            user_facade.register("test1", "pass", "address");
            user_facade.Login("test1", "pass");
            user_facade.add_product_to_basket("123_456", "test1",1);
            user_facade.add_product_to_basket("123_456", "test1",1);
            user_facade.remove_product_from_basket("123_456", "test1");
            Assert.AreEqual(true, user_facade.get_cart("test1").get_basket("123").check_if_product_exists("123_456"));
        }

        [TestMethod]
        public void remove_product_from_basket_and_nothing_remains_in_it()
        {
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.add_product_to_basket("123_456", "test1",1);
                user_facade.remove_product_from_basket("123_456", "test1");
                user_facade.get_cart("test1").get_basket("123");
                Assert.Fail("This test should've failed - no product is left in this basket then basket should not be existed");
            }
            catch(Exception e)
            {
                Assert.AreEqual("basket does not exists", e.Message);
            }


        }

        [TestMethod]
        public void remove_product_from_not_existing_basket()
        {
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.remove_product_from_basket("123_456", "test1");
                Assert.Fail("This test should've failed - basket does not exists");
            }
            catch (Exception e)
            {
                Assert.AreEqual("basket does not exists", e.Message);
            }

        }


        /*
        [TestMethod]
        public void add_product_to_basket_in_parallel_success()
        {
            ms.Add_Product_To_basket("1231", "my");
            Thread sleeping_thread=new Thread(new ThreadStart(() => { Thread.Sleep(1000); ms.addproduct("user1") and shit}))
            Thread fast_thread=new Thread(new ThreadStart(() => {  ms.addproduct("user2") and shit}))
        expext user2 to sccuess and user 1 to fail

        }

       */



        [TestMethod]
        public void Check_Delivery_Success()
        {
            user_facade.register("test1", "pass", "address");
            user_facade.Login("test1", "pass");
            try
            {
               Assert.AreEqual("Delivery is available", Market_System.DomainLayer.MarketSystem.GetInstance().Check_Delivery("address"));
            }

            catch(Exception e)
            {
                Assert.Fail("Should've made the delivery possible, but failed due to error: " + e.Message);
            }
        }
        

        
        [TestMethod]
        public void Check_Out_Success()
        {
         
            
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.add_product_to_basket("132_151", "test1", 1);
                user_facade.Check_Out("test1");
                
            }

            catch (Exception e)
            {
                Assert.Fail("Should've made the delivery possible, but failed due to error: " + e.Message);
            }
        }
        
        [TestMethod]
        public void Check_Out_Failure()
        {
            
            
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.Check_Out("test1");
            }

            catch (Exception e)
            {
                Assert.AreEqual("Payment has failed, either your cart is empty",  e.Message);
            }
        }
        
        

        
        [TestMethod]
        public void user_purchase_history_success()
        {
            
            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.add_product_to_basket("132_151", "test1", 1);
                user_facade.Check_Out("test1");
                user_facade.save_purhcase_in_user("test1", user_facade.get_cart("test1"));
               

                string should_be = DateTime.Now.ToShortDateString() + ": \n" + "basket 132 : \n" + "product 132_151"+ " quantity: 1\n";
                Assert.AreEqual(should_be, user_facade.get_purchase_history_of_a_member("test1")[0].tostring());
            }

            catch (Exception e)
            {
                Assert.Fail("Should've showed the purchase history succefully, but failed due to error: " + e.Message);
            }
        }


        [TestMethod]
        public void user_purchase_history_fail()
        {

            try
            {
                user_facade.register("test1", "pass", "address");
                user_facade.Login("test1", "pass");
                user_facade.get_purchase_history_of_a_member("test1")[0].tostring();
                Assert.Fail("Should've failed , but was successful somehow ");
              
            }

            catch (Exception e)
            {
                Assert.AreEqual("user never bought anything!", e.Message);
            }
        }

    }
}
