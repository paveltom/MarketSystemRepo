using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;

namespace Market_System.DomainLayer.StoreComponent
{
    public abstract class Purchase_Policy
    {
        public string PolicyID { get; private set; }// ???
        public string PolicyName { get; private set; }
        public double SalePercentage { get; private set; }
        public string Description { get; private set; }
        public Statement SalePolicyFormula { get; private set; }


        public Purchase_Policy(string polID, string polName, double salePercentage, string description)
        {
            this.PolicyID = polID;
            this.PolicyName = polName;
            this.SalePercentage = salePercentage;
            this.Description = description;
        }

        // returns items with saled price
        public abstract List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes);

        public abstract Boolean Validate(List<ItemDTO> chosenProductsWithAttributes);



    }
}