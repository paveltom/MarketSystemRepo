using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.Domain_Layer.User_Component;

namespace Market_System.service_layer
{
    public class user_service_controller
    {
        //private user_facade (domain)
        private UserFacade userFacade;
        public user_service_controller()
        {
            this.userFacade = UserFacade.GetInstance();
        }

        public void login_member()// 1.4
        {
            //this.uf.login()  example
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