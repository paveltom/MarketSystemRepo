using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class StorePurchaseHistoryObjModel
    {
        [Key]
        public string HistoryId { get; set; } // productID_datetime.now
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Day_Month_Year { get; set; }

        public virtual StoreModel Store { get; set; }

        public override string ToString()
        {
            string return_me = "";
            return_me = return_me + "product " + ProductId + " quantity: " + Quantity + "\n";
            return return_me;
        }
    }
}