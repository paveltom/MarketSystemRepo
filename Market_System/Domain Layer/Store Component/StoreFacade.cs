using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class StoreFacade
    {

        //This variable is going to store the Singleton Instance
        private static StoreFacade Instance = null;
        private static StoreRepo storeRepo;
        //private static ConcurrentDictionary<string, Store> stores; // locks the collection of current Stores that are in use. Remove store from collection when done.
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

        public Store GetStore(string storeID)
        {
            try
            {
                return storeRepo.GetStore(storeID);
            } catch (Exception e)
            {
                throw e;
            }
        }

        public List<Product> GetProductsFromStore(string storeID)
        {
            try
            {
                return storeRepo.GetStore(storeID).GetProducts();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private static object newStoreLock = new object();  
        public void Add_New_Store(string userID, string storeID, List<string> newStoreDetails)
        {
            lock (newStoreLock)
            {
                try
                {
                    string newIDForStore = storeRepo.GetNewStoreID();
                    Store currStore = new Store(userID, newIDForStore);
                    storeRepo.AddStore(currStore);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void RemoveStore(string storeID)
        {
            try
            {
                storeRepo.RemoveStore(storeID)
            } catch (Exception e) { throw e; }
        }

        public void Add_Product_To_Store(string storeID, string usertID, List<string> productProperties) //may be pass ItemDTO instead
        {
            try
            {
               storeRepo.getStore(storeID).Add_Product(usertID, productProperties);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Remove_Product_From_Store(string store_ID, string userID, Product product)
        {
            try
            {
                storeRepo.getStore(storeID).Remove_Product(userID, product.Product_ID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditProduct(string userID, string productID, List<String> editedProductDetails)
        {
            try
            {
                storeRepo.GetStore(GetStoreIdFromProductID(productID)).EditProduct(userID, productID, editedProductDetails);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Owner(string userID, string storeID, string newOwnerID)
        {
            try
            {
                storeRepo.getStore(storeID).Add_New_Owner(userID, newOwnerID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Managaer(string userID, string storeID, string newManagerID)
        {
            try
            {
                Store currStore = storeRepo.GetStore(storeID).Add_New_Owner(userID, newManagerID);
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
                return storeRepo.GetStore(storeID).GetManagersOfTheStore(userID);
            }
            catch (Exception e) { 
                throw e;        
            }
        }

        public List<string> GetOwnersOfTheStore(string storeID)
        {
            try
            {
                return storeRepo.GetStore(storeID).GetAllOwnersOfTheStore();
            } catch (Exception e) { throw e; }
        }
        


        private static object CalculatePriceLock = new object();    
        public double CalculatePrice(List<ItemDTO> products)
        {
            // maybe add here thread functionality instead - every one who access the method receives a thread to calculate price
            lock (CalculatePriceLock)
            {
                try
                {
                    double totalPrice = 0;
                    foreach (KeyValuePair<string, List<ItemDTO>> entry in GatherStoresWithProductsByItems(products))
                    {
                        totalPrice += this.storeRepo.GetStore(entry.Key).CalculatePrice(entry.Value);
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
        public void Purchase(List<ItemDTO> products)
        {
            // maybe add here thread functionality instead - every one who access the method receives a thread to purchase items
            lock (PurchaseLock)
            {
                try
                {
                    foreach (KeyValuePair<string, List<ItemDTO>> entry in GatherStoresWithProductsByItems(products))
                    {
                        this.storeRepo.GetStore(entry.Key).Purchase(entry.Value);
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
                storeRepo.GetStore(GetStoreIdFromProductID(productID)).AddProductComment(string userID, string productID, string comment, double rating);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReserveProduct(strnig productID)
        {
            try
            {
                storeRepo.GetStore(GetStoreIdFromProductID(productID)).ReserveProduct(productID);
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        public List<string> GetPurchaseHistoryOfTheStore(string userID, string storeID)
        {
            try
            {
                return storeRepo.GetStore(storeID).GetPurchaseHistoryOfTheStore(userID);
            } 
            catch (Exception e) {
                throw e;
            }
        }

        public List<Product> SearchProductByKeyword(string keyword)
        {
            try
            {
                return storeRepo.SearchProductsByKeyword(keyword);
            } catch (Exception e) { throw e; }

        }

        public List<Product> SearchProductByName(string name)
        {
            try
            {
                return storeRepo.SearchProductsByKeyword(name);
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

        * public void search_product_by_category() //2.2
            
     */




