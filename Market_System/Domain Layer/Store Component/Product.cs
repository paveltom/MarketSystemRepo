using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Product : Property
    {
        // make the fields and/or its getter/setter threadsafe

        public String Product_ID { get; private set; } // composed of 2 parts: *storeId*_*inStoreProductId* - 1234_9876 for exmpl
        private ConcurrentDictionary<String, Purchase_Policy> PurchasePolicies; // make it threadsafe ChaiinOfResponsobolities 
        private ConcurrentDictionary<String, Purchase_Strategy> PurchaseStrategies; // make it threadsafe ChainOfResponsibilities
        private ConcurrentDictionary<String, List<String>> PurchaseAttributes;
        private ConcurrentBag<String> Comments;
        private Array<int> Dimenssions; // array of 3

        public String Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; } 
        public int ReservedQuantity { get; set; }
        public double Rating { get; set; } // between 1-10
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public Catagory ProductCategory { get; set; }   // (mabye will be implementing by composition design pattern to support a sub catagoring.)


        public Product(String product_ID, String name, String, String description, double Price, int initQuantity, double rating, double weight, Array<int> dimenssions, List<String> comments, List<Purchase_Policy> purchase_Policies, 
                        List<Purchase_Strategy> purchase_Strategies, Dictionary<Strinbg, List<String>> product_Attributes, ) // can be Builder
        {
            this.product_ID = product_ID;
            // add here method to initialize other comlex properties
        }

        public AddPurchasePolicy(Purchase_Policy newPolicy)
        {
            try
            {
                this.PurchasePolicies.AddOrUpdate(newPolicy.id, newPolicy, (existingID, existingPolicy) => existingPolicy = newPolicys)
            } catch(Exception e) { }
        }

        public Boolean RemovePurchasePolicy(String policyID)
        {
            try
            {
                return this.PurchasePolicies.TryRemove(policyID, out Purchase_Policy result) // the removed item can be collected by TryRemove(policyID, out Purchase_Policy result)
            } catch(Exception e) { }
        }

        public Boolean AddPurchaseStrategy(Purchase_Strategy newStrategy)
        {
            try
            {
                return this.PurchaseStrategies.AddOrUpdate(newPolicy.id, newPolicy, (existingID, existingPolicy) => existingPolicy = newPolicys)
            }
            catch (Exception e) { return false; }
        }

        public Boolean RemovePurchasePolicy(String strategyID)
        {
            try
            {
                return this.PurchaseStrategies.TryRemove(policyID, out Purchase_Policy result) // the removed item can be collected by TryRemove(policyID, out Purchase_Policy result)
            }
            catch (Exception e) { return false;  }
        }

        public double ImplementSale(List<String> chosenAttributes)
        {
            PurchasePolicies.accept(chosenAttributes); // chain of responsibility that returns the price with implemented sale
            // get items
            // concrete product varies by PurchaseAttributes, so the sale calculated considering chosenAttributes by
            // applying PurchasePolicies chain of responsibility
        }
        
        
        // ========Methods ToDo==========:
        // passing a data for store representation - probably ItemDTO
        // return price after sale appliement



    }

 
}