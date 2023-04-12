using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility.
    public class Purchase_Policy
    {
        private string policyID;
        private string policyName;

        public Purchase_Policy(string polID, string polName)
        {
            this.policyID = polID;
            this.policyName = polName;
        }

        public string GetID()
        {
            return this.policyID;
        }
    }
}