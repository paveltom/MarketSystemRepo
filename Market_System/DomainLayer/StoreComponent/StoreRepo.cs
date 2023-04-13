﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Market_System.DomainLayer.StoreComponent
{
    public class StoreRepo
    {
        private static List<string> opened_stores_ids;
        private static List<string> temporary_closed_stores_ids;
        private static Dictionary<Store, Dictionary<Product, int>> storeDatabase; //<Store, <Product, quantity>>
        private static Random store_id_generator;
        private static Dictionary<Store, Dictionary<string, List<Purchase_History_Obj_For_Store>>> purchase_history; // key in second dictionary is date  val
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
                        store_id_generator = new Random();
                        purchase_history = new Dictionary<Store, Dictionary<string, List<Purchase_History_Obj_For_Store>>>();
                        Instance = new StoreRepo();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }




        public void record_purchase(Store store,ItemDTO item)
        {
            if(!purchase_history.ContainsKey(store))
            {
                purchase_history.Add(store,new Dictionary<string, List<Purchase_History_Obj_For_Store>>());
            }

            if (!purchase_history[store].ContainsKey(DateTime.Now.ToShortDateString()))
            {
                purchase_history[store].Add(DateTime.Now.ToShortDateString(), new List<Purchase_History_Obj_For_Store>());
            }

            purchase_history[store][DateTime.Now.ToShortDateString()].Add(new Purchase_History_Obj_For_Store(item));

        }



        public List<ItemDTO> SearchProductsByCategory(Category category)
        {
            List<ItemDTO> search_result = new List<ItemDTO>();

            lock (RemoveProductLock)
            {
                foreach (Store s in storeDatabase.Keys)
                {
                    if (!s.is_closed_temporary())
                    {
                        foreach (KeyValuePair<Product, int> pair in storeDatabase[s])
                        {

                            if (pair.Key.get_ProductCategory().CategoryName.Equals(category.CategoryName))
                            {
                                search_result.Add(new ItemDTO(pair.Key));
                            }
                        }
                    }
                }


                return search_result;
            }
          
        }


        public List<ItemDTO> SearchProductsByKeyword(string keyword)
        {
            List<ItemDTO> search_result = new List<ItemDTO>();

            lock (RemoveProductLock)
            {
                foreach (Store s in storeDatabase.Keys)
                {
                    if (!s.is_closed_temporary())
                    {
                        foreach (KeyValuePair<Product, int> pair in storeDatabase[s])
                        {

                            if (pair.Key.Name.Contains(keyword))
                            {
                                search_result.Add(new ItemDTO(pair.Key));
                            }
                        }
                    }

                }


                return search_result;
            }

        }




        public List<ItemDTO> SearchProductsByName(string name)
        {
            List<ItemDTO> search_result = new List<ItemDTO>();

            lock (RemoveProductLock)
            {
                foreach (Store s in storeDatabase.Keys)
                {
                    if (!s.is_closed_temporary())
                    {
                        foreach (KeyValuePair<Product, int> pair in storeDatabase[s])
                        {

                            if (pair.Key.Name.Equals(name))
                            {
                                search_result.Add(new ItemDTO(pair.Key));
                            }
                        }
                    }

                }


                return search_result;
            }

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

        public void AddStore(string founder, string store_ID)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder))
                {
                    throw new Exception("This store with the provided Store ID already exists for this founder");
                }
            }
            storeDatabase.Add(new Store(founder, store_ID), new Dictionary<Product, int>());
            opened_stores_ids.Add(store_ID);
        }
      
        public void AddProduct(string store_ID, string founder_username, Product product, int quantity)
        {
            if (opened_stores_ids.Contains(store_ID))
            {
                foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
                {
                    if (pair.Key.GetStore_ID().Equals(store_ID) && pair.Key.GetFounder().Equals(founder) && !pair.Value.ContainsKey(product))
                    {
                        storeDatabase[pair.Key].Add(product, quantity);
                        //  pair.Key.AddProduct(new Product product.get_productid(), quantity);
                        // pair.Key.AddProduct(founder_username,product.) fix this later
                        return;
                    }
                }
                throw new Exception("Invalid data has been provided, either this product already exists in this store.");
            }
            else
            {
                if (temporary_closed_stores_ids.Contains(store_ID))
                {
                    throw new Exception("can't add product to closed store!");
                }
                else
                {
                    throw new Exception("store does not exist");
                }
            }
            
        }

        private static object RemoveProductLock = new object();
        public void RemoveProduct(string store_ID, string founder, Product product)
        {
            lock (RemoveProductLock)
            {
                if (opened_stores_ids.Contains(store_ID))
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
                else
                {
                    if (temporary_closed_stores_ids.Contains(store_ID))
                    {
                        throw new Exception("can't remove product from a closed store!");
                    }
                    else
                    {
                        throw new Exception("store does not exist");
                    }
                }
            }
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


        public string getNewStoreID()
        {
            
            bool found_same_id = false;
            string newStoreID;
            while (true)
            {
                newStoreID = store_id_generator.Next().ToString();
                foreach (Store s in storeDatabase.Keys)
                {
                    if (s.GetStore_ID().Equals( newStoreID))
                    {
                        found_same_id = true;
                        break;

                    }
                }
                if(!found_same_id)
                {
                    break;
                }
            }
            return newStoreID;
        }

        public string getNewProductID(string storeID)
        {
            bool found_same_id = false;
            Store store = getStore(storeID);
            
            string newProductID = storeID +"_";
            while (true)
            {
                newProductID = newProductID+store_id_generator.Next().ToString();
                foreach (Product p in storeDatabase[store].Keys)
                {
                    if (p.get_productid().Equals(newProductID))
                    {
                        found_same_id = true;
                        newProductID = storeID + "_";
                        break;


                    }
                }
                if (!found_same_id)
                {
                    break;
                }
            }
            
      
            return newProductID;
        }




        public void saveStore(Store storeToSave)
        {

        }

        public void saveProduct(Product productToSave)
        {

        }

        public string getPurchaseHistoryOfTheStore(string store_ID)
        {
            if (opened_stores_ids.Contains(store_ID))
            {
                string return_me = "";
                Store s = getStore(store_ID);
                foreach (KeyValuePair<string, List<Purchase_History_Obj_For_Store>> purchase__pair in purchase_history[s])
                {
                    return_me = return_me + purchase__pair.Key + ": \n";
                    foreach (Purchase_History_Obj_For_Store obj in purchase__pair.Value)
                    {
                        return_me = return_me + obj.tostring();

                    }

                }
                return return_me;
            }
            else
            {
                if (temporary_closed_stores_ids.Contains(store_ID))
                {
                    throw new Exception("can't show a closed store's purhase history!");
                }
                else
                {
                    throw new Exception("store does not exist");
                }
            }
        }


        



        internal void close_store_temporary(string store_ID)
        {
            opened_stores_ids.Remove(store_ID);
            temporary_closed_stores_ids.Add(store_ID);
        }

        internal void destroy()
        {
            Instance = null;

        }
    }
}