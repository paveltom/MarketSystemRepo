using Market_System.DomainLayer.StoreComponent;
using System;
using System.Collections.Concurrent;
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


        public double Price { get; private set; }
        public string StoreID { get; private set; }
        public String Name { get; private set; }
        public String Description { get; private set; }
        public Double Rating { get; private set; } // between 1-10
        public Double Weight { get; private set; }
        public long timesBought { get; private set; }
        public long timesRated { get; private set; }
        public Category ProductCategory { get; private set; }   // (mabye will be implementing by composition design pattern to support a sub catagoring.)
        public Double[] Dimenssions { get; private set; } // array of 3
        public ConcurrentDictionary<string, Purchase_Policy> PurchasePolicies { get; private set; } // make it threadsafe ChaiinOfResponsobolities 
        public ConcurrentDictionary<string, Purchase_Strategy> PurchaseStrategies { get; private set; } // make it threadsafe ChainOfResponsibilities
        public ConcurrentBag<string> Comments { get; private set; }
        public ConcurrentDictionary<String, List<String>> PurchaseAttributes { get; private set; }



        public ItemDTO(string id,int quantity)
        {
            this._itemId=id;
            this.quantity=quantity;
            this.reserved_quantity = 0;
            this.Price = 0;
        }

        public ItemDTO(Product product)
        {
            this._itemId = product.Product_ID;
            this.quantity = product.Quantity;
            this.reserved_quantity = product.ReservedQuantity;
            this.Price = product.Price;

            this.StoreID = product.StoreID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Rating = product.Rating;
            this.Weight = product.Weight;
            this.timesBought = product.timesBought;
            this.timesRated = product.timesRated;
            this.ProductCategory = product.ProductCategory;
            this.Dimenssions = product.Dimenssions;
            this.PurchasePolicies = product.PurchasePolicies;
            this.PurchaseStrategies = product.PurchaseStrategies;
            this.PurchaseAttributes = product.PurchaseAttributes;
        }


        public string GetID()
        {
            return this._itemId;
        }
        public int GetQuantity()
        {
            return this.quantity;
        }

        public int GetReservedQuantity()
        {
            return this.reserved_quantity;
        }

        public void SetPrice(double price)
        {
            this.Price = price;
        }


        public int SetReservedQuantity(int set_me)
        {
            return this.reserved_quantity = this.reserved_quantity + set_me;
        }

        public Dictionary<string, string> GetStringValuesDict()
        {
            Dictionary<string, string> FieldValue = new Dictionary<string, string>();
            FieldValue.Add("ItemID", this._itemId);
            FieldValue.Add("ItemID", this.quantity.ToString());
            FieldValue.Add("ItemID", this.reserved_quantity.ToString());
            FieldValue.Add("ItemID", this.Price.ToString());
            FieldValue.Add("ItemID", this.Name);
            FieldValue.Add("ItemID", this.Description);
            FieldValue.Add("ItemID", this.Rating.ToString());
            FieldValue.Add("ItemID", this.Weight.ToString());
            FieldValue.Add("ItemID", this.timesBought.ToString());
            FieldValue.Add("ItemID", this.ProductCategory.CategoryName);
            FieldValue.Add("ItemID", this.PurchaseAttributes.ToString());
            return FieldValue;
        }


    }
}
