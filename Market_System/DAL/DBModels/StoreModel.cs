using Market_System.DomainLayer.StoreComponent;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public virtual ICollection<StorePurchaseHistoryObjModel> PurchaseHistory { get; set; }
        public virtual ICollection<BidModel> Bids { get; set; } // init !!!!!!!!!!!!!!!!!


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
                        this.Strategies.Add(model);
                    }
                });

                updatedStore.productDefaultStrategies.ForEach(x =>
                {
                    if (!this.Strategies.Any(s => s.StrategyID == x.Key))
                    {
                        PurchaseStrategyModel model = new PurchaseStrategyModel();
                        model.DefineNewMe(x.Value, true, this, null);
                        this.Strategies.Add(model);
                    }
                });

            }

            List<PurchasePolicyModel> removePolicies = new List<PurchasePolicyModel>();
            this.Policies.ForEach(x =>
            {
                if (!updatedStore.storePolicies.Any(s => s.Key == x.PolicyID) && !updatedStore.productDefaultPolicies.Any(s => s.Key == x.PolicyID))
                    removePolicies.Add(x);
            });

            if (removePolicies.Count > 0)
                removePolicies.ForEach(x => this.Policies.Remove(x));
            else
            {
                updatedStore.storePolicies.ForEach(x =>
                {
                    if (!this.Policies.Any(s => s.PolicyID == x.Key))
                    {
                        PurchasePolicyModel model = new PurchasePolicyModel();
                        model.DefineNewMe(x.Value, false, this, null);
                        this.Policies.Add(model);
                    }
                });

                updatedStore.productDefaultPolicies.ForEach(x =>
                {
                    if (!this.Policies.Any(s => s.PolicyID == x.Key))
                    {
                        PurchasePolicyModel model = new PurchasePolicyModel();
                        model.DefineNewMe(x.Value, true, this, null);
                        this.Policies.Add(model);
                    }
                });

            }

        }
    }
}