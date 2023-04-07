using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading;

namespace Market_System.Domain_Layer.Store_Component
{
    public class StoreFacade
    {

        //This variable is going to store the Singleton Instance
        private static StoreFacade Instance = null;
        private static StoreRepo storeRepo;
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
                        stores = new ConcurrentDictionary<string, Store>;
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

        // public TValue GetOrAdd (TKey key, Func<TKey,TValue> valueFactory);

        private Store AcquireStore (string storeID)
        {
            try 
            {
                return ((Lazy<Store>)stores.GetOrAdd(storeID, (k, val) => new Lazy<Store>(() =>
                {
                    storeUsage.GetOrAdd(k, 1, (k, val) => val + 1);
                    return storeRepo.GetStore(k);
                }))).Value; // valueFactory could be calle multiple timnes so Lazy instance may be created multiple times also, but only one will actually be used
            } catch (Exception e) { throw e; }
        }

        private static object ReleaseStoreLock = new object();
        private void ReleaseStore (string storeID)
        {
            lock (ReleaseStoreLock)
            {
                try
                {
                    if (storeUsage.TryRemove(storeID, 1))
                        stores.TryRemove(storeID, out _);
                    else
                        storeUsage.TryUpdate(storeID, (storeUsage.TryGetValue(storeID) - 1), _);
                }
                catch (Exception e) { throw e; }
            }
        }
        
        public StoreDTO GetStore(string storeID)
        {
            try
            {
                StoreDTO ret = AcquireStore(storeID).GetStoreDTO();
                ReleaseStore(storeID);
                return ret;
            } catch (Exception e)
            {
                throw e;
            }
        }

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

        private static object newStoreLock = new object();  // so data of 2 different new stores won't intervene
        public void AddNewStore(string userID, string storeID, List<string> newStoreDetails)
        {
            lock (newStoreLock)
            {
                try
                {
                    string newIDForStore = storeRepo.GetNewStoreID();
                    if (newIDForStore == "")
                        return false;
                    Store currStore = new Store(userID, newIDForStore, newStoreDetails, null);
                    storeRepo.AddStore(currStore);
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

            } catch (Exception e) { throw e; }
        }

        public void AddProductToStore(string storeID, string usertID, List<string> productProperties) //may be pass ItemDTO instead
        {
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

        public void Remove_Product_From_Store(string store_ID, string userID, string productID)
        {
            try
            {
                AcquireStore(storeID).RemoveProduct(usertID, productProperties);
                ReleaseStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditProduct(string userID, string productID, List<String> editedProductDetails) //may be pass ItemDTO instead
        {
            try
            {
                AcquireStore(storeID).EditProduct(usertID, productProperties);
                ReleaseStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
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
            catch (Exception e) { 
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
            } catch (Exception e) { throw e; }
        }
        

        /*public double CalculatePrice(List<ItemDTO> products)
        {
            try
            {

                return new Thread(() => { PrivateCalculatePrice(products)});

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        */

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


        private static object GatherStoresWithProductsByItemsLock = new object();
        private ConcurrentDictionary<string, List<ItemDTO>> GatherStoresWithProductsByItems(List<ItemDTO> products)
        {
            lock (GatherStoresWithProductsByItemsLock)
            {
                try
                {
                    ConcurrentDictionary<string, List<string>> visitedStores = new ConcurrentDictionary<string, List<string>>();
                    foreach (ItemDTO item in products)
                    {
                        String storeID = GetStoreIdFromProductID(item.GetID());
                        List<ItemDTO> productToAdd = new List<ItemDTO> { item };
                        visitedStores.AddOrUpdate(storeID, productToAdd, (k, v) => v.Concat(productToAdd));
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
            } catch (Exception e)
            {
                throw new NotImplementedException();    
            }
        }


        public void AddProductComment(string userID, string productID, string comment, double rating)
        {
            try
            {
                AcquireStore(GetStoreIdFromProductID(productID)).AddProductComment(string userID, string productID, string comment, double rating);
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
            catch(Exception e)
            {
                throw e;
            }
        }

        public Boolean LetGoProduct(ItemDTO reservedProduct)
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


        public List<string> GetPurchaseHistoryOfTheStore(string userID, string storeID)
        {
            try
            {
                List<string> ret = AcquireStore(storeID).GetPurchaseHistoryOfTheStore(userID);
                ReleaseStore(storeID);
                return ret;
            } 
            catch (Exception e) {
                throw e;
            }
        }

        public List<ItemDTO> SearchProductByKeyword(string keyword)
        {
            try
            {
                return storeRepo.SearchProductsByKeyword(keyword);
            } catch (Exception e) { throw e; }

        }

        public List<ItemDTO> SearchProductByName(string name)
        {
            try
            {
                return storeRepo.SearchProductsByName(name);
            }
            catch (Exception e) { throw e; }

        }

        public List<ItemDTO> SearchProductByName(string category)
        {
            try
            {
                return storeRepo.SearchProductsByCategory(name);
            }
            catch (Exception e) { throw e; }

        }


        //this for tests
        public void Destroy_me()
        {    
            Instance = null;     
        }
    }

    /*
    TODO:

            
     */




