using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class PurchaseStrategyModel
    {
        [Key]
        public string StrategyID { get; private set; }
        public string StrategyName { get; private set; }
        public string Description { get; private set; }
        public string StrategyFormula { get; private set; }

        public virtual ProductModel Product { get; private set; }
        public virtual StoreModel Store { get; private set; }

    }
}