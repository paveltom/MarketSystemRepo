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


            if (!check_if_basket_of_store_exists(store_id))
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

        public Bucket get_basket(string store_id)
        {
            foreach (Bucket basket in this.baskets)
            {
                if (basket.get_store_id().Equals(store_id))
                {

                    return basket;
                }

            }

            throw new Exception("basket does not exists");
        }

        internal void remove_product(string product_id)
        {
            string store_id = product_id.Substring(0, 3);// 3 first digist are store id
            int remove_basket_flag = -1; // if it is set to 1 then delete , if it is still -1 then error basket does not exists

            foreach (Bucket basket in this.baskets)
            {
                if (basket.get_store_id().Equals(store_id))
                {
                    remove_basket_flag = basket.remove_product(product_id);

                }

            }
            if (remove_basket_flag == 1)
            {
                remove_basket(store_id);

            }
            if (remove_basket_flag == -1)
            {
                throw new Exception("basket does not exists");
            }
        }

        private void remove_basket(string store_id)
        {
            //because we are running from end to begging no worry about modifying the collection and running at it at the same time
            for(int i=this.baskets.Count()-1;i>=0;i--)
            {
                if(this.baskets[i].get_store_id()==store_id)
                {
                        this.baskets.RemoveAt(i);
                        return;
                }
            }
        }

        public List<Bucket> gett_all_baskets()
        {
            return this.baskets;
        }

        public double get_total_price()
        {
            return this.total_price;
        }

    }
}