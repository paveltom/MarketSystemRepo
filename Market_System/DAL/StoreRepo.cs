using Market_System.DAL.DBModels;
using Market_System.DomainLayer.StoreComponent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml.Linq;

namespace Market_System.DAL
{
    public class StoreRepo
    {
        // private static Dictionary<Store, Dictionary<Product, int>> storeDatabase; //<Store, <Product, quantity>>
        // private static Dictionary<Store, Dictionary<string, List<Purchase_History_Obj_For_Store>>> purchase_history; // key in second dictionary is date  val



        /*
        public void UseProducts()
        {
            using (var context = new ProductContext())
            {     
                // Perform data access using the context
            }
        }
         */


        // init those fields on Instance initialize via DBContext access
        private static List<string> opened_stores_ids; 
        private static List<string> temporary_closed_stores_ids;
        private static Random store_id_generator;
        
        
        private static StoreRepo Instance = null;
        private static readonly object Instancelock = new object();
        public static StoreRepo GetInstance()
        {
            if (Instance == null)
            {
                lock (Instancelock)
                { 
                    if (Instance == null)
                    {
                        Instance = new Market_System.DAL.StoreRepo();
                        // opened_stores_ids = new List<string>(); ---> initiate from DB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        // temporary_closed_stores_ids = new List<string>(); ---> initiate from DB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                } 
            }
            return Instance;
        }



        public void RemoveDataBase(string password)
        {
            string path = System.Environment.CurrentDirectory + "\\MarketSystemDB.db";
            if (password == "qwe123" && File.Exists(path))
                lock (this)
                {
                    File.Delete(path);
                }

        }





        // =====================================================
        // =================== Store Methods =================== 


        public void AddStore(string userID, Store currStore)
        {
            foreach (string s in opened_stores_ids.Concat(temporary_closed_stores_ids))
            {
                if (s == currStore.Store_ID)
                {
                    throw new Exception("This store with the provided Store ID already exists.");
                }
            }

            using (StoreDataContext context = new StoreDataContext())
            {
                StoreModel newStore = new StoreModel()
                {
                    StoreID = currStore.Store_ID,

                    Name = currStore.Name,

                    founderID = currStore.founderID,

                    temporaryClosed = false,
                };

                context.Stores.Add(newStore);
                context.SaveChanges();

            }


            opened_stores_ids.Add(currStore.Store_ID);
        }



        public bool checkIfStoreExists(string founder, string store_ID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                return context.Stores.Any(x => x.StoreID == store_ID && x.temporaryClosed == false);
            }
        }


        internal void re_open_closed_temporary_store(string userID, string storeID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                context.Stores.SingleOrDefault(x => x.StoreID == storeID).temporaryClosed = false;
                if(context.SaveChanges() > 0)
                    temporary_closed_stores_ids.Remove(storeID);
            }

        }


        public Store getStore(string store_id)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }



        public string getNewStoreID()
        {

            string newStoreID;
            using (StoreDataContext context = new StoreDataContext())
            {
                while (true)
                {
                    newStoreID = store_id_generator.Next().ToString();
                    if (context.Stores.Any(x => x.StoreID == newStoreID))
                        continue;
                    return newStoreID;
                }                
            }
        }









        // =================== END of Store Methods =================== 
        // ============================================================












        // =======================================================
        // =================== Product Methods =================== 



        // =================== END of Product Methods =================== 
        // ==============================================================
















        // ============================================================================================================================
        // ============================================================================================================================
        // ============================================================================================================================
        // ============================================================================================================================
        // ============================================================================================================================
        // ============================================================================================================================
        // ============================================================================================================================




        // ============================================================                           TODO                             ============================================================




        /*

        public Product getProduct(string product_id)
        {
           string store_id= GetStoreIdFromProductID(product_id);
            foreach (KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase)
            {

                if (pair.Key.Store_ID.Equals(store_id) )
                {
                    foreach(Product p in pair.Value.Keys)
                    {
                        if(p.Product_ID.Equals(product_id))
                        {
                            return p;
                        }
                    }
                    throw new Exception("product does not exists");
                }

            }
            throw new Exception("store does not exists");


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

  

        internal string get_product_name_from_prodcut_id_and_store(string product_id, Store s)
        {
           foreach(KeyValuePair<Product, int> pair in storeDatabase[s])
            {
                if(pair.Key.Product_ID.Equals(product_id))
                {
                    return pair.Key.Name;
                }
            }
            return null;
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
            Dictionary<Product, int> save_me = null;
            

            foreach (Store s in storeDatabase.Keys)
            {
                if (s.Store_ID.Equals(storeToSave.Store_ID))
                {

                    save_me = storeDatabase[s];
                    storeDatabase.Remove(s);
                    
                    break;
                }
            }
            if (save_me != null)
            {
                storeDatabase.Add(storeToSave, save_me);
                return;
            }
            throw new Exception("storetosave doesn't exists in storeRepo");
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
            storeDatabase[store].Add(productToSave, productToSave.Quantity-productToSave.ReservedQuantity);
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



        public void destroy()
        {
            Instance = null;

        }




        public List<Store> GetStores()
        {
            List<Store> stores = new List<Store>();
            foreach(KeyValuePair<Store, Dictionary<Product, int>> pair in storeDatabase){
                stores.Add(pair.Key);
            }
            return stores;
        }
        */

















    }
}