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
        private int reserved_quantity;
        private string name;
        private string description;

        public ItemDTO(string id,int quantity)
        {
            this._itemId=id;
            this.quantity=quantity;
            this.reserved_quantity = 0;
        }

        public ItemDTO(Product product)
        {
            this._itemId = product.Product_ID;
            this.quantity = product.Quantity;
            this.reserved_quantity = product.ReservedQuantity;
        }


        public string GetID()
        {
            return this._itemId;
        }
        public int GetQuantity()
        {
            return this.quantity;
        }

        public string getDescription()
        {
            return this.description;
        }
        public void setDescription(string desc)
        {
            this.description = desc;
        }


        public int GetReservedQuantity()
        {
            return this.reserved_quantity;
        }


        public int SetReservedQuantity(int set_me)
        {
            return this.reserved_quantity = this.reserved_quantity + set_me;
        }

        public string get_name()
        {
            return name;
        }

        public void set_name(string name)
        {
            this.name = name;
        }



    }
}
