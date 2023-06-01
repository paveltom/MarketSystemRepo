using Market_System.user_component_DAL.Models;
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
          //  List<purchase_history_model> histores = User_DAL_controller.GetInstance().get_context().get_purhcase_histories_by_username(username);
           // if (histores == null)
           // {
                List<Bucket_model_history> list_of_bucket_model_histroy = new_purchase.convert_buckets_to_Bucket_model_history();

                User_DAL_controller.GetInstance().get_context().Add(new purchase_history_model(username, list_of_bucket_model_histroy, new_purchase.get_total_price()));
                User_DAL_controller.GetInstance().get_context().SaveChanges();
          //  }
   /*
            if (purchases_database.ContainsKey(username))
            {
                purchases_database[username].Add(new_purchase);
            }
            else
            {
                purchases_database.Add(username, new List<PurchaseHistoryObj>());
                purchases_database[username].Add(new_purchase);
            }
   */
        }

        internal List<PurchaseHistoryObj> get_history(string username)
        {
           List< purchase_history_model> histroies = User_DAL_controller.GetInstance().get_context().get_purhcase_histories_by_username(username);
            if (histroies != null && histroies.Count>0)
            {
                return convert_purchase_history_model_to_PurchaseHistoryObj(histroies);
            }
            /*
            if (purchases_database.ContainsKey(username))
            {
                return purchases_database[username];
            }
            */
            throw new Exception("user never bought anything!");
        }

        private List<PurchaseHistoryObj> convert_purchase_history_model_to_PurchaseHistoryObj(List<purchase_history_model> histroies)
        {
            List<PurchaseHistoryObj> phol = new List<PurchaseHistoryObj>();
            List<Bucket> buckets = new List<Bucket>();
            foreach(purchase_history_model phm in histroies)
            {


                phol.Add(new PurchaseHistoryObj(phm.username, convert_list_of_bucket_history_model_to_list_of_buckets(phm.baskets), phm.total_price));

            }
            return phol;
        }

        private List<Bucket> convert_list_of_bucket_history_model_to_list_of_buckets(List<Bucket_model_history> baskets)
        {
            List<Bucket> reutn_me = new List<Bucket>();
            foreach (Bucket_model_history bmh in baskets)
            {
                reutn_me.Add(new Bucket(bmh));
            }
            return reutn_me;
        }

        internal bool check_if_user_bought_item(string username, string product_id)
        {
            List<PurchaseHistoryObj> history = purchases_database[username];
            foreach(PurchaseHistoryObj history_obj in history)
            {
                if(history_obj.check_if_contains_product(product_id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}