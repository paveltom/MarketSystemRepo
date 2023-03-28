using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.Domain_Layer.Store_Component;

namespace Market_System.service_layer
{
    public class store_service_controller
    {
        // private store_facade (domain)
        private StoreFacade sf;
        public store_service_controller()
        {
            this.sf = new StoreFacade();
        }

        
        public void get_shop()
        {
            // this.sf.getshop() example
        }
        public void get_products_from_shop()
        {

        }
        public void search_product_by_category() //2.2
        {

        }
        public void search_product_by_keyword() //2.2
        {

        }
        public void search_product_by_name() //2.2
        {

        }
        public void apply_purchase_policy() //2.5
        {

        }
        public void open_new_store() // 3.2
        {

        }
        public void comment_on_product() // 3.3
        {

        }
        public void add_product_to_store() //4.1
        {

        }
        public void remove_product_from_store() //4.1
        {

        }
        public void edit_product_details() //4.1
        {

        }
        public void assign_new_owner() // 4.4
        {

        }
        public void assign_new_manager() // 4.6
        {
            
        }
        public void close_store() //4.9
        {

        }
        public void get_managers_of_store() //4.11
        {

        }
        public void get_purchase_history_from_store() //4.13
        {

        }
 


    }
}