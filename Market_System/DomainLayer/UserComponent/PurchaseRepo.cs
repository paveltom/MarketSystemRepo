using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class PurchaseRepo
    {

        private static Dictionary<string, List<PurchaseHistoryObj>> purchases_database; // key username , value list of purchasehistoryobj

        private static PurchaseRepo Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static PurchaseRepo GetInstance()
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
                        
                        purchases_database = new Dictionary<string,List<PurchaseHistoryObj>>();
                        Instance = new PurchaseRepo();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        public void destroy_me()
        {
            Instance = null;
        }


        public void save_purchase(string username, PurchaseHistoryObj new_purchase)
        {
            if(purchases_database.ContainsKey(username))
            {
                purchases_database[username].Add(new_purchase);
            }
            else
            {
                purchases_database.Add(username, new List<PurchaseHistoryObj>());
                purchases_database[username].Add(new_purchase);
            }
        }

        internal List<PurchaseHistoryObj> get_history(string username)
        {
           if(purchases_database.ContainsKey(username))
            {
                return purchases_database[username];
            }
            throw new Exception("user never bought anything!");
        }
    }
}