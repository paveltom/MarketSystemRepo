using Market_System.DAL.DBModels;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.UserComponent;
using Microsoft.Ajax.Utilities;
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

        // init those fields on Instance initialize via DBContext access
        private static List<string> opened_stores_ids;
        private static List<string> temporary_closed_stores_ids;
        private static Random store_id_generator;


        private static StoreRepo Instance = null;
        private static readonly object Instancelock = new object();
        private static readonly object RemoveProductLock = new object();
        
        public static StoreRepo GetInstance()
        {
            if (Instance == null)
            {
                lock (Instancelock)
                {
                    if (Instance == null)
                    {

                        Instance = new Market_System.DAL.StoreRepo();
                        store_id_generator = new Random();
                        using (StoreDataContext context = new StoreDataContext())
                        {
                            opened_stores_ids = new List<string>(); // ---------------------------------------> initiate from DB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            temporary_closed_stores_ids = new List<string>(); // -----------------------------> initiate from DB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            opened_stores_ids = opened_stores_ids.Concat(context.Stores.Where(sm => sm.temporaryClosed == false).Select(x => x.StoreID)).ToList();
                            temporary_closed_stores_ids = temporary_closed_stores_ids.Concat(context.Stores.Where(sm => sm.temporaryClosed == true).Select(x => x.StoreID)).ToList();
                        }
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


        public void destroy()
        {
            Instance = null;

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
                if (context.SaveChanges() > 0)
                    temporary_closed_stores_ids.Remove(storeID);
            }

        }


        public Store getStore(string store_id)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                StoreModel store;
                if ((store = context.Stores.SingleOrDefault(x => x.StoreID == store_id)) != null)
                {
                    //return store.ModelToStore();
                    return ModelToStore(store);

                }
                throw new Exception("store does not exists");

            }
        }

        public Store GetStore(string storeID)
        {
            return getStore(storeID);
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



        public List<Store> GetStores() // return ALL stores - also closed ones
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                //return context.Stores.Select(x => x.ModelToStore()).ToList();
                List<StoreModel> models = context.Stores.ToList();
                return models.Select(x => ModelToStore(x)).ToList();

            }
        }


        public void saveStore(Store storeToSave)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                StoreModel model = context.Stores.SingleOrDefault(x => x.StoreID == storeToSave.Store_ID);
                if (model != null)
                    model.UpdateWholeModel(storeToSave);
                else
                    throw new Exception("storetosave doesn't exists in storeRepo");
                context.SaveChanges();
            }
        }



        public string getPurchaseHistoryOfTheStore(string store_ID)
        {
            if (opened_stores_ids.Contains(store_ID))
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    string ret = "";
                    StoreModel store;
                    if ((store = context.Stores.SingleOrDefault(x => x.StoreID == store_ID)) != null)
                        store.PurchaseHistory.ForEach(x => ret = ret + x.ToString());
                    return ret;
                }
            }
            else if (temporary_closed_stores_ids.Contains(store_ID))
                throw new Exception("can't show a closed store's purhase history!");
            else
                throw new Exception("store does not exist");

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
            if (temporary_closed_stores_ids.Contains(store_ID))
                return;
            using (StoreDataContext context = new StoreDataContext())
            {
                StoreModel store;
                if ((store = context.Stores.SingleOrDefault(x => x.StoreID == store_ID)) != null)
                {
                    store.temporaryClosed = true;
                    context.SaveChanges();
                    opened_stores_ids.Remove(store_ID);
                    temporary_closed_stores_ids.Add(store_ID);
                }                    
            }
        }


        public void record_purchase(Store store, ItemDTO item)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                StoreModel model = context.Stores.SingleOrDefault(x => x.StoreID == store.Store_ID);
                if (model != null) 
                {
                    StorePurchaseHistoryObjModel purchase = new StorePurchaseHistoryObjModel();
                    purchase.Quantity = item.GetQuantity();
                    purchase.ProductId = item.GetID();
                    purchase.Store = model;
                    purchase.TotalPrice = item.Price * item.GetQuantity();
                    purchase.Day_Month_Year = DateTime.Now.Day.ToString("dd") + "_" + DateTime.Now.Month.ToString("MM") + "_" + DateTime.Now.Year.ToString("yyyy");
                    model.PurchaseHistory.Add(purchase);
                    context.SaveChanges();
                }
            }
        }


        public double GetStoreProfitForDate(string storeID, string date_as_dd_MM_yyyy)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                StoreModel model = context.Stores.SingleOrDefault(x => x.StoreID == storeID);
                if (model != null)
                {
                    return model.PurchaseHistory.Where(h => h.Day_Month_Year == date_as_dd_MM_yyyy).ToList().Aggregate(0.0, (acc, x) => acc += x.TotalPrice, acc => acc);
                }
                throw new Exception("No such store.");
            }
        }




        public Store ModelToStore(StoreModel model)
        {
            List<Purchase_Policy> policies = new List<Purchase_Policy>();
            List<Purchase_Policy> defaultPolicies = new List<Purchase_Policy>();
            model.Policies.ForEach(x =>
            {
                if (x.isDefault)
                    defaultPolicies.Add(x.ModelToPolicy());
                else
                    policies.Add(x.ModelToPolicy());
            });

            List<Purchase_Strategy> strategies = new List<Purchase_Strategy>();
            List<Purchase_Strategy> defaultStrategies = new List<Purchase_Strategy>();
            model.Strategies.ForEach(x =>
            {
                if (x.isDefault)
                    defaultStrategies.Add(new Purchase_Strategy(x.StrategyID, x.StrategyName, x.Description, x.StrategyFormula));
                else
                    strategies.Add(new Purchase_Strategy(x.StrategyID, x.StrategyName, x.Description, x.StrategyFormula));
            });
            List<string> productIds = new List<string>();
            if (model.Products != null)
                productIds = model.Products.Select(x => x.ProductID).ToList();
            Store ret = new Store(model.founderID, model.StoreID, policies, strategies, productIds, model.temporaryClosed);

            ret.productDefaultPolicies = new ConcurrentDictionary<string, Purchase_Policy>(defaultPolicies.ToDictionary(keySelector: x => x.PolicyID, elementSelector: x => x));
            ret.productDefaultStrategies = new ConcurrentDictionary<string, Purchase_Strategy>(defaultStrategies.ToDictionary(keySelector: x => x.StrategyID, elementSelector: x => x)); ;

            return ret;

        }





        // =================== END of Store Methods =================== 
        // ============================================================












        // =======================================================
        // =================== Product Methods =================== 

        public Product getProduct(string product_id)
        {
            string store_id = GetStoreIdFromProductID(product_id);
            using (StoreDataContext context = new StoreDataContext())
            {
                if (context.Stores.SingleOrDefault(s => s.StoreID == store_id) == null)
                    throw new Exception("store does not exists");
                ProductModel pm;
                if ((pm = context.Products.SingleOrDefault(p => p.ProductID == product_id)) != null)
                    return pm.ModelToProduct();
                else
                    throw new Exception("product does not exists");
            }
        }



        public List<ItemDTO> SearchProductsByCategory(Category category)
        {
            lock (RemoveProductLock)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    return context.Products.Where(x => x.ProductCategory == category.CategoryName).Select(pm => pm.ModelToProduct()).Select(p => p.GetProductDTO()).ToList();
                }
            }
        }



        internal string get_product_name_from_prodcut_id_and_store(string product_id, Store s)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                ProductModel pm;
                if ((pm = context.Products.SingleOrDefault(p => p.ProductID == product_id)) != null)
                    return pm.Name;
            }
            return null;
        }



        public List<ItemDTO> SearchProductsByKeyword(string keyword)
        {
            lock (RemoveProductLock)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    return context.Products.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.ProductCategory.Contains(keyword)).Select(pm => pm.ModelToProduct()).Select(p => p.GetProductDTO()).ToList();
                }
            }
        }




        public List<ItemDTO> SearchProductsByName(string name)
        {
            lock (RemoveProductLock)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    return context.Products.Where(x => x.Name.Contains(name)).Select(pm => pm.ModelToProduct()).Select(p => p.GetProductDTO()).ToList();
                }
            }
        }



        public void AddProduct(string store_ID, string founder_username, Product product, int quantity)
        {
            if (opened_stores_ids.Contains(store_ID))
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    StoreModel sm;
                    ProductModel pm;
                    if((sm = context.Stores.SingleOrDefault(s => s.StoreID == store_ID)) == null || (sm.founderID != founder_username) || (pm = context.Products.SingleOrDefault(p => p.ProductID == product.Product_ID)) != null)
                        throw new Exception("Invalid data has been provided, either this product already exists in this store.");
                    pm = new ProductModel();
                    pm.ProductID = product.Product_ID;
                    pm.StoreID = store_ID;
                    pm.UpdateWholeModel(product);
                    pm.Store = sm;                    
                    context.Products.Add(pm);
                    context.SaveChanges();
                    //sm.Products.Add(pm);
                    //context.SaveChanges();
                }
            }
            else if (temporary_closed_stores_ids.Contains(store_ID))
                throw new Exception("can't add product to closed store!");
            else
                throw new Exception("store does not exist");

        }       



        public void RemoveProduct(string store_ID, string founder, Product product)
        {
            lock (RemoveProductLock)
            {
                if (opened_stores_ids.Contains(store_ID))
                {
                    using (StoreDataContext context = new StoreDataContext())
                    {
                        StoreModel sm;
                        ProductModel pm;
                        if ((sm = context.Stores.SingleOrDefault(s => s.StoreID == store_ID)) == null || (sm.founderID != founder) || (pm = context.Products.SingleOrDefault(p => p.ProductID == product.Product_ID)) == null)
                            throw new Exception("Invalid data has been provided, either this product already exists in this store.");
                        sm.Products.Remove(pm);
                        context.SaveChanges();
                    }
                }
                else if (temporary_closed_stores_ids.Contains(store_ID))
                    throw new Exception("can't remove product to closed store!");
                else
                    throw new Exception("store does not exist");
            }
        }




        public string getNewProductID(string storeID)
        {
            string newProductID;
            using (StoreDataContext context = new StoreDataContext())
            {
                while (true)
                {
                    newProductID = storeID + "_" + store_id_generator.Next().ToString();
                    if (context.Products.Any(x => x.ProductID == newProductID))
                        continue;
                    return newProductID;
                }
            }
        }


        public Product GetProduct(string productID)
        {
            return getProduct(productID);
            /*using (StoreDataContext context = new StoreDataContext())
            {
                ProductModel pm;
                if ((pm = context.Products.SingleOrDefault(p => p.ProductID == productID)) != null)
                    return pm.ModelToProduct();
            }
            throw new Exception("No such product in this store with the provided ID");*/
        }




        public void saveProduct(Product productToSave)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                ProductModel pm;
                if ((pm = context.Products.SingleOrDefault(pm => pm.ProductID == productToSave.Product_ID)) != null)
                    pm.UpdateWholeModel(productToSave);
                context.SaveChanges();
            }


        }


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


















    }
}