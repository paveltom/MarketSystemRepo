using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility
    public class Purchase_Strategy
    {
        public string strategyID { get; private set; }
        public string strategyName { get; private set; }
        public string Description { get; private set; }

        public Purchase_Strategy(string stratID, string stratName)
        {
            this.strategyID = stratID;
            this.strategyName = stratName;
        }

        public string GetID()
        {
            return this.strategyID;
        }

        public Boolean validateProduct(int quantity, List<string> chosenAttributes)
        {
            // foreach attribute validate purchase restrictions
            return false;
        }

        public Boolean validateStore(int quantity, List<string> chosenAttributes)
        {
            // foreach attribute validate purchase restrictions
            return false;
        }

        public Boolean validateCategory(int quantity, List<string> chosenAttributes)
        {
            // foreach attribute validate purchase restrictions
            return false;
        }

        public Boolean validateMarket(int quantity, List<string> chosenAttributes)
        {
            // foreach attribute validate purchase restrictions
            return false;
        }
    }
}