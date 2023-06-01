using Market_System.user_component_DAL.Models;
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
        

        public Bucket(string store_id)
        {
            Random id_generator = new Random();
            this.basket_id = id_generator.Next().ToString();
            this.store_id = store_id;
            this.products = new Dictionary<string, int>();
        }

        public Bucket(Bucket_model bucket_model)
        {
            this.basket_id = bucket_model.basket_id;
            this.store_id = bucket_model.store_id;
            this.products = new Dictionary<string, int>();
            foreach (Product_in_basket_model pibm in bucket_model.products)
            {
                products.Add(pibm.product_id, pibm.quantity);
            }
            
        }

        public Bucket(Bucket_model_history bmh)
        {
            this.basket_id = bmh.basket_id;
            this.store_id = bmh.store_id;
            this.products = convert_list_of_Product_in_basket_history_model_to_dict(bmh.products);

        }

        private Dictionary<string, int> convert_list_of_Product_in_basket_history_model_to_dict(List<Product_in_basket_history_model> products)
        {
            Dictionary<string, int> reutrn_me = new Dictionary<string, int>();
            foreach(Product_in_basket_history_model pibhm in products)
            {
                reutrn_me.Add(pibhm.product_id, pibhm.quantity);
            }
            return reutrn_me;
        }

        public void add_product(string product_id, int quantity)
        {

            if (!check_if_product_exists(product_id))
            {
                this.products.Add(product_id, 0);
            }


            this.products[product_id] = this.products[product_id] + quantity;


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

        internal int remove_product(string product_id, int quantity)
        {
            this.products[product_id] = this.products[product_id] -quantity;
            if(this.products[product_id]<=0)
            {
                this.products.Remove(product_id);
            }
            if(this.products.Count()==0)
            {
                return 1;
            }
            return 0;
        }

        public string get_basket_id()
        {
            return this.basket_id;
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

        internal ItemDTO extract_item(string product_id)
        {
            return new ItemDTO(product_id, products[product_id]);
        }

        internal List<Product_in_basket_model> convert_basket_to_Product_in_basket_model()
        {
            List<Product_in_basket_model> list_of_dtos = new List<Product_in_basket_model>();
            foreach (KeyValuePair<string, int> entry in this.products) // each entry is < product_id , quantity > 
            {
                Product_in_basket_model add_me = new Product_in_basket_model();
                add_me.product_id = entry.Key;
                add_me.quantity = entry.Value;
                add_me.basket_id = this.basket_id;
                list_of_dtos.Add(add_me);
            }
            return list_of_dtos;
        }
    }
}