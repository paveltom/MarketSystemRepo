using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
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
    }
}