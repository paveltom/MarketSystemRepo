using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer;
using Market_System.ServiceLayer;

namespace Market_System.Service_Layer
{
    public class Store_Service_Controller
    {
        // private store_facade (domain)
        private MarketSystem market_System;

        public Store_Service_Controller()
        {
            this.market_System = MarketSystem.GetInstance();
        }

        
        public void get_store()
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
        public string open_new_store(string username, int store_ID) // 3.2
        {
            try
            {
                market_System.Add_New_Store(username, store_ID);
                return "Store has been opened succesfully";
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }
        public void comment_on_product() // 3.3
        {

        }
        public string add_product_to_store(int store_ID, string founder, Product product, int quantity) //4.1
        {
            try
            {
                market_System.Add_Product_To_Store(store_ID, founder, product, quantity);
                return "Product has been added succesfully";
            }

            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string remove_product_from_store(int store_ID, string founder, Product product) //4.1
        {
            try
            {
                market_System.Remove_Product_From_Store(store_ID, founder, product);
                return "Product has been removed succesfully";
            }

            catch (Exception e)
            {
                return e.Message;
            }
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