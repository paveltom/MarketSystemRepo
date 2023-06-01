using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market_System.user_component_DAL.Models
{
    public class Bucket_model_history
    {


        [Key]
        public string basket_id { get; set; }

        public string store_id { get; set; }
        public List<Product_in_basket_history_model> products { get; set; }

        public Bucket_model_history()
        {

        }

        public Bucket_model_history(string basket_id, string store_id)
        {
            this.basket_id = basket_id;
            this.store_id = store_id;
            this.products = new List<Product_in_basket_history_model>();
        }
    }
}