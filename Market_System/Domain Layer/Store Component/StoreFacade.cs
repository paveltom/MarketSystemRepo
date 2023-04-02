using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class StoreFacade
    {
        //This variable is going to store the Singleton Instance
        private static StoreFacade Instance = null;
        private static StoreRepo storeRepo;
        private static List<Store> stores;


        //To use the lock, we need to create one variable
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
                        stores = new List<Store>();
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

        public void Add_New_Store(string userID, string storeID)
        {
            try
            {
                storeRepo.AddStore(userID, storeID);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void Add_Product_To_Store(string storeID, string usertID, List<string> productProperties) //may be pass ItemDTO instead
        {
            try
            {
                Store currStore = storeRepo.getStore(storeID);
                currStore.Add_Product(userID, productProperties);
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
                Store currStore = storeRepo.getStore(storeID);
                currStore.Remove_Product(userID, product.Product_ID);
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
                Store currStore = storeRepo.getStore(storeID);
                currStore.Add_New_Owner(userID, newOwnerID);
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
                Store currStore = storeRepo.getStore(storeID);
                currStore.Add_New_Owner(userID, newManagerID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void CalculatePrice(List<ItemDTO> products)
        {
            try
            {
                throw new NotImplementedException("implement me please");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void Purchase(List<ItemDTO> products)
        {
            try
            {
                throw new NotImplementedException("implement me please");
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}

// ToDo:
// stores have to concurrent??? ConcurrentBag<Store>