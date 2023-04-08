using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Store : Property
    {
        //Implement all of the Property Methods here
        private Dictionary<int, int> products; //<product_id, quantity>
        private List<string> owners; //<Owner's_username>
        private List<string> managers;
        private string founder; //founder's username
        private int store_ID;

        public Store(string founder, int store_ID)
        {
            //TODO:: change it later to load the info from the database.
            products = new Dictionary<int, int>();
            owners = new List<string>();
            managers = new List<string>();
            this.founder = founder;
            this.store_ID = store_ID;
        }

        public int GetStore_ID()
        {
            return this.store_ID;
        }

        public string GetFounder()
        {
            return this.founder;
        }

        public void Add_Product(int product_id, int quantity)
        {
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
    }
}