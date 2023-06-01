using Market_System.user_component_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class PurchaseHistoryObj
    {
        private string username;
        private List<Bucket> baskets;
        private string purchase_date;
        private double total_price;

        public PurchaseHistoryObj(string username,List<Bucket> list_of_baskets,double price)
        {
            this.username = username;

            this.baskets = copy_list(list_of_baskets);
            this.total_price = price;
            this.purchase_date = DateTime.Now.ToShortDateString(); 
        }

        public bool check_if_contains_product(string product_id)
        {
            foreach(Bucket basket in baskets)
            {
                if(basket.check_if_product_exists(product_id))
                {
                    return true;
                }
            }
            return false;
        }
        public string tostring()
        {
            string return_me = purchase_date+": \n";
            foreach (Bucket basket in baskets)
            {
                return_me = return_me + "basket " + basket.get_store_id() + " : \n";

                foreach (KeyValuePair<string, int> product__pair in basket.get_products())//copies products in basket
                {
                    return_me = return_me + "product " + product__pair.Key + " quantity: " + product__pair.Value+"\n";

                }
                
            }

            return return_me;
        }

        private List<Bucket> copy_list(List<Bucket> list)
        {
            List<Bucket> new_list = new List<Bucket>();
            foreach(Bucket basket in list)//copies basktes
            {
                Bucket basket_copy = new Bucket(basket.get_store_id());

                foreach(KeyValuePair<string, int> product__pair in basket.get_products())//copies products in basket
                    {
                    basket_copy.add_product(product__pair.Key,product__pair.Value);

                }
                new_list.Add(basket_copy);
            }
            return new_list;
        }

        internal List<Bucket_model_history> convert_buckets_to_Bucket_model_history()
        {
            List<Bucket_model_history> return_me = new List<Bucket_model_history>();
            foreach (Bucket basket in this.baskets)
            {
                Bucket_model_history new_bmh = new Bucket_model_history(basket.get_basket_id(), basket.get_store_id());
                foreach(KeyValuePair<string, int> product__pair in basket.get_products())
                {
                    Product_in_basket_history_model pibhm = new Product_in_basket_history_model(product__pair.Key, product__pair.Value, basket.get_basket_id());
                    new_bmh.products.Add(pibhm);
                }
                
                   return_me.Add(new_bmh);

            }
            return return_me;
        }

        internal double get_total_price()
        {
            return this.total_price;
        }
    }
}