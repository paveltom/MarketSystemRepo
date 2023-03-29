using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.Domain_Layer;
using Market_System.Domain_Layer.User_Component;
using Market_System.Service_Layer;

namespace Market_System.Service_Layer
{
    public class User_Service_Controller
    {
        //private user_facade (domain)
        private MarketSystem market_System;
        private string username; //TODO:: Change it later - to get this username from session-key
        
        public User_Service_Controller()
        {
            this.market_System = MarketSystem.GetInstance();
            username = "";
        }

        //TODO:: CHANGE TO THROW A RESPONSE;
        public string Login_Member(string username, string password) // 1.4
        {
            try
            {
                market_System.Login(username, password);
                this.username = username;
                return "Logged-In succesfully";
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }
        public void login_guest()//1.1
        {
            market_System.login_guest();

        }

        //TODO:: CHANGE TO THROW A RESPONSE;
        public string Logout()//3.1
        {
            try
            {
                market_System.Logout(username);
                username = "";
                return "Logged-out succesfully";
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }

      
        public string register(string username,string password) // 1.3
        {
            try
            {
                market_System.register(username,password);
                
                return "registered succesfully";
            }

            catch (Exception e)
            {
                return e.Message;
            }

        }
        public void add_product_to_basket()
        {

        }
        public void remove_product_from_basket()
        {

        }
        public void assign_new_owner() // 4.4
        {

        }
        public void assign_new_manager() // 4.6
        {

        }
        public void edit_manger_permissions() //4.7
        {

        }
        public void get_managers_of_store() //4.11
        {

        }
        public void get_purchase_history_of_a_member() //6.4
        {
            


        }





    }
}