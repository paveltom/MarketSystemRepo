using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.User_Component
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
            this.purchase_date = DateTime.Now.ToString();
        }




        private List<Bucket> copy_list(List<Bucket> list)
        {
            List<Bucket> new_list = new List<Bucket>();
            foreach(Bucket basket in list)//copies basktes
            {
                Bucket basket_copy = new Bucket(basket.get_store_id());

                foreach(string product_id in basket.get_products())//copies products in basket
                    {
                    basket_copy.add_product(product_id);

                }
                new_list.Add(basket_copy);
            }
            return new_list;
        }

    }
}