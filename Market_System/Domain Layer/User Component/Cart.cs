using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.User_Component
{
    public class Cart
    {
        private List<Bucket> baskets;
        private double total_price;
        public Cart()
        {
            this.baskets = new List<Bucket>();
            this.total_price = 0;
        }

        public void add_product(string product_id)
        {
            string store_id = product_id.Substring(0, 3);// 3 first digist are store id


           if(!check_if_basket_of_store_exists(store_id))
            {
                this.baskets.Add(new Bucket(store_id));
            }
           
            foreach (Bucket basket in this.baskets)
            {
                if (basket.get_store_id().Equals(store_id))
                {
                    basket.add_product(product_id);
                    return;
                }

            }
        }

        public bool check_if_basket_of_store_exists(string store_id)
        {
            foreach (Bucket basket in this.baskets)
            {
                if (basket.get_store_id().Equals(store_id))
                {
                    return true;
                }

            }
            return false;
        }

        public void update_total_price(double price)
        {
            this.total_price = price;
        }
    }
}