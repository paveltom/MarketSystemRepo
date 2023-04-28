using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility.
    public class Purchase_Policy
    {
        public string policyID { get; private set; }
        public string policyName { get; private set; } // as of attribute
        public int minValue { get; private set; }
        public int maxValue { get; private set; }
        public double salePercentage { get; private set; }

        public string Description { get; private set; }

        public Purchase_Policy(string polID, string polName, int max, int min, double salePercentage, string description)
        {
            this.policyID = polID;
            this.policyName = polName;
            this.maxValue = max;
            this.minValue = min;
            this.salePercentage = salePercentage;   
            this.Description = description;
        }

        public string GetID()
        {
            return this.policyID;
        }

        public double ApplyPolicy(double price, int quantity, List<string> chosenAttributes)
        {
            if (ValidatePolicy(price, quantity))
                return price / 100 * salePercentage;
            else
                return -1;
        }

        public Boolean ValidatePolicy(double price, int quantity)
        {
            // very simple validation: has to be modified later => max / min fields to be removed
            if (quantity >= minValue && quantity <= maxValue)
                return true;
            else
                return false;
        }
    }
}