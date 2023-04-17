using System;
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
                market_System.unlink_user_with_session(session_id); // unlinks the guest, because when we enter website we are a guest with sessino id
                market_System.Login(username, password);
                market_System.link_user_with_session(username, session_id);
               
                return "Logged-In succesfully";
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }
        public string login_guest(string session_id)//1.1
        {
            string guest_name= market_System.login_guest();
            
            market_System.link_user_with_session(guest_name, session_id); 
            return guest_name;

        }


        public string get_username_from_session_id(string session_id)
        {
            return market_System.get_username_from_session_id(session_id);
        }


        public string Logout(string session_id)//3.1
        {
            try
            {
                string username = market_System.get_username_from_session_id(session_id);
               
                market_System.Logout(username);
                market_System.unlink_user_with_session(session_id);
               
                return username+"Logged-out succesfully";
            }

            catch(Exception e)
            {
                return e.Message;
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
                return e.Message;
            }

        }
        public string add_product_to_basket(string product_id,string session_id,string quantity)
        {
            try
            {
                string username = market_System.get_username_from_session_id(session_id);
                market_System.ReserveProduct(new ItemDTO(product_id, int.Parse(quantity)));
                return market_System.Add_Product_To_basket(product_id,username,quantity);
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }
        public string remove_product_from_basket(string product_id, string session_id)
        {
            try
            {
                string username = market_System.get_username_from_session_id(session_id);
                return market_System.remove_product_from_basket(product_id, username);
            }
            catch (Exception e)
            {
                return e.Message;
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
                return e.Message;
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
                return e.Message;
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
            string username = market_System.get_username_from_session_id(session_id);
                return market_System.get_purchase_history_of_a_member(username);
        }
        
        public string Check_Delivery(string address)
        {
            try
            {
                return market_System.Check_Delivery(address);
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }
        
        
        public string Check_Out(string username,string credit_card, Cart cart)
        {
            try
            {
                return market_System.Check_Out(username, credit_card, cart);
            }

            catch (Exception e)
            {
                return e.Message;
            }
        }

        internal void destroy()
        {
            market_System.destroy_me();
        }

        internal string change_password(string new_password, string session_id)
        {
            string username = market_System.get_username_from_session_id(session_id);
            return  market_System.change_password(username,  new_password);
        }

        public string Read_System_Events(string session_ID)
        {

        }

        public string Read_System_Errors(string session_ID)
        {

        }
    }
}