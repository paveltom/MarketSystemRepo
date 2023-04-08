using Market_System.Domain_Layer.Store_Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer
{
    public class ItemDTO
    {
        private string _itemId;
        private int quantity;

    public ItemDTO(string id,int quantity)
        {
            this._itemId=id;
            this.quantity=quantity;
        }


        public string get_item_id()
        {
            return this._itemId;
        }
        public int get_quantity()
        {
            return this.quantity;
        }



    }
}
