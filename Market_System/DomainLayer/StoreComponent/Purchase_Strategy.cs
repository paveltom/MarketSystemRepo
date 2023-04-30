using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent.Predicates;


namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility
    public class Purchase_Strategy
    {
        public string StrategyID { get; private set; }
        public string StrategyName { get; private set; }
        public string Description { get; private set; }
        public Predicate StrategyFormula { get; private set; }

        public Purchase_Strategy(string stratID, string stratName, string description)
        {
            this.StrategyID = stratID;
            this.StrategyName = stratName;
            this.Description = description;
        }

        public void SetName(string name)
        {
            this.StrategyName = name;
        }

        public void SetDescription(string description)
        {
            this.Description = description;
        }


        public Boolean ValidateProduct(int quantity, List<string> chosenAttributes)
        {
            throw new NotImplementedException();

        }

        public Boolean ValidateStore(int quantity, List<string> chosenAttributes)
        {
            throw new NotImplementedException();

        }

        public Boolean ValidateCategory(int quantity, List<string> chosenAttributes)
        {
            throw new NotImplementedException();

        }







        public Boolean ValidateMarket(int quantity, List<string> chosenAttributes)
        {
            // foreach attribute validate purchase restrictions
            throw new NotImplementedException();
        }
    }
}