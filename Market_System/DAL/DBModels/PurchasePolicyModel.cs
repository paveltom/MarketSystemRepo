using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class PurchasePolicyModel
    {
        [Key]
        public string PolicyID { get; private set; }// ???
        public string PolicyName { get; private set; }
        public double SalePercentage { get; private set; }
        public string Description { get; private set; }
        public string SalePolicyFormula { get; private set; }
        public string Target { get; set; } // Product, Store, Category
        public string TargetValue { get; set; } // ProductID, StoreID, CategoryName

        public virtual ProductModel Product { get; private set; }
        public virtual StoreModel Store { get; private set; }
    }
}