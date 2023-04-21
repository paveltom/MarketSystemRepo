using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using System.Collections.Generic;
using Market_System.DomainLayer.UserComponent;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class integration_tests
    {

        private MarketSystem ms;



        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Setup()
        {
            Logger.get_instance().change_logger_path_to_tests();
            ms = MarketSystem.GetInstance();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TearDown()
        {
            Logger.get_instance().change_logger_path_to_regular();
            ms.destroy_me();

        }
        [TestMethod]
        public void add_product_to_basket_integration_test_success()
        {
            try
            {
                ms.register("store_owner", "p@ssvv0rcl", "aiur");
              
                ms.Login("store_owner", "p@ssvv0rcl");
                ms.link_user_with_session("store_owner", "random_shit");
                string session_id = ms.get_session_id_from_username("store_owner");
                StoreDTO store_dto = ms.Add_New_Store(session_id, new List<string> { "store_123" });
                ItemDTO item_dto = ms.Add_Product_To_Store(store_dto.StoreID, session_id, new List<string> { "boots", "nice_boots", "100", "80", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "shoes" });
                string user_id = ms.get_userid_from_session_id(session_id);
                ms.Logout(user_id);
                ms.register("buyer", "p@ssvv0rcl999999", "shakuras");
               
                ms.Login("buyer", "p@ssvv0rcl999999");
                ms.link_user_with_session("buyer", "random_shit69");
                session_id = ms.get_session_id_from_username("buyer");
                user_id = ms.get_userid_from_session_id(session_id);
                ms.ReserveProduct(new ItemDTO(item_dto.GetID(), 10));
                ms.Add_Product_To_basket(item_dto.GetID(), session_id, "10");
               Cart cart= ms.get_cart_of_userID(user_id);
                Bucket basket = cart.get_basket(store_dto.StoreID);
                Dictionary<string, int> basket_products = basket.get_products();
                Assert.IsTrue(basket.get_store_id().Equals( store_dto.StoreID )&& basket_products[item_dto.GetID()]==10);
                StoreDTO store = ms.GetStore(store_dto.StoreID);
                List<ItemDTO> AllProducts = store.AllProducts;
                Assert.IsTrue(AllProducts[0].GetReservedQuantity()==10);



            }
            catch (Exception e)
            {
                Assert.Fail("this test shouldn't have failed!, but failed due to:  " + e.Message);
            }
        }

        [TestMethod]
        public void add_product_to_basket_integration_test_fail_due_to_trying_to_buy_an_amount_greater_than_what_is_actually_in_store()
        {
            try
            {
                ms.register("store_owner", "p@ssvv0rcl", "aiur");

                ms.Login("store_owner", "p@ssvv0rcl");
                ms.link_user_with_session("store_owner", "random_shit");
                string session_id = ms.get_session_id_from_username("store_owner");
                StoreDTO store_dto = ms.Add_New_Store(session_id, new List<string> { "store_123" });
                ItemDTO item_dto = ms.Add_Product_To_Store(store_dto.StoreID, session_id, new List<string> { "boots", "nice_boots", "100", "1", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "shoes" });
                string user_id = ms.get_userid_from_session_id(session_id);
                ms.Logout(user_id);
                ms.register("buyer", "p@ssvv0rcl999999", "shakuras");

                ms.Login("buyer", "p@ssvv0rcl999999");
                ms.link_user_with_session("buyer", "random_shit69");
                session_id = ms.get_session_id_from_username("buyer");
                user_id = ms.get_userid_from_session_id(session_id);
                ms.ReserveProduct(new ItemDTO(item_dto.GetID(), 10));
                ms.Add_Product_To_basket(item_dto.GetID(), session_id, "10");
                Assert.Fail("this test shouldn't have failed!, because there are no 10 boots in store, only 1");



            }
            catch (Exception e)
            {
                Assert.AreEqual("Can't reserve: quantity too large.", e.Message);

                

            }
        }


        [TestMethod]
        public void update_cart_total_price_after_adding_product_success()
        {
            try
            {
                ms.register("store_owner", "p@ssvv0rcl", "aiur");

                ms.Login("store_owner", "p@ssvv0rcl");
                ms.link_user_with_session("store_owner", "random_shit");
                string session_id = ms.get_session_id_from_username("store_owner");
                StoreDTO store_dto = ms.Add_New_Store(session_id, new List<string> { "store_123" });
                ItemDTO item_dto = ms.Add_Product_To_Store(store_dto.StoreID, session_id, new List<string> { "boots", "nice_boots", "100", "100", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "shoes" });
                string user_id = ms.get_userid_from_session_id(session_id);
                ms.Logout(user_id);
                ms.register("buyer", "p@ssvv0rcl999999", "shakuras");

                ms.Login("buyer", "p@ssvv0rcl999999");
                ms.link_user_with_session("buyer", "random_shit69");
                session_id = ms.get_session_id_from_username("buyer");
                user_id = ms.get_userid_from_session_id(session_id);
                ms.ReserveProduct(new ItemDTO(item_dto.GetID(), 10));
                ms.Add_Product_To_basket(item_dto.GetID(), session_id, "10");
                Cart cart = ms.get_cart_of_userID(user_id);
                
                
                Assert.IsTrue(cart.get_total_price() == 1000);



            }
            catch (Exception e)
            {
                Assert.Fail("this test shouldn't have failed!, but failed due to:  " + e.Message);



            }
        }
       
        
        [TestMethod]
        public void get_purchase_history_from_store_success()
        {
            try
            {
                ms.register("store_owner", "p@ssvv0rcl", "aiur");

                ms.Login("store_owner", "p@ssvv0rcl");
                ms.link_user_with_session("store_owner", "random_shit");
                string session_id = ms.get_session_id_from_username("store_owner");
                StoreDTO store_dto = ms.Add_New_Store(session_id, new List<string> { "store_123" });
                ItemDTO item_dto_1 = ms.Add_Product_To_Store(store_dto.StoreID, session_id, new List<string> { "boots", "nice_boots", "100", "80", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "shoes" });
                ItemDTO item_dto_2 = ms.Add_Product_To_Store(store_dto.StoreID, session_id, new List<string> { "shirt", "nice_shirt", "75", "20", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "shirts" });
                string user_id = ms.get_userid_from_session_id(session_id);
                ms.Logout(user_id);
                ms.register("buyer", "p@ssvv0rcl999999", "shakuras");

                ms.Login("buyer", "p@ssvv0rcl999999");
                ms.link_user_with_session("buyer", "random_shit69");
                session_id = ms.get_session_id_from_username("buyer");
                user_id = ms.get_userid_from_session_id(session_id);
                ms.ReserveProduct(new ItemDTO(item_dto_1.GetID(), 10));
                ms.Add_Product_To_basket(item_dto_1.GetID(), session_id, "10");
                ms.Check_Out("buyer", "5998-5858-7161-2561", ms.get_cart_of_userID(user_id));
                Cart cart = ms.get_cart_of_userID(user_id);
                ms.purchase(session_id, cart.convert_to_item_DTO());
                ms.Logout(user_id);



                ms.register("buyer2", "mypass16516", "char");

                ms.Login("buyer2", "p@mypass16516");
                ms.link_user_with_session("buyer2", "bullshit");
                session_id = ms.get_session_id_from_username("buyer2");
                user_id = ms.get_userid_from_session_id(session_id);
                ms.ReserveProduct(new ItemDTO(item_dto_2.GetID(), 5));
                ms.Add_Product_To_basket(item_dto_2.GetID(), session_id, "5");
                ms.Check_Out("buyer2", "5998-5858-7161-2561", ms.get_cart_of_userID(user_id));
                Cart cart_2 = ms.get_cart_of_userID(user_id);
                ms.purchase(session_id, cart_2.convert_to_item_DTO());
                ms.Logout(user_id);
                ms.Login("store_owner", "p@ssvv0rcl");
                 session_id = ms.get_session_id_from_username("store_owner");
                string shouldbe = DateTime.Now.ToShortDateString() + ":\n";
                shouldbe = shouldbe + "product " + item_dto_1.GetID() + " quantity: " + item_dto_1.GetQuantity() + "\n";
                 shouldbe = shouldbe+DateTime.Now.ToShortDateString() + ":\n";
                shouldbe = shouldbe + "product " + item_dto_2.GetID() + " quantity: " + item_dto_2.GetQuantity() + "\n";
                Assert.IsTrue(ms.GetStorePurchaseHistory(session_id, store_dto.StoreID).Equals(shouldbe));
                
                
                



            }
            catch (Exception e)
            {
                Assert.Fail("this test shouldn't have failed!, but failed due to:  " + e.Message);
            }
        }

    }











   
}


