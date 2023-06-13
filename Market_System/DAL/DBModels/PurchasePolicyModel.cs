using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Microsoft.Ajax.Utilities;
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
        public string PolicyID { get; set; }// ???
        public string PolicyName { get; set; }
        public double SalePercentage { get; set; }
        public string Description { get; set; }
        public string SalePolicyFormula { get; set; }
        public string Target { get; set; } // Product, Store, Category
        public string TargetValue { get; set; } // ProductID, StoreID, CategoryName
        public bool isDefault { get; set; }
        public string MaximumPairs { get; set; }

        public virtual ProductModel Product { get; set; }
        public virtual StoreModel Store { get; set; }



        public Purchase_Policy ModelToPolicy()
        {
            switch (this.Target)
            {
                case "Product":
                    return (Purchase_Policy)new ProductPolicy(this.PolicyID, this.PolicyName, this.SalePercentage, this.Description, this.SalePolicyFormula, this.TargetValue);
                case "Store":
                    return (Purchase_Policy)new StorePolicy(this.PolicyID, this.PolicyName, this.SalePercentage, this.Description, this.TargetValue, this.SalePolicyFormula);
                case "Category":
                    return (Purchase_Policy)new CategoryPolicy(this.PolicyID, this.PolicyName, this.SalePercentage, this.Description, this.TargetValue, this.SalePolicyFormula);
                default:
                    return (Purchase_Policy)new MaximumPolicy(this.PolicyID, this.PolicyName, this.Description, this.MaximumPairs.Split(';').Where(s => s.Length > 1).ToDictionary(s => s.Split('+')[0], s => Double.Parse(s.Split('+')[1])));
            }
        }

        public void DefineNewMe(Purchase_Policy policyToCopy, bool isDefault, StoreModel store, ProductModel product)
        {
            this.PolicyID = policyToCopy.PolicyID;
            this.PolicyName = policyToCopy.PolicyName;
            this.Description = policyToCopy.Description;
            this.SalePolicyFormula = policyToCopy.strFormula;
            this.isDefault = isDefault;
            this.Store = store;
            this.Product = product;

            if (policyToCopy is ProductPolicy)
                this.Target = "Product";
            else if (policyToCopy is StorePolicy)
                this.Target = "Store";
            else if (policyToCopy is CategoryPolicy)
                this.Target = "Category";
            else
            {
                MaximumPolicy mp = policyToCopy as MaximumPolicy;
                this.Target = "Maximum";
                mp.productsSale.ForEach(p => this.MaximumPairs += p.Key + "+" + p.Value + ";");
            }
        }
    }

}