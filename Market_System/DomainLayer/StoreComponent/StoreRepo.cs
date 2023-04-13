using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Market_System.DomainLayer.StoreComponent
{
    public class StoreRepo
    {
        private static Dictionary<Store, Dictionary<Product, int>> storeDatabase; //<Store, <Product, quantity>>

        //This variable is going to store the Singleton Instance
        private static StoreRepo Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static StoreRepo GetInstance()
        {
            //This is thread-Safe - Performing a double-lock check.
            if (Instance == null)
            {
                //As long as one thread locks the resource, no other thread can access the resource
                //As long as one thread enters into the Critical Section, 
                //no other threads are allowed to enter the critical section
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        storeDatabase = new Dictionary<Store, Dictionary<Product, int>>();
                        Instance = new StoreRepo();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        public bool checkIfStoreExists(string founder, int store_ID)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if(pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddStore(string founder, int store_ID)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder))
                {
                    throw new Exception("This store with the provided Store ID already exists for this founder");
                }
            }
            storeDatabase.Add(new Store(founder, store_ID), new Dictionary<Product, int>());
        }

        public void AddProduct(int store_ID, string founder, Product product, int quantity)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder) && !pair.Value.ContainsKey(product))
                {
                    storeDatabase[pair.Key].Add(product, quantity);
                    pair.Key.Add_Product(product.GetProductID(), quantity);
                    return;
                }
            }
            throw new Exception("Invalid data has been provided, either this product already exists in this store.");
        }

        public void RemoveProduct(int store_ID, string founder, Product product)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder) && pair.Value.ContainsKey(product))
                {
                    storeDatabase[pair.Key].Remove(product);
                    pair.Key.Remove_Product(product.GetProductID());
                    return;
                }
            }
            throw new Exception("Invalid data has been provided, either this product doesn't exist in this store.");
        }

        public void Assign_New_Owner(string founder, string username, int store_ID)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder) && !pair.Key.Already_Has_Owner(username))
                {
                    pair.Key.Add_New_Owner(username);
                    return;
                }
            }
            throw new Exception("Invalid data has been provided, either this owner already exists in this store.");
        }

        public void Assign_New_Manager(string founder, string username, int store_ID)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder) && !pair.Key.Already_Has_Manager(username))
                {
                    pair.Key.Add_New_Manager(username);
                    return;
                }
            }
            throw new Exception("Invalid data has been provided, either this manager already exists in this store.");
        }

        public Store getStore(string store_id)
        {
            foreach (Store S in storeDatabase.Keys)
            {
                if (S.Store_ID.Equals(store_id))
                {
                    return S;
                }
            }

            throw new Exception("store does not exists");
        }

        public string getProduct(string product.Name)
        {
            foreach (Product P in products)
            {
                if (P.GetStorename().Equals(product.Name))
                {
                    return P.getProduct();
                }
            }

            throw new Exception("product does not exists");
        }


        public string getNewStoreID(string store.Name)
        {
            if (store.GetStoreID() != null)
            {
                throw new Exception("Store already has an ID");
            }
            string newStoreID = "ID" + (storeDatabase.Count + 1);
            foreach (Store s in storeDatabase)
            {
                if (s.GetStoreID() == newStoreID)
                {
                    throw new Exception("Generated ID is already in use by another store");
                }
            }
            return newStoreID;
        }

        public string getNewProductID(string product.Name)
        {
            if (product.GetProductID() != null)
            {
                throw new Exception("product already has an ID");
            }
            string newProductID = "ID" + (products.Count + 1);
            foreach (Product P in products)
            {
                if (P.GetProductID() == newProductID)
                {
                    throw new Exception("Generated ID is already in use by another product");
                }
            }
            return newProductID;
        }

        public string searchFunctionality(int option, string keyword = "", string productName = "", string category = "")
        {
            string searchResults = "";

            switch (option)
            {
                case 1:
                    searchResults = "Search by keyword for: " + keyword;
                    break;
                case 2:
                    searchResults = "Search by product name for: " + productName;
                    break;
                case 3:
                    searchResults = "Search by category for: " + category;
                    break;
                default:
                    searchResults = "Invalid search option selected.";
                    break;
            }

            return searchResults;

        }

        public void saveStore(Store storeToSave)
        {

        }

        public void saveProduct(Product productToSave)
        {

        }

        public string getPurchaseHistoryOfTheStore(string store_ID)
        {
            return this.getStore(store_ID).get_purchase_history();
        }
            
        public void record_purchase(string store_id,ItemDTO item)
        {
            this.getStore(store_id).record_purchase(item);
        }
    }
}