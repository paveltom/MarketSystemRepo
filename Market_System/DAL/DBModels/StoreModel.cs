using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class StoreModel
    {
        // everything that is other objects are defined as VIRTUAL

        [Key]
        public string StoreID { get; set; } // key in DB

        public string Name { get; set; }
        
        public string founderID { get; set; } //founder's userID

        public bool temporaryClosed { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }
        public virtual ICollection<PurchaseStrategyModel> Strategies { get; set; }
        public virtual ICollection<PurchasePolicyModel> Policies { get; set; }
        public virtual ICollection<EmployeeModel> Employees { get; set; }
        public virtual ICollection<StorePurchaseHistoryObjModel> PurchaseHistory { get; set; }



        /*public Store ModelToStore()
        {
            List<Purchase_Policy> policies = new List<Purchase_Policy>();
            List<Purchase_Policy> defaultPolicies = new List<Purchase_Policy>();
            this.Policies.ForEach(x =>
            {
                if (x.isDefault)
                    defaultPolicies.Add(x.ModelToPolicy());
                else
                    policies.Add(x.ModelToPolicy());
            });

            List<Purchase_Strategy> strategies = new List<Purchase_Strategy>(); 
            List<Purchase_Strategy> defaultStrategies = new List<Purchase_Strategy>();
            this.Strategies.ForEach(x =>
            {
                if (x.isDefault) 
                    defaultStrategies.Add(new Purchase_Strategy(x.StrategyID, x.StrategyName, x.Description, x.StrategyFormula)); 
                else 
                    strategies.Add(new Purchase_Strategy(x.StrategyID, x.StrategyName, x.Description, x.StrategyFormula));
            });
            List<string> productIds = new List<string>();
            if (this.Products != null)
                productIds = this.Products.Select(x => x.ProductID).ToList();
            Store ret = new Store(this.founderID, this.StoreID, policies, strategies, productIds, this.temporaryClosed);            

            ret.productDefaultPolicies = new ConcurrentDictionary<string, Purchase_Policy>(defaultPolicies.ToDictionary(keySelector: x => x.PolicyID, elementSelector: x => x));
            ret.productDefaultStrategies = new ConcurrentDictionary<string, Purchase_Strategy>(defaultStrategies.ToDictionary(keySelector: x => x.StrategyID, elementSelector: x => x)); ;

            return ret;

        }*/


        public void UpdateWholeModel(Store updatedStore)
        {
            this.Name = updatedStore.Name;
            
            this.founderID = updatedStore.founderID;
            this.temporaryClosed = updatedStore.is_closed_temporary();
            List<PurchaseStrategyModel> remove = new List<PurchaseStrategyModel>();
            this.Strategies.ForEach(x =>
            {
                if(!updatedStore.storeStrategies.Any(s => s.Key == x.StrategyID) && !updatedStore.productDefaultStrategies.Any(s => s.Key == x.StrategyID))
                    remove.Add(x);
            });

            if (remove.Count > 0)
                remove.ForEach(x => this.Strategies.Remove(x));
            else
            {
                updatedStore.storeStrategies.ForEach(x =>
                {
                    if (!this.Strategies.Any(s => s.StrategyID == x.Key))
                    {
                        PurchaseStrategyModel model = new PurchaseStrategyModel();
                        model.DefineNewMe(x.Value, false, this, null);
                    }
                });

                updatedStore.productDefaultStrategies.ForEach(x =>
                {
                    if (!this.Strategies.Any(s => s.StrategyID == x.Key))
                    {
                        PurchaseStrategyModel model = new PurchaseStrategyModel();
                        model.DefineNewMe(x.Value, true, this, null);
                    }
                });

            }

        }
    }
}