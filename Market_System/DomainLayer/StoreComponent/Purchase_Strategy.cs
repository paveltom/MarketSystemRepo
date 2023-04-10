using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility
    public class Purchase_Strategy
    {
        private string strategyID;
        private string strategyName;

        public Purchase_Strategy(string stratID, string stratName)
        {
            this.strategyID = stratID;
            this.strategyName = stratName;
        }

        public string GetID()
        {
            return this.strategyID;
        }
    }
}