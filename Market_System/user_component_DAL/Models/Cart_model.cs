using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    public class Cart_model
    {
        [Key]
        public int ID { get; set; }
        public List<Bucket_model> baskets {  get; set;  }
        public double total_price { get; set; }

        public Cart_model()
        {
            this.baskets = new List<Bucket_model>();
            this.total_price = 0;
        }

   
        internal Bucket_model get_basket_model_by_id(string basket_id)
        {
            foreach (Bucket_model bm in baskets)
            {
                if (bm.basket_id.Equals(basket_id))
                {
                    return bm;
                }
            }
            return null;
        }
    }
}