using Market_System.DomainLayer.StoreComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer
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

        public ItemDTO(Product product)
        {
            this._itemId = product.Product_ID;
            this.quantity = product.Quantity;
        }


        public string GetID()
        {
            return this._itemId;
        }
        public int GetQuantity()
        {
            return this.quantity;
        }



    }
}
