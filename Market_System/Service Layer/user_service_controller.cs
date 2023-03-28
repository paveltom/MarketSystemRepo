using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.Domain_Layer;
using Market_System.Domain_Layer.User_Component;

namespace Market_System.Service_Layer
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
        public string Login_Member(string username, string password) // 1.4
        {
            try
            {
                market_System.Login(username, password);
                return "Logged-In succesfully";
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }
        public void login_guest()//1.1
        {
        }
        public void log_out()//3.1
        {

        }

      
        public void register() // 1.3
        {

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