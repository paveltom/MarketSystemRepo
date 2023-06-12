using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent;

namespace Market_System.DAL.DBModels
{
    public class PurchaseStrategyModel
    {
        [Key]
        public string StrategyID { get; set; }
        public string StrategyName { get; set; }
        public string Description { get; set; }
        public string StrategyFormula { get; set; }
        public bool isDefault { get; set; }


        public virtual ProductModel Product { get; set; }
        public virtual StoreModel Store { get; set; }



        public Purchase_Strategy ModelToPolicy()
        {
            return new Purchase_Strategy(this.StrategyID, this.StrategyName, this.Description, this.StrategyFormula);
        }


        public void DefineNewMe(Purchase_Strategy strategyToCopy, bool isDefault, StoreModel store, ProductModel product)
        {
            this.StrategyID = strategyToCopy.StrategyID;
            this.StrategyName = strategyToCopy.StrategyName;
            this.Description = strategyToCopy.Description;
            this.StrategyFormula = strategyToCopy.strFormula;
            this.isDefault = isDefault;
            this.Store = store;
            this.Product = product;
        }

    }
}