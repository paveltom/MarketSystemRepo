using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent.Predicates;


namespace Market_System.DomainLayer.StoreComponent
{
    public class Purchase_Policy
    {
        public string PolicyID { get; private set; }
        public string PolicyName { get; private set; } 
        public double SalePercentage { get; private set; }

        public string Description { get; private set; }

        public Predicate SalePolicyFormula { get; private set; }

        public Purchase_Policy(string polID, string polName, int max, int min, double salePercentage, string description)
        {
            this.PolicyID = polID;
            this.PolicyName = polName;
            this.SalePercentage = salePercentage;   
            this.Description = description;
        }

        // returns the SALE VALUE: 10% sale on 20$ returns '2'.
        public double ApplyProductPolicy(double initPrice, int quantity, ConcurrentDictionary<string, string> chosenAttributes)
        {
            if (ValidateProduct(chosenAttributes, quantity))
                return initPrice / 100 * SalePercentage;
            else
                return 0;
        }

        public double ApplyStorePolicy(List<ItemDTO> chosenProductsWithAttributes)
        {
            double initPrice = 0;
            chosenProductsWithAttributes.Select(x => initPrice += x.Price);
            if (ValidateStore(chosenProductsWithAttributes))
                return initPrice / 100 * SalePercentage;
            else
                return 0;
        }

        public double ApplyCategoryPolicy(List<ItemDTO> chosenProductsWithAttributes)
        {
            double initPrice = 0;
            chosenProductsWithAttributes.Select(x => initPrice += x.Price);
            if (ValidateCategory(chosenProductsWithAttributes))
                return initPrice / 100 * SalePercentage;
            else
                return 0;
        }

        public Boolean ValidateProduct(ConcurrentDictionary<string, string> chosenAttributes, int quantity)
        {
            if (this.SalePolicyFormula.Satisfies(quantity, chosenAttributes))
                return true;
            else
                return false;
        }

        public Boolean ValidateStore(List<ItemDTO> chosenProductsWithAttributes)
        {
            throw new NotImplementedException();    
        }

        public Boolean ValidateCategory(List<ItemDTO> chosenProductsWithAttributes)
        {
            throw new NotImplementedException();
        }
    }
}