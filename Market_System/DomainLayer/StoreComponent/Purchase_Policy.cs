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

        public Purchase_Policy(string polID, string polName, int max, int min, double salePercentage)
        {
            this.policyID = polID;
            this.policyName = polName;
            this.maxValue = max;
            this.minValue = min;
            this.salePercentage = salePercentage;   
        }

        public string GetID()
        {
            return this.policyID;
        }

        public double ApplyPolicy(double price, int quantity)
        {
            if (ValidatePolicy(price, quantity))
                return price / 100 * salePercentage;
            else
                return -1;
        }

        public Boolean ValidatePolicy(double price, int quantity)
        {
            if (quantity >= minValue && quantity <= maxValue)
                return true;
            else
                return false;
        }
    }
}