using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading;
using Market_System.DomainLayer;
using Market_System.Domain_Layer.Store_Component;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DAL;
using Microsoft.Ajax.Utilities;
using Market_System.DomainLayer.UserComponent;
using EnvDTE;
using Market_System.DomainLayer.PaymentComponent;
using Microsoft.NET.StringTools;

namespace Market_System.DomainLayer.StoreComponent
{
    public class StoreFacade
    {

        private static StoreFacade Instance = null;
        private static StoreRepo storeRepo = null;
        private static ConcurrentDictionary<string, Store> stores; // locks the collection of current Stores that are in use. Remove store from collection when done.
        private static ConcurrentDictionary<string, int> storeUsage;
        private static ConcurrentDictionary<string, System.Timers.Timer> Timers;

        public static ConcurrentDictionary<string, Purchase_Policy> marketPolicies { get; private set; }
        public static ConcurrentDictionary<string, Purchase_Strategy> marketStrategies { get; private set; }


        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static StoreFacade GetInstance()
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
                        stores = new ConcurrentDictionary<string, Store>();
                        storeUsage = new ConcurrentDictionary<string, int>();
                        storeRepo = StoreRepo.GetInstance();
                        Instance = new StoreFacade();
                        marketPolicies = new ConcurrentDictionary<string, Purchase_Policy>();
                        marketStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
                        ContinueTimers();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }


        internal List<string> get_user_wokring_stores_id(string user_id)
        {
            List<string> return_me = new List<string>();
            Dictionary<string, string> storeID_role = GetStoresWorkingIn(user_id);

            foreach (KeyValuePair<string, string> entry in storeID_role)
            {
                return_me.Add(entry.Key);
            }
            return return_me;
        }

        internal bool check_if_user_can_manage_stock(string storeID, string usertID)
        {
            try
            {
                bool ret = AcquireStore(storeID).check_if_can_manage_stock(usertID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal List<string> get_user_wokring_stores(string user_id)
        {
            List<string> return_me = new List<string>();
            Dictionary<string, string> storeID_role = GetStoresWorkingIn(user_id);

            foreach(KeyValuePair<string, string> entry in storeID_role)
            {
                return_me.Add("store name:   " + GetStore(entry.Key).Name + "       ID:      " + entry.Key + "       Role:   " + entry.Value);
            }
            return return_me;
           
        }

        // ====================================================================
        // ====================== General class methods ===============================

        
        private static void ContinueTimers()
        {
            try
            {
                Timers = StoreRepo.GetInstance().RestoreTimers();
                Timers.Values.ForEach(t => t.Start());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        private static object PrivateCalculatePriceLock = new object();
        public double CalculatePrice(List<ItemDTO> products)
        {
            lock (PrivateCalculatePriceLock)
            {
                try
                {
                    double totalPrice = 0;
                    foreach (KeyValuePair<string, List<ItemDTO>> entry in GatherStoresWithProductsByItems(products))
                    {
                        totalPrice += AcquireStore(entry.Key).CalculatePrice(entry.Key, entry.Value);
                        ReleaseStore(entry.Key);
                    }
                    return totalPrice;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }


        private static object PurchaseLock = new object();
        public void Purchase(string userID, List<ItemDTO> products, string transactionID)
        {
            lock (PurchaseLock)
            {
                double price = 0.0;
                try
                {
                    AddPayment(userID, transactionID, price, false);
                    foreach (KeyValuePair<string, List<ItemDTO>> entry in GatherStoresWithProductsByItems(products))
                    {
                        price += AcquireStore(entry.Key).Purchase(userID, entry.Value);
                        ReleaseStore(entry.Key);
                    }
                }
                catch (Exception e)
                {
                    PaymentComponent.PaymentProxy.get_instance().cancel_pay(transactionID);

                    AddPayment(userID, transactionID, price, true);
                    throw e;
                }
            }
        }

        internal string get_product_name_from_prodcut_id(string product_id)
        {
            string store_id = GetStoreIdFromProductID(product_id);
            Store s =StoreRepo.GetInstance().getStore(store_id);

            return storeRepo.get_product_name_from_prodcut_id_and_store(product_id, s);
        }

        internal void close_store_temporary(string userID, string storeID)
        {
            try
            {
                AcquireStore(storeID).RemoveStore(userID);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static object GatherStoresWithProductsByItemsLock = new object();
        public ConcurrentDictionary<string, List<ItemDTO>> GatherStoresWithProductsByItems(List<ItemDTO> products) // public for unit tests
        {
            lock (GatherStoresWithProductsByItemsLock)
            {
                try
                {
                    ConcurrentDictionary<string, List<ItemDTO>> visitedStores = new ConcurrentDictionary<string, List<ItemDTO>>();
                    foreach (ItemDTO item in products)
                    {
                        String storeID = GetStoreIdFromProductID(item.GetID());
                        List<ItemDTO> productToAdd = new List<ItemDTO> { item };
                        visitedStores.AddOrUpdate(storeID, productToAdd, (k, v) => Enumerable.Concat(v, productToAdd).ToList());
                    }
                    return visitedStores;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        public string GetStoreIdFromProductID(string productID)
        {
            try
            {
                return productID.Substring(0, productID.IndexOf("_"));
            }
            catch (Exception e)
            {
                throw new Exception("invalid product ID, please type in valid product ID");
            }
        }


        public void AddPayment(string userID, string transactionID, double price, bool canceled)
        {
            try
            {
                storeRepo.AddPayment(userID, transactionID, price, canceled);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /*
        private Boolean ValidateMarketStrategyRestrictions()
        {
            // validate all the items
        }


        private double ImplementMarketSale()
        {
            // consider all the items
        }


        private Boolean ValidateCategoryStrategyRestrictions()
        {
            // validate all the items
        }


        private double ImplementCategorySale()
        {
            // consider all the items
        }
        */

        // ====================== END of General class methods ===============================
        // ===================================================================================







        // ====================================================================
        // ====================== Store methods ===============================

        private Store AcquireStore(string storeID)
        {
            try
            {
                return stores.GetOrAdd(storeID, x => 
                {
                    Lazy<Store> lazyStore = new Lazy<Store>(() =>
                    {
                        storeUsage.AddOrUpdate(storeID, 1, (k, val) => val + 1);
                        return storeRepo.getStore(storeID);
                    });
                    return lazyStore.Value;
                }); // valueFactory could be calle multiple timnes so Lazy instance may be created multiple times also, but only one will actually be used
            }
            catch (Exception e) { throw e; }
        }

        private static object ReleaseStoreLock = new object();
        private void ReleaseStore(string storeID)
        {
            lock (ReleaseStoreLock)
            {
                try
                {
                    int storeUsedBy = 0;
                    if (storeUsage.TryGetValue(storeID, out storeUsedBy))
                    {
                        if (storeUsedBy > 1)
                            storeUsage.TryUpdate(storeID, storeUsedBy - 1, storeUsedBy);
                        else
                        {
                            storeUsage.TryRemove(storeID, out _);
                            stores.TryRemove(storeID, out _);
                        }
                    }
                    else
                        stores.TryRemove(storeID, out _);
                }
                catch (Exception e) { throw e; }
            }
        }

        public void ChangeStoreName(string userID, string storeID, string newName)
        {
            try
            {
                AcquireStore(storeID).ChangeName(userID, newName);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        public StoreDTO GetStore(string storeID)
        {
            try
            {
                StoreDTO ret = AcquireStore(storeID).GetStoreDTO();
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static object newStoreLock = new object();  // so data of 2 different new stores won't intervene
        public StoreDTO AddNewStore(string userID, List<string> newStoreDetails)
        {
            lock (newStoreLock)
            {
                try
                {
                    string newIDForStore = storeRepo.getNewStoreID();
                    if (newIDForStore == "")
                        throw new Exception("Created bad store ID.");
                    if (newStoreDetails[0] == "")
                        throw new Exception("Store name missing.");
                    Store currStore = new Store(userID, newIDForStore, null, null, null, false);                    
                    storeRepo.AddStore(userID, currStore);
                    currStore.ChangeName(userID, newStoreDetails[0]);
                    
                    return currStore.GetStoreDTO();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        internal bool check_if_can_assign_manager_or_owner(string store_id, string userid)
        {
            try
            {
                bool ret = AcquireStore(store_id).check_if_can_assign_manager_or_owner(userid);
                ReleaseStore(store_id);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Dictionary<string, string> GetStoresWorkingIn(string other_User_ID)
        {
            try
            {
                Dictionary<string, string> stores_Working_In = new Dictionary<string, string>(); //<store_ID, Role>
                foreach (Store store in storeRepo.GetStores())
                {
                    string temp_role = store.GetUsersRole(other_User_ID);
                    if (temp_role != null)
                    {
                        stores_Working_In.Add(store.Store_ID, temp_role);
                    }
                }
                return stores_Working_In;
            }
            catch (Exception e) { throw e; }
        }

        internal bool check_if_can_show_infos(string storeID, string userid)
        {
            try
            {
                bool ret = AcquireStore(storeID).check_if_can_show_infos(userid);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal bool check_if_can_close_store(string storeID, string userid)
        {
            try
            {
                bool ret = AcquireStore(storeID).check_if_can_close_store(userid);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveStore(string userID, string storeID)
        {
            try
            {
                AcquireStore(storeID).RemoveStore(userID);
                stores.TryRemove(storeID, out _);
                storeUsage.TryRemove(storeID, out _);

            }
            catch (Exception e) { throw e; }
        }

        internal bool check_if_can_remove_or_add_permessions(string storeID, string userid)
        {
            try
            {
                bool ret = AcquireStore(storeID).check_if_can_remove_or_add_permessions(userid);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RestoreStore(string userID, string storeID)
        {
            try
            {
                AcquireStore(storeID).ReopenStore(userID);
            }
            catch (Exception e) { throw e; }
        }

        public string GetPurchaseHistoryOfTheStore(string userID, string storeID)
        {
            try
            {
                string ret = AcquireStore(storeID).GetPurchaseHistoryOfTheStore(userID); 
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void TransferFoundership(string userID, string storeID, string newFounderID)
        {
            try
            {
                AcquireStore(storeID).TransferFoundership(userID, newFounderID);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        public void AddStorePurchasePolicy(string userID, string storeID, List<string> newPolicyProperties)
        {
            try
            {
                AcquireStore(storeID).AddStorePurchasePolicy(userID, newPolicyProperties);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }

        }

        public void AddStorePurchasePolicy(string userID, string storeID, Purchase_Policy newPolicy) // for tests only
        {
            try
            {
                AcquireStore(storeID).AddStorePurchasePolicy(userID, newPolicy);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }

        }


        public void RemoveStorePurchasePolicy(string userID, string storeID, String policyID)
        {
            try
            {
                AcquireStore(storeID).RemoveStorePurchasePolicy(userID, policyID);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public void AddStorePurchaseStrategy(string userID, string storeID, List<string> newStrategy)
        {
            try
            {
                AcquireStore(storeID).AddStorePurchaseStrategy(userID, newStrategy);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        public void AddStorePurchaseStrategy(string userID, string storeID, Purchase_Strategy newStrategy) // for tests only
        {
            try
            {
                AcquireStore(storeID).AddStorePurchaseStrategy(userID, newStrategy);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveStorePurchaseStrategy(string userID, string storeID, String strategyID)
        {
            try
            {
                AcquireStore(storeID).RemoveStorePurchaseStrategy(userID, strategyID);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public double GetStoreProfitForDate(string userID, string storeID, string date_as_dd_MM_yyyy)
        {
            try
            {
                double profit = AcquireStore(storeID).GetStoreProfitForDate(userID, date_as_dd_MM_yyyy);
                ReleaseStore(storeID);
                return profit;
            }
            catch (Exception e) { throw e; }
        }


        public double GetMarketProfitForDate(string userID, string date_as_dd_MM_yyyy)
        {
            try
            {
                double profit = 0.0;
                List<string> allOpenStoresIds = storeRepo.GetStores().Where(s => !s.is_closed_temporary()).Select(s => s.Store_ID).ToList();
                foreach(string sid in allOpenStoresIds)
                {
                    profit += AcquireStore(sid).GetStoreProfitForDate(userID, date_as_dd_MM_yyyy);
                    ReleaseStore(sid);
                }                
                return profit;
            }
            catch (Exception e) { throw e; }
        }



        //  BID 

        public BidDTO PlaceBid(string storeID, string session, string productID, double newPrice, int quantity)
        {
            try
            {
                BidDTO ret = AcquireStore(storeID).PlaceBid(session, productID, newPrice, quantity);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }

        public bool ApproveBid(string userID, string bidID)
        {
            try
            {
                string storeID = GetStoreIdFromProductID(storeRepo.GetBid(bidID).ProductID);
                bool ret = AcquireStore(storeID).ApproveBid(userID, bidID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }


        public BidDTO GetBid(string userID, string bidID)
        {
            try
            {
                string storeID = GetStoreIdFromProductID(storeRepo.GetBid(bidID).ProductID);
                BidDTO ret = AcquireStore(storeID).GetBid(userID, bidID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }



        public void CounterBid(string userID, string bidID, double counterPrice)
        {
            try
            {
                string storeID = GetStoreIdFromProductID(storeRepo.GetBid(bidID).ProductID);
                AcquireStore(storeID).CounterBid(userID, bidID, counterPrice);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveBid(string userID, string bidID)
        {
            try
            {
                string storeID = GetStoreIdFromProductID(storeRepo.GetBid(bidID).ProductID);
                AcquireStore(storeID).RemoveBid(userID, bidID);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }



        public List<BidDTO> GetStoreBids(string userID, string storeID)
        {
            try
            {
                List<BidDTO> ret = AcquireStore(storeID).GetStoreBids(userID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }


        // Auction:
        private static object auctionTimerLock = new object();  
        public void SetAuction(string userID, string productID, double newPrice, long auctionMinutesDuration)
        {
            lock (auctionTimerLock)
            {
                try
                {
                    Store store = AcquireStore(GetStoreIdFromProductID(productID));
                    store.SetAuction(userID, productID, newPrice);
                    string founderID = store.founderID;
                    ReleaseStore(GetStoreIdFromProductID(productID));
                    AddTimer(AuctionTimerHandler, founderID, productID, auctionMinutesDuration, "auction");
                    
                }
                catch (Exception e) { throw e; }
            }
        }     


        public void AuctionTimerHandler(object sender, System.Timers.ElapsedEventArgs e, string userID, string productID)
        {
            try
            {
                System.Timers.Timer doneTimer = sender as System.Timers.Timer;
                Timers.TryRemove(productID + "_auction_timer", out _);
                doneTimer.Dispose();
                AcquireStore(GetStoreIdFromProductID(productID)).AuctionPurchase(userID, productID);
                ReleaseStore(GetStoreIdFromProductID(productID));
                RemoveAuction(userID, productID);
            }
            catch (Exception ex) { throw ex; }
        }

        public void UpdateAuction(string userID, string productID, double newPrice, string card_number, string month, string year, string holder, string ccv, string id)
        {
            string newTransID = "";
            try
            {
                newTransID = PaymentProxy.get_instance().pay(card_number, month, year, holder, ccv, id);
                string previousTransactionID = AcquireStore(GetStoreIdFromProductID(productID)).UpdateAuction(userID, productID, newPrice, newTransID);
                ReleaseStore(GetStoreIdFromProductID(productID));
                PaymentProxy.get_instance().cancel_pay(previousTransactionID);
            }
            catch (Exception ex) 
            {
                PaymentProxy.get_instance().cancel_pay(newTransID);
                throw ex; 
            }
        }

        public void RemoveAuction(string userID, string productID)
        {
            try
            {
                AcquireStore(GetStoreIdFromProductID(productID)).RemoveAuction(userID, productID);
                ReleaseStore(GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }



        // Lottery:
        public void SetNewLottery(string storeID, string userID, string productID, long durationInMinutes)
        {

            try
            {
                Store store = AcquireStore(storeID);
                store.SetNewLottery(userID, productID);
                ReleaseStore(storeID);
                string founderID = store.founderID;
                AddTimer(LotteryTimerHandler, founderID, productID, durationInMinutes, "lottery");
            }
            catch (Exception e) { throw e; }

        }

        public void RemoveLottery(string storeID, string userID, string productID)
        {

            try
            {
                Store store = AcquireStore(storeID);
                store.RemoveLottery(userID, productID);
                Dictionary<string, string> refund = store.ReturnUsersLotteryTransactions(userID, productID);
                ReleaseStore(storeID);                
                refund.ForEach(p => Refund(p.Key, p.Value));
            }
            catch (Exception e) { throw e; }
        }


        public bool AddLotteryTicket(string storeID, string userID, string productID, int percentage, string card_number, string month, string year, string holder, string ccv, string id)
        {
            string transactionID = PaymentProxy.get_instance().pay(card_number, month, year, holder, ccv, id);
            try
            {              
                Store store = AcquireStore(storeID);
                double price = store.AddLotteryTicket(userID, productID, percentage, transactionID);
                AddPayment(userID, transactionID, price, false);
                if (price == 0)
                {
                    ReleaseStore(storeID);
                    return false;
                }                
                if (store.RemainingLotteryPercantage(userID, productID) == 0)
                {
                    store.LotteryWinner(productID);
                    return true;
                }
                ReleaseStore(storeID);
                return false;
            }
            catch (Exception e) {
                PaymentProxy.get_instance().cancel_pay(transactionID);
                throw e; 
            }

        }


        public Dictionary<string, int> ReturnUsersLotteryTickets(string userID, string productID)
        {
            try
            {
                string storeID = GetStoreIdFromProductID(productID);
                Dictionary<string, int> ret = AcquireStore(storeID).ReturnUsersLotteryTickets(userID, productID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }


        public int RemainingLotteryPercantage(string storeID, string userID, string productID)
        {
            try
            {
                int ret = AcquireStore(storeID).RemainingLotteryPercantage(userID, productID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }

        public void LotteryTimerHandler(object sender, System.Timers.ElapsedEventArgs e, string userID, string productID)
        {
            try
            {
                System.Timers.Timer doneTimer = sender as System.Timers.Timer;
                Timers.TryRemove(productID + "_lottery_timer", out _);
                doneTimer.Dispose();

                string storeID = GetStoreIdFromProductID(productID);
                Store store = AcquireStore(storeID);                
                Dictionary<string, string> refundme = store.ReturnUsersLotteryTransactions(userID, productID);


                if (store.RemainingLotteryPercantage(userID, productID) < 0)
                    refundme.ForEach(p => Refund(p.Key, p.Value));
                else
                    store.LotteryWinner(productID);
                store.RemoveLottery(userID, productID);
                ReleaseStore(storeID);
            }
            catch (Exception ex) { throw ex; }
        }      


        public void Refund(string userID, string transID)
        {
            try
            {
                PaymentProxy.get_instance().cancel_pay(transID);
            }
            catch(Exception ex) { throw ex; } 
        }





        private void AddTimer(Action<object, System.Timers.ElapsedEventArgs, string, string> methodWithTimerNeeded, string founderID, string productID, long minutesDuration, string type)
        {
            System.Timers.Timer newTimer = new System.Timers.Timer(TimeSpan.FromMinutes(minutesDuration).TotalMilliseconds);
            newTimer.Elapsed += (sender, e) => methodWithTimerNeeded(sender, e, founderID, productID);
            DateTime creationTime = DateTime.Now;
            newTimer.Start();
            string timerID = productID + "_" + type + "_timer";
            storeRepo.AddTimer(newTimer, timerID, founderID, productID, creationTime, minutesDuration);
            Timers.AddOrUpdate(timerID, newTimer, (k, val) => newTimer);            
        }



        private void RemainingTime(string timerID) // <productID>_<type>_timer
        {

        }



        // ====================== END of Store methods ===============================
        // ===========================================================================





        // =======================================================================
        // ====================== EMployee methods ===============================
        public void AddEmployeePermission(string executive_username, string storeID, string employee_username, Permission newP)
        {
            try
            {
                AcquireStore(storeID).AddEmployeePermission(executive_username, employee_username, newP);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        public void RemoveEmployeePermission(string userID, string storeID, string employeeID, Permission permToRemove)
        {
            try
            {
                AcquireStore(storeID).RemoveEmployeePermission(userID, employeeID, permToRemove);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        public void AssignNewOwner(string userID, string storeID, string newOwnerID)
        {
            try
            {
                AcquireStore(storeID).AssignNewOwner(userID, newOwnerID);
                ReleaseStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Remove_Store_Owner(string userID, string storeID, string other_Owner_ID)
        {
            try
            {
                AcquireStore(storeID).Remove_Store_Owner(userID, other_Owner_ID);
                ReleaseStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AssignNewManager(string userID, string storeID, string newManagerID)
        {
            try
            {
                AcquireStore(storeID).AssignNewManager(userID, newManagerID);
                ReleaseStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<string> GetManagersOfTheStore(string userID, string storeID)
        {
            try
            {
                List<string> ret = AcquireStore(storeID).GetManagersOfTheStore(userID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> GetOwnersOfTheStore(string userID, string storeID)
        {
            try
            {
                List<string> ret = AcquireStore(storeID).GetOwnersOfTheStore(userID);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }

        public void ManageEmployeePermissions(string userID, string storeID, string employeeID, List<Permission> perms) // update only for store manager - exchanges permissions
        {
            try
            {
                AcquireStore(storeID).ManagePermissions(userID, employeeID, perms);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }






        // ====================== END of Employee methods ===============================
        // ==============================================================================





        // ======================================================================
        // ====================== Product methods ===============================

        public List<ItemDTO> GetProductsFromStore(string storeID)
        {
            try
            {
                List<ItemDTO> ret = AcquireStore(storeID).GetItems();
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ItemDTO AddProductToStore(string storeID, string usertID, List<String> productProperties)
        {
            // List<string>, length 10, as foolows:
            // Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale, Weight, Dimenssions, PurchaseAttributes, ProductCategory 
            try
            {
                ItemDTO ret = AcquireStore(storeID).AddProduct(usertID, productProperties);
                ReleaseStore(storeID);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveProductFromStore(string storeID, string userID, string productID)
        {
            try
            {
                AcquireStore(storeID).RemoveProduct(userID, productID);
                ReleaseStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> get_all_comments_of_product(string productID)
        {
            try
            {
                List<string> list_of_comments= AcquireStore(GetStoreIdFromProductID(productID)).get_all_comments_of_product(productID);
                ReleaseStore(GetStoreIdFromProductID(productID));
                return list_of_comments;
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddProductComment(string userID, string productID, string comment, double rating)
        {
            try
            {
                AcquireStore(GetStoreIdFromProductID(productID)).AddProductComment(userID, productID, comment, rating);
                ReleaseStore(GetStoreIdFromProductID(productID));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReserveProduct(ItemDTO reservedProduct)
        {
            try
            {
                AcquireStore(GetStoreIdFromProductID(reservedProduct.GetID())).ReserveProduct(reservedProduct);
                ReleaseStore(GetStoreIdFromProductID(reservedProduct.GetID()));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void LetGoProduct(ItemDTO reservedProduct)
        {
            try
            {
                AcquireStore(GetStoreIdFromProductID(reservedProduct.GetID())).LetGoProduct(reservedProduct);
                ReleaseStore(GetStoreIdFromProductID(reservedProduct.GetID()));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemDTO> SearchProductByKeyword(string keyword)
        {
            try
            {
                return storeRepo.SearchProductsByKeyword(keyword);
            }
            catch (Exception e) { throw e; }

        }

        public List<ItemDTO> SearchProductByName(string name)
        {
            try
            {
                return storeRepo.SearchProductsByName(name);
            }
            catch (Exception e) { throw e; }

        }

        public List<ItemDTO> SearchProductByCategory(Category category)
        {
            try
            {
                return storeRepo.SearchProductsByCategory(category);
            }
            catch (Exception e) { throw e; }

        }

        public void ChangeProductName(string userID, string productID, string name)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductName(userID, productID, name);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductDescription(string userID, string productID, string description)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductDescription(userID, productID, description);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductPrice(string userID, string productID, double price)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductPrice(userID, productID, price);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductSale(string userID, string productID, double sale)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductSale(userID, productID, sale);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }


        public void ChangeProductRating(string userID, string productID, double rating)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductRating(userID, productID, rating);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductQuantity(string userID, string productID, int quantity)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductQuantity(userID, productID, quantity);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductWeight(string userID, string productID, double weight)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductWeight(userID, productID, weight);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }


        internal List<ItemDTO> GetProductsFromAllStores()
        {
            try
            {
               List<Store> all_stores= StoreRepo.GetInstance().GetStores();
                List<ItemDTO> return_me = new List<ItemDTO>();
               foreach(Store store in all_stores)
                {
                    if (!store.is_closed_temporary())
                    {
                        return_me = (List<ItemDTO>)return_me.Concat(store.GetItems()).ToList();
                    }
                }
                return return_me;
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductTimesBought(string userID, string productID, int times)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductTimesBought(userID, productID, times);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductCategory(string userID, string productID, Category category)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductCategory(userID, productID, category);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductDimenssions(string userID, string productID, double[] dims)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductDimenssions(userID, productID, dims);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
            }
            catch (Exception e) { throw e; }
        }



        public void AddProductPurchasePolicy(string userID, string storeID, string productID, List<string> newPolicyProps)
        {
            try
            {
                AcquireStore(storeID).AddProductPurchasePolicy(userID, productID, newPolicyProps);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public void AddProductPurchasePolicy(string userID, string storeID, string productID, Purchase_Policy newPolicy) // for tests only
        {
            try
            {
                AcquireStore(storeID).AddProductPurchasePolicy(userID, productID, newPolicy);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        public void RemoveProductPurchasePolicy(string userID, string storeID, string productID, String policyID)
        {
            try
            {
                AcquireStore(storeID).RemoveProductPurchasePolicy(userID, productID, policyID);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public void AddProductPurchaseStrategy(string userID, string storeID, string productID, List<string> newStrategyProperties)
        {
            try
            {
                AcquireStore(storeID).AddProductPurchaseStrategy(userID, productID, newStrategyProperties);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveProductPurchaseStrategy(string userID, string storeID, string productID, String strategyID)
        {
            try
            {
                AcquireStore(storeID).RemoveProductPurchaseStrategy(userID, productID, strategyID);
                ReleaseStore(storeID);
            }
            catch (Exception e) { throw e; }
        }

        internal List<string> get_all_categories()
        {
            CategoriesOptions categories = new CategoriesOptions();
            List<string> return_me = new List<string>();
            foreach(Category cat in categories.Categories)
            {
                return_me.Add(cat.CategoryName);
            }
            return return_me;
        }

        internal List<ItemDTO> GetProductsFromAllStores_not_zero_quantity()
        {
            try
            {
                List<Store> all_stores = StoreRepo.GetInstance().GetStores();
                List<ItemDTO> return_me = new List<ItemDTO>();
                foreach (Store store in all_stores)
                {
                    if (!store.is_closed_temporary())
                    {
                        return_me = (List<ItemDTO>)return_me.Concat(store.GetItems_not_zero_quantity()).ToList();
                    }
                }
                return return_me;
            }
            catch (Exception e) { throw e; }
        }



        // ====================== END of Product methods =============================
        // ===========================================================================


        //this for tests
        public void Destroy_me()
        {
            Instance = null;
            storeRepo.destroy();
        }

        public bool Check_If_Member_Only(string member_ID)
        {
            try
            {
                foreach(Store store in storeRepo.GetStores())
                {
                    if (!store.Check_If_Member_Only(member_ID))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e) { throw e; }
        }




        // ======================================================
        // ======================== TODO ========================


        // ======================== END of TODO ========================
        // =============================================================

    }
}
