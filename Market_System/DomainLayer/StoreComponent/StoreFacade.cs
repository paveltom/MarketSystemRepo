using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading;
using Market_System.DomainLayer;

namespace Market_System.DomainLayer.StoreComponent
{
    public class StoreFacade
    {

        private static StoreFacade Instance = null;
        private static StoreRepo storeRepo = null;
        private static ConcurrentDictionary<string, Store> stores; // locks the collection of current Stores that are in use. Remove store from collection when done.
        private static ConcurrentDictionary<string, int> storeUsage;

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
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        // ====================================================================
        // ====================== General class methods ===============================

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
                        totalPrice += AcquireStore(entry.Key).CalculatePrice(entry.Value);
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
        public void Purchase(string userID, List<ItemDTO> products)
        {
            // maybe add here thread functionality instead - every one who access the method receives a thread to purchase items
            lock (PurchaseLock)
            {
                try
                {
                    foreach (KeyValuePair<string, List<ItemDTO>> entry in GatherStoresWithProductsByItems(products))
                    {
                        AcquireStore(entry.Key).Purchase(userID, entry.Value);
                        ReleaseStore(entry.Key);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        internal void close_store_temporary(string sessionID, string storeID)
        {
            try
            {
                foreach (KeyValuePair<string, Store> entry in stores)
                {
                    if (entry.Key.Equals(storeID))
                    {
                        entry.Value.RemoveStore(sessionID);
                    }
                }
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
                throw new NotImplementedException();
            }
        }
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
        public void AddNewStore(string userID, List<string> newStoreDetails)
        {
            lock (newStoreLock)
            {
                try
                {
                    string newIDForStore = storeRepo.getNewStoreID();
                    if (newIDForStore == "")
                        throw new Exception("Created bad store ID.");
                    Store currStore = new Store(userID, newIDForStore, null, null, null, false);
                    currStore.ChangeName(userID, newStoreDetails[0]);
                    storeRepo.AddStore(userID, currStore);
                }
                catch (Exception e)
                {
                    throw e;
                }
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

        public void RestoreStore(string userID, string storeID)
        {
            try
            {
                storeRepo.re_open_closed_temporary_store(userID, storeID);
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

        public void AddStorePurchasePolicy(string userID, string storeID, Purchase_Policy newPolicy)
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


        public void AddStorePurchaseStrategy(string userID, string storeID, Purchase_Strategy newStrategy)
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

        public void AddProductToStore(string storeID, string usertID, List<String> productProperties)
        {
            // List<string>, length 10, as foolows:
            // Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale, Weight, Dimenssions, PurchaseAttributes, ProductCategory 
            try
            {
                AcquireStore(storeID).AddProduct(usertID, productProperties);
                ReleaseStore(storeID);
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

        public void ChangeProductSale(string userID, string productID, double sale)
        {
            try
            {
                AcquireStore(this.GetStoreIdFromProductID(productID)).ChangeProductSale(userID, productID, sale);
                ReleaseStore(this.GetStoreIdFromProductID(productID));
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


        public void AddProductPurchasePolicy(string userID, string storeID, string productID, Purchase_Policy newPolicy)
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


        public void AddProductPurchaseStrategy(string userID, string storeID, string productID, Purchase_Strategy newStrategy)
        {
            try
            {
                AcquireStore(storeID).AddProductPurchaseStrategy(userID, productID, newStrategy);
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



        // ====================== END of Product methods =============================
        // ===========================================================================


        //this for tests
        public void Destroy_me()
        {
            Instance = null;
        }




        // ======================================================
        // ======================== TODO ========================


        // ======================== END of TODO ========================
        // =============================================================

    }
}
