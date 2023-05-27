using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Purchase_History_Obj_For_Store
    {
        
        
        private string product_id;
        private int quantity;

        public Purchase_History_Obj_For_Store(ItemDTO item)
        {           
            this.quantity = item.GetQuantity();
            this.product_id = item.GetID();
        }


        public string tostring()
        {
            string return_me = "";
            return_me = return_me + "product " +product_id + " quantity: " + quantity + "\n";       
            return return_me;
        }
    }
}