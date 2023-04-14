using System;
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

                            if (pair.Key.ProductCategory.CategoryName.Equals(category.CategoryName))
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
               
                if(pair.Key.Store_ID.Equals(store_ID) && pair.Key.founderID.Equals(founder))
                {
                    return true;
                }
            }
            return false;
        }

        /*
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
        */
             
        public void AddProduct(string store_ID, string founder_username, Product product, int quantity)
        {
            if (opened_stores_ids.Contains(store_ID))
            {
                foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
                {
                    if (pair.Key.Store_ID.Equals(store_ID) && pair.Key.founderID.Equals(founder_username) && !pair.Value.ContainsKey(product))
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
                        if (pair.Key.Store_ID.Equals(store_ID) && pair.Key.founderID.Equals(founder) && pair.Value.ContainsKey(product))
                        {
                            pair.Value.Remove(product);
                            
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

        internal void AddStore(string userID, Store currStore)
        {
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {
                if (pair.Key.Store_ID.Equals(currStore.Store_ID) && pair.Key.founderID.Equals(userID))
                {
                    throw new Exception("This store with the provided Store ID already exists for this founder");
                }
            }
            storeDatabase.Add(currStore, new Dictionary<Product, int>());
            opened_stores_ids.Add(currStore.Store_ID);
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

   


        public string getNewStoreID()
        {
            
            bool found_same_id = false;
            string newStoreID;
            while (true)
            {
                newStoreID = store_id_generator.Next().ToString();
                foreach (Store s in storeDatabase.Keys)
                {
                    if (s.Store_ID.Equals( newStoreID))
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
                    if (p.Product_ID.Equals(newProductID))
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

        public Store GetStore(string storeID)
        {
            foreach (Store s in storeDatabase.Keys)
            {
                if (s.Store_ID.Equals(storeID))
                {
                    return s;
                }
            }

            throw new Exception("No such store with the provided Store ID");
        }

        public Product GetProduct(string productID)
        {
            var storeID = GetStoreIdFromProductID(productID);
            Store store = GetStore(storeID);
            foreach (Product p in storeDatabase[store].Keys)
            {
                if (p.Product_ID.Equals(productID))
                {
                    return p;
                }
            }
            throw new Exception("No such product in this store with the provided ID");
        }


        public void saveStore(Store storeToSave)
        {
            foreach (Store s in storeDatabase.Keys)
            {
                if (s.Store_ID.Equals(storeToSave.Store_ID))
                {
                    storeDatabase.Remove(s);
                    break;
                }
            }
            storeDatabase.Add(storeToSave, storeToSave.GetListOfProducts());

        }

        public void saveProduct(Product productToSave)
        {
            var storeID = productToSave.GetStoreID();
            Store store = GetStore(storeID);
            foreach (Product p in storeDatabase[store].Keys)
            {
                var productID = productToSave.Product_ID;
                if (p.Product_ID.Equals(productID))
                {
                    storeDatabase[store].Remove(p);
                    break;
                }
            }
            storeDatabase[store].Add(productToSave, productToSave.Quantity);
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

        private string GetStoreIdFromProductID(string productID)
        {
            try
            {
                return productID.Substring(0, productID.IndexOf("_"));
            }
            catch (Exception e)
            {
                throw e;
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