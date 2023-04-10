using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.UserComponent
{

    public class Bucket
    {
        private string basket_id;
        private string store_id;
        private Dictionary<string, int> products;
        private Random id_generator;
        public Bucket(string store_id)
        {
            this.id_generator = new Random();
            this.basket_id = id_generator.Next().ToString();
            this.store_id = store_id;
            this.products = new Dictionary<string, int>();
        }

        public void add_product(string product_id)
        {

            if (!check_if_product_exists(product_id))
            {
                this.products.Add(product_id, 0);
            }


            this.products[product_id] = this.products[product_id] + 1;


        }

        public string get_store_id()
        {
            return this.store_id;
        }

        public bool check_if_product_exists(string product_id)
        {
            return this.products.ContainsKey(product_id);
        }

        public Dictionary<string, int> get_products()
        {
            return this.products;
        }

        internal int remove_product(string product_id)
        {
            this.products[product_id] = this.products[product_id] - 1;
            if(this.products[product_id]==0)
            {
                this.products.Remove(product_id);
            }
            if(this.products.Count()==0)
            {
                return 1;
            }
            return 0;
        }

        public List<ItemDTO> convert_basket_to_dtos_list()
        {
            
            List<ItemDTO> list_of_dtos = new List<ItemDTO>();
            foreach (KeyValuePair<string, int> entry in this.products) // each entry is < product_id , quantity > 
            {
                list_of_dtos.Add(new ItemDTO(entry.Key, entry.Value));
            }
            return list_of_dtos;
        }
    }
}