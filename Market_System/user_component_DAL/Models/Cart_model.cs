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


    }
}