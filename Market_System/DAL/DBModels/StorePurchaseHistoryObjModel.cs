using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class StorePurchaseHistoryObjModel
    {
        public string product_id;
        public int quantity;

        public virtual StoreModel Store { get; set; }



        public override string ToString()
        {
            string return_me = "";
            return_me = return_me + "product " + product_id + " quantity: " + quantity + "\n";
            return return_me;
        }
    }
}