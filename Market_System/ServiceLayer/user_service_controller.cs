﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.ServiceLayer;


namespace Market_System.ServiceLayer
{
    public class User_Service_Controller
    {
        //private user_facade (domain)
        
        private MarketSystem market_System;
        
        public User_Service_Controller()
        {

            this.market_System = MarketSystem.GetInstance();
            
        }

        //TODO:: CHANGE TO THROW A RESPONSE;
        public string Login_Member(string username, string password,string session_id) // 1.4
        {
            try
            {
                
               string guest_id= market_System.unlink_userID_with_session(session_id); // unlinks the guest, because when we enter website we are a guest with sessino id
                market_System.remove_guest_id_from_userRepo(guest_id);
                market_System.check_username_is_logged_out(username);
                market_System.Login(username, password);
                market_System.link_user_with_session(username, session_id);
               
                return "Logged-In succesfully";
            }

            catch(Exception e)
            {
                //if something fail we should re login as guest
                login_guest(session_id);
                throw e;
            }
        }

        internal Dictionary<string, string> extract_item_from_basket(string product_id,string session_id)
        {
            return market_System.extract_item_from_basket(product_id, session_id);
        }

        internal bool check_if_user_bought_item(string product_id, string session_id)
        {
            try
            {
                return this.market_System.check_if_user_bought_item(product_id, session_id);
                
            }
            catch (Exception e)
            {
                 throw e;
            }
        }

        public string login_guest(string session_id)//1.1
        {
            try
            {
                string guest_name = market_System.login_guest();
                string guest_id=market_System.link_guest_with_user_id(guest_name);
                market_System.link_guest_with_session(guest_id, session_id);
                return guest_name;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal Response<List<string>> show_basket_in_cart(string selected_store_id,string session_id)
        {
            Response<List<string>> list_of_items = Response < List<string> > .FromValue(market_System.show_basket_in_cart(selected_store_id, session_id));
            return list_of_items;
        }

        public string get_userID_from_session_id(string session_id)
        {

            return market_System.get_userid_from_session_id(session_id);

        }


        public string Logout(string session_id)//3.1
        {
            try
            {
                string user_id = market_System.get_userid_from_session_id(session_id);
               
                market_System.Logout(user_id);
                market_System.unlink_userID_with_session(session_id);
               
                return user_id+"Logged-out succesfully";
            }

            catch(Exception e)
            {
                throw e;
            }
        }

      
        public string register(string username,string password,string address) // 1.3
        {
            try
            {
                market_System.register(username,password,address);      
                return "registered succesfully";
            }

            catch (Exception e)
            {
                throw e;
            }

        }

        internal bool check_if_current_user_is_admin(string session_id)
        {
            return market_System.check_if_current_user_is_admin(session_id);
        }

        public List<string> get_store_ids_from_cart(string session_id)
        {
            try
            {

                return market_System.get_store_ids_from_cart(session_id);
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string add_product_to_basket(string product_id,string session_id,string quantity)
        {
            try
            {
           
                market_System.ReserveProduct(new ItemDTO(product_id, int.Parse(quantity)));
                return market_System.Add_Product_To_basket(product_id, session_id, quantity);
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public void save_purhcase_in_user(string session_id)
        {
            try
            {


                this.market_System.save_purhcase_in_user(session_id);


            }

            catch (Exception e)
            {
                //TODO:: לבטל שריון של ההזמנה!!!!
                throw e;

            }
        }
        public string remove_product_from_basket(string product_id, string session_id,string quantity)
        {
            try
            {

                market_System.LetGoProduct(new ItemDTO(product_id, int.Parse(quantity)));
                
                return market_System.remove_product_from_basket(product_id, session_id, int.Parse(quantity));
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /*
        public string assign_new_owner(string founder, string username, int store_ID) // 4.4
        {
            try
            {
                market_System.Assign_New_Owner(founder, username, store_ID);
                return "new owner has been assigned succesfully";
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        public string assign_new_manager(string founder, string username, int store_ID) // 4.6
        {
            try
            {
                market_System.Assign_New_Manager(founder, username, store_ID);
                return "new manager has been assigned succesfully";
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        */
        public void edit_manger_permissions() //4.7
        {

        }
        public void get_managers_of_store() //4.11
        {

        }
        public List<PurchaseHistoryObj> get_purchase_history_of_a_member(string session_id) //6.4
        {

           
                return market_System.get_purchase_history_of_a_member(session_id);

        }
        
        public string Check_Delivery(string address)
        {
            try
            {
                return market_System.Check_Delivery(address);
            }

            catch(Exception e)
            {
                throw e;
            }
        }
        
        
        public string Check_Out(string session_id,string credit_card)
        {
            try
            {

                return market_System.Check_Out(session_id, credit_card);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void destroy()
        {
            market_System.destroy_me();
        }

        internal string change_password(string new_password, string session_id)
        {

            
            return  market_System.change_password(session_id,  new_password);

   
        }

        public bool isLoggedInAdministrator(string session_ID)
        {
            try
            {
                
                return market_System.isLoggedInAdministrator(session_ID);
            }

            catch (Exception e)
            {
                throw e;
            }

        }

        public string AddNewAdmin(string sessionID, string Other_username)
        {
            try
            {
                market_System.AddNewAdmin(sessionID, Other_username);
                return "Admin has been added succefully";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckIfAdmin(string sessionID, string Other_username)
        {
            try
            {
                return market_System.CheckIfAdmin(sessionID, Other_username);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Remove_A_Member(string sessionID, string member_Username)
        {
            try
            {
                market_System.Remove_A_Member(sessionID, member_Username);
                return "removed the member successfully";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public MemberDTO Get_Member_Info(string session_id, string member_Username)
        {
            try
            {
                return market_System.Get_Member_Info(session_id, member_Username);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal string getusername(string session_id)
        {
            return market_System.getusername(session_id);
        }

        internal bool HasNewMessages(string session_id)
        {
            try
            {
                return market_System.HasNewMessages(session_id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> GetMessages(string session_id)
        {
            try
            {
                return market_System.GetMessages(session_id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal string getAdressOrEmpty(string session_id)
        {
            try
            {
                string userid = market_System.get_userid_from_session_id(session_id);
                return market_System.getUserAdress(userid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}