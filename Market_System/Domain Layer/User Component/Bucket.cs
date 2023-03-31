using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.User_Component
{

    public class Bucket
    {
        private string basket_id;
        private string store_id;
        private List<string> products;
        private Random id_generator;
        public Bucket(string store_id)
        {
            this.id_generator = new Random();
            this.basket_id = id_generator.Next().ToString();
            this.store_id = store_id;
        }

        public void add_product(string product_id)
        {
            this.products.Add(product_id);
        }

        public string get_store_id()
        {
            return this.store_id;
        }



    }
}