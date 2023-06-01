using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    public class Bucket_model
    {
        [Key]
        public string basket_id { get; set; }

        public string store_id { get; set; }
        public List<Product_in_basket_model> products { get; set; }

        public Bucket_model()
        {

        }

        public Bucket_model(string basket_id,string store_id)
        {
            this.basket_id = basket_id;
            this.store_id = store_id;
            this.products = new List<Product_in_basket_model>();
        }

        internal Product_in_basket_model get_product_in_bucket_model_by_productid(string product_id)
        {
            foreach (Product_in_basket_model pibm in products)
            {
                if (pibm.product_id.Equals(product_id))
                {
                    return pibm;
                }
            }
            return null;
        }
    }
}