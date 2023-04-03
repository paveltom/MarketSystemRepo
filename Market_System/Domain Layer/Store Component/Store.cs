using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Store : Property
    {
        //Implement all of the Property Methods here
        private string store_ID;
        private string name;
        private ConcurrentDictionary<string, Product> products; //<product_id, Product>
        private Employees employees;
        private String founderID; //founder's userID


        public Store(string store_ID) // builder for existing store - load fields from database
        {
            //TODO:: change it later to load the info from the database BUT!!! lazy load -> add methods that retreive data only on demand
        }


        public Store(string founderID, string newStoreID) // builder for a new store - initialize all fields with default values
        {
            //TODO:: change it later to init default values
        }


        public int GetStore_ID()
        {
            return this.store_ID;
        }

        public string GetFounder()
        {
            return this.founder;
        }

        public void Add_Product(string userID, List<string> productProperties)
        {
            // validate userID first
            products.Add(product_id, quantity);
        }

        public void Remove_Product(int product_id)
        {
            products.Remove(product_id);
        }

        public void Add_New_Owner(string username)
        {
            owners.Add(username);
        }

        public bool Already_Has_Owner(string username)
        {
            if (owners.Contains(username)) return true;
            return false;
        }

        public void Add_New_Manager(string username)
        {
            managers.Add(username);
        }

        public bool Already_Has_Manager(string username)
        {
            if (managers.Contains(username)) return true;
            return false;
        }

        public double CalculatePrice(List<ItemDTO> productsToCalculate)
        {
            throw new NotImplementedException();
        }

        public double Purchase(List<ItemDTO> productsToPurchase)
        {
            throw new NotImplementedException();
        }

        public double ReserveProduct(List<ItemDTO> productsToPurchase)
        {
            throw new NotImplementedException();
        }

        public void EditProduct(string userID, string productID, List<strin> editedDetails) 
        {
            // has to be separated into sub-editions: editWeight(), editQuantity(), etc on higher level
            throw new NotImplementedException();
        }

        public void GetManagersOfTheStore(string userID)
        {
            throw new NotImplementedException();
        }

        public void GetPurchaseHistoryOfTheStore(string userID)
        {
            throw new NotImplementedException();
        }

        




        // ========Methods ToDo==========:
        // passing a data for store representation (including what details?)
        // passing a data for store content representation- what products availble for a buyer
        // *maby should an additional store content view, a seperate view for a manager of the store
        // (search)-get products by name: getting all products with same name *or similar name
        // (search)-get products by catagory: getting all products from that catagory

        // ======more ideas=====
        /*
         *  .חיפוש מוצרים ללא התמקדות בחנות ספציפית, לפי שם המוצר, קטגוריה או מילות מפתח.
         *  ניתן לסנן את התוצאות בהתאם למאפיינים כגון: טווח מחירים, דירוג המוצר, קטגוריה
         * דירוג החנות וכד'.
         */
    }
}