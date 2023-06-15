using Market_System.DAL;
using Market_System.DAL.DBModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class PurchaseRepo
    {
        private static PurchaseRepo Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        public static PurchaseRepo GetInstance()
        {
            if (Instance == null)
            {
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {                       
                        Instance = new PurchaseRepo();
                    }
                } //Critical Section End
            }
            return Instance;
        }

        public void destroy_me()
        {
            Instance = null;
        }


        public void save_purchase(string username, PurchaseHistoryObj new_purchase)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                CartModel cart = context.Carts.SingleOrDefault(c => c.CartID == username + "Cart");
                if (cart == null)
                    throw new Exception("404 - your cart wasn't found.");

                UserPurchaseHistoryObjModel model = new UserPurchaseHistoryObjModel();
                model.Username = username;
                model.TotalPrice = new_purchase.GetTotalPrice();
                model.PurchaseDateTicks = new_purchase.PurchaseDateTime.Ticks.ToString();
                model.HisstoryID = username + model.PurchaseDateTicks;
                List<string> bucketsIDs = new_purchase.GetBuckets().Select(b => b.GetID()).ToList();
                context.Buckets.Where(b => bucketsIDs.Contains(b.BucketID)).ForEach(b => {
                    cart.Buckets.Remove(b);
                    b.Purchased = true;
                    model.Buckets.Add(b);
                    });
                context.UserPurchases.Add(model);
                context.SaveChanges();
            }
        }

        public void SaveBucket(Bucket saveMe)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                BucketModel model;
                if ((model = context.Buckets.SingleOrDefault(b => b.BucketID == saveMe.GetID())) == null)
                {
                    model = new BucketModel();
                    model.BucketID = saveMe.GetID();
                    model.Purchased = false;
                    model.StoreID = saveMe.get_store_id();
                    model.Products = saveMe.get_products().Aggregate("", (acc, p) => acc += p.Key + "+" + p.Value, acc => acc);
                    context.Buckets.Add(model);

                }
                // only update added product
                model.Products = saveMe.get_products().Aggregate("", (acc, p) => acc += p.Key + "+" + p.Value, acc => acc);
                context.SaveChanges();
            }
        }

        internal List<PurchaseHistoryObj> get_history(string username)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                List<PurchaseHistoryObj> ret = context.UserPurchases.Where(p => p.Username == username).ToList().Select(p => p.ModelToHistory()).ToList();
                if(ret.Count == 0)
                    throw new Exception("user never bought anything!");
                return ret;
            }
            
        }

        internal bool check_if_user_bought_item(string username, string product_id)
        {
            using (StoreDataContext context = new StoreDataContext())
            {                
                foreach (UserPurchaseHistoryObjModel model in context.UserPurchases.Where(u => u.Username == username).ToList())
                    if(model.Buckets.Any(b => b.ModelToBucket().check_if_product_exists(product_id)))
                        return true;
                return false;
            }            
        }
    }
}