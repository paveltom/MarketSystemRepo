using Market_System.DAL.DBModels;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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
                            opened_stores_ids = context.Stores.Where(s => s.temporaryClosed == false).Select(s => s.StoreID).ToList(); // ---------------------------------------> initiate from DB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            temporary_closed_stores_ids = context.Stores.Where(s => s.temporaryClosed == true).Select(s => s.StoreID).ToList();  // -----------------------------> initiate from DB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
            //string path = System.Environment.CurrentDirectory + "\\MarketDB.db";
            if (password == "qwe123")
                lock (this)
                {
                    //File.Delete(path);
                    //DbConnection conn = Database.DefaultConnectionFactory.CreateConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True");
                    //conn.Open();
                    //conn.Database.Remove(0);
                    //conn.Close();
                    using (StoreDataContext context = new StoreDataContext())
                    {
                        context.Database.Delete();
                    }
                    // Database.Delete("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True");
                    //var sc = new Microsoft.SqlServer.Management.Common.ServerConnection(your localDB SqlConnection here);
                    //var server = new Microsoft.SqlServer.Management.Smo.Server(sc);
                    //server.KillDatabase(dbName here);
                }

        }


        public void destroy()
        {
            Instance = null;
        }


        public ConcurrentDictionary<string, TimerPlus> RestoreTimers()
        {
            using (StoreDataContext context = new StoreDataContext())
            {                
                ConcurrentDictionary<string, TimerPlus> ret = new ConcurrentDictionary<string, TimerPlus>();
                context.TimersDB.ToList().ForEach(t =>
                {

                    DateTime currDate = DateTime.Now;
                    DateTime creationDate = DateTime.Parse(t.CreationTimeStamp);
                    TimeSpan ts = currDate - creationDate;
                    double remainedTime = TimeSpan.FromMinutes(Double.Parse(t.MinutesToCount)).TotalMilliseconds - ts.TotalMilliseconds;
                    if (remainedTime < 0)
                        remainedTime = 1;
                    TimerPlus newT = new TimerPlus(remainedTime, creationDate);

                    Action<object, System.Timers.ElapsedEventArgs, string, string> methodWithTimerNeeded;
                    if (t.TimerID.Contains("lottery"))
                    {
                        methodWithTimerNeeded = StoreFacade.GetInstance().LotteryTimerHandler;
                        newT.Elapsed += (sender, e) => methodWithTimerNeeded(sender, e, t.FounderID, t.ProductID);
                    }
                    else if (t.TimerID.Contains("auction"))
                    {
                        methodWithTimerNeeded = StoreFacade.GetInstance().AuctionTimerHandler;
                        newT.Elapsed += (sender, e) => methodWithTimerNeeded(sender, e, t.FounderID, t.ProductID);
                    }                  
                    
                });
                return ret;
            }
        }


        public void AddTimer(System.Timers.Timer newTimer, string timerID, string founderID, string productID, DateTime creationTime, long minutesToCount)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                TimerModel model = new TimerModel();
                model.FounderID = founderID;
                model.ProductID = productID;
                model.TimerID = timerID;
                model.CreationTimeStamp = creationTime.ToString();
                model.MinutesToCount = minutesToCount.ToString();
                context.TimersDB.Add(model);
                context.SaveChanges();
            }
        }




        public void AddPayment(string userID, string transactionID, double price, bool canceled)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                TransactionModel model;
                if (canceled && (model = context.Transactions.SingleOrDefault(t => t.TransactioID == transactionID)) != null)
                {
                    model.Cancelled = true;
                }
                else
                {
                    model = new TransactionModel();
                    model.TransactioID = transactionID;
                    model.Price = price;
                    model.UserID = userID;
                    model.Cancelled = false;
                }
                context.SaveChanges();
            }
        }



        public List<Purchase_History_Obj_For_Store> GetPurchaseHistoryOfTheUser(string userID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                List<StorePurchaseHistoryObjModel> models = context.Purchases.Where(p => p.UserID == userID).ToList();
                return models.Select(m => m).ToList().Select(m => 
                new Purchase_History_Obj_For_Store(m.Quantity, m.ProductId, new DateTime(long.Parse(m.HistoryId.Substring(m.HistoryId.LastIndexOf('_') + 1))), m.TotalPrice)).ToList();
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
                        store.PurchaseHistory.ForEach(x => ret = ret + x.ToString() + "\n");
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


        public void record_purchase(Store store, ItemDTO item, string buyerID)
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
                    purchase.UserID = buyerID;
                    DateTime dateTime = DateTime.Now;
                    string month = dateTime.Month.ToString();
                    if (month.Length == 1)
                        month = "0" + month;
                    string date_as_dd_MM_yyyy = dateTime.Day.ToString() + "_" + month + "_" + dateTime.Year.ToString();
                    purchase.Day_Month_Year = date_as_dd_MM_yyyy;
                    purchase.HistoryId = item.GetID() + "_" + DateTime.Now.Ticks;
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
            ret.ChangeName(ret.founderID, model.Name);

            ret.productDefaultPolicies = new ConcurrentDictionary<string, Purchase_Policy>(defaultPolicies.ToDictionary(keySelector: x => x.PolicyID, elementSelector: x => x));
            ret.productDefaultStrategies = new ConcurrentDictionary<string, Purchase_Strategy>(defaultStrategies.ToDictionary(keySelector: x => x.StrategyID, elementSelector: x => x)); ;

            return ret;

        }




        public BidDTO PlaceBid(string storeID, string userID, string productID, double newPrice, int quantity, string card_number, string month, string year, string holder, string ccv, string id)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                // validate enough quantity to reserve
                StoreModel store;
                if (((store = context.Stores.SingleOrDefault(s => s.StoreID == storeID)) == null) || (context.Products.SingleOrDefault(p => p.ProductID == productID && (p.Quantity - p.ReservedQuantity >= quantity)) == null))
                    throw new Exception("Cannot add bid.");
                string currBID = userID + "_" + productID + "_bid";
                BidModel bid;
                if((bid = context.Bids.SingleOrDefault(b => b.BidID == currBID)) == null)
                {
                    bid = new BidModel();
                    bid.ProductID = productID;
                    bid.NewPrice = newPrice;
                    bid.UserID = userID;
                    bid.BidID = currBID;
                    bid.ApprovedByUser = false;
                    bid.ApprovedByStore = false;
                    bid.CounterOffer = false;
                    bid.DeclinedByStore = false;
                    bid.DeclinedByUser = false;
                    bid.NumOfApproves = 0;
                    bid.Quantity = quantity;
                    bid.PaymentDetails = card_number + "_" + month + "_" + year + "_" + holder + "_" + ccv + "_" + id;
                    store.Bids.Add(bid);

                    context.Bids.Add(bid);
                    context.SaveChanges();
                }

                return bid.ModelToBid();
            }
        }

        public bool ApproveBid(string storeID, string userID, string bidID)
        {
            bool ret = false;
            using (StoreDataContext context = new StoreDataContext())
            {                
                BidModel bid;
                if((bid = context.Bids.SingleOrDefault(b => b.BidID == bidID)) != null)
                {
                    if (bid.UserID == userID)
                    {
                        bid.ApprovedByUser = true;
                        ret = true;
                    }
                    else
                        bid.NumOfApproves++;
                    int neededApproves = context.Employees.Where(e => e.StoreID == storeID).ToList().Where(e => e.Role == "Owner" || e.Permissions.Contains("STOCK")).Count();
                    if (bid.NumOfApproves == neededApproves)
                    {
                        bid.ApprovedByStore = true;
                        ret = true;
                    }
                    context.SaveChanges();
                }
            }
            return ret;
        }


        public BidDTO GetBid(string bidID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                BidModel bid;
                if ((bid = context.Bids.SingleOrDefault(b => b.BidID == bidID)) != null)
                    return bid.ModelToBid();
                throw new Exception("No such bid.");
            }
        }



        public void CounterBid(string bidID, double counterPrice)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                BidModel bid;
                if ((bid = context.Bids.SingleOrDefault(b => b.BidID == bidID)) != null)
                {
                    bid.NewPrice = counterPrice;
                    bid.ApprovedByStore = true;
                    bid.CounterOffer = true;
                    context.SaveChanges();
                }
            }
        }


        public void RemoveBid(string bidID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                BidModel bid;
                if ((bid = context.Bids.SingleOrDefault(b => b.BidID == bidID)) != null)
                {
                    context.Bids.Remove(bid);
                    context.SaveChanges();
                }
            }
        }


        public List<BidDTO> GetStoreBids(string storeID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                return context.Bids.Where(b => b.Store.StoreID == storeID).ToList().Select(b => b.ModelToBid()).ToList();
            }
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
                ProductModel pm;
                if (context.Stores.SingleOrDefault(s => s.StoreID == store_id) == null)
                    throw new Exception("store does not exists");
                
                if ((pm = context.Products.SingleOrDefault(p => p.ProductID == product_id)) == null)                 
                    throw new Exception("product does not exists");

                return ModelToProduct(pm);
            }
            
        }


        public Product ModelToProduct(ProductModel model)
        {
            double[] dimenssions = model.Dimenssions.Split('_').Select(s => double.Parse(s)).ToArray();
            List<string> comments = model.Comments.Select(c => c.Comment).ToList();
            ConcurrentDictionary<string, Purchase_Policy> policies = new ConcurrentDictionary<string, Purchase_Policy>(model.Policies.Select(p => p.ModelToPolicy()).ToDictionary(keySelector: x => x.PolicyID, elementSelector: x => x));
            ConcurrentDictionary<string, Purchase_Strategy> strategies = new ConcurrentDictionary<string, Purchase_Strategy>(model.Strategies.Select(p => p.ModelToPolicy()).ToDictionary(keySelector: x => x.StrategyID, elementSelector: x => x));
            Dictionary<string, List<string>> attributes = new Dictionary<string, List<string>>(model.ProductPurchaseAttributes.ToDictionary(keySelector: a => a.AttributeName, elementSelector: a => a.AttributeOptions.Split('_').ToList()));
            Category category = new Category(model.ProductCategory);


            List<string> auctionDetails = model.Auction.Split('_').ToList();
            string auctionKey = auctionDetails[0];
            string auctionValue = auctionDetails[1];
            string transID = auctionDetails[2];
            KeyValuePair<string, List<string>> auction = new KeyValuePair<string, List<string>>(auctionKey, new List<string> { auctionValue, transID });
            ConcurrentDictionary<string, List<string>> lottery = new ConcurrentDictionary<string, List<string>>(model.Lottery.ToDictionary(l => l.UserID, l => new List<string> { l.Percantage.ToString(), l.TransactionID }));
            Product ret = new Product(model.ProductID, model.Name, model.Description, model.Price, model.Quantity, model.ReservedQuantity, model.Rating, model.Sale, model.Weight, dimenssions, comments, policies,
                                                                                                                        strategies, attributes, model.timesBought, category, model.timesRated, auction, lottery);
            return ret;

        }



        public List<ItemDTO> SearchProductsByCategory(Category category)
        {
            lock (RemoveProductLock)
            {
                List<ProductModel> list = new List<ProductModel> ();
                using (StoreDataContext context = new StoreDataContext())
                {
                    list = context.Products.Where(x => x.ProductCategory == category.CategoryName).ToList();
                    return list.Select(pm => ModelToProduct(pm)).Select(p => p.GetProductDTO()).ToList();
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
                List<ProductModel> list = new List<ProductModel>();
                using (StoreDataContext context = new StoreDataContext())
                {
                    list = context.Products.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.ProductCategory.Contains(keyword)).ToList();
                    return list.Select(pm => ModelToProduct(pm)).Select(p => p.GetProductDTO()).ToList();
                }
                
            }
        }




        public List<ItemDTO> SearchProductsByName(string name)
        {
            lock (RemoveProductLock)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    List<ProductModel> list = context.Products.Where(x => x.Name.Contains(name)).ToList();
                    return list.Select(pm => ModelToProduct(pm)).Select(p => p.GetProductDTO()).ToList();

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
                    pm.Store = sm;                    
                    context.Products.Add(pm);
                    pm.UpdateWholeModel(product);
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