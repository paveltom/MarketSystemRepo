using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    public class purchase_history_model
    {
        [Key]
        public int ID { get; set; }
        public string username { get; set; }
        public List<Bucket_model_history> baskets { get; set; }
        public string purchase_date { get; set; }
        public double total_price { get; set; }

        public purchase_history_model()
        {

        }

        public purchase_history_model(string username, List<Bucket_model_history> history_baskets,double total_price)
        {
            this.username = username;
            this.baskets = history_baskets;
            this.purchase_date = DateTime.Now.ToShortDateString();
            this.total_price = total_price;
        }


    }
}