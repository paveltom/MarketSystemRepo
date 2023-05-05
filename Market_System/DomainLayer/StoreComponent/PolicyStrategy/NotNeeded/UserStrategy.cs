/*using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class UserStrategy : Purchase_Strategy
    {
        public String UserAttributeName { get; private set; }

        public UserStrategy(string polID, string polName, string description, string userAttribute, Statement formula) :
            base(polID, polName, description, formula)
        { this.UserAttributeName = userAttribute; }


        public override Boolean Validate(List<ItemDTO> chosenProductsWithAttributes)
        {
            throw new NotImplementedException();
        }

        public Boolean Validate(Dictionary<string, string> userData, List<ItemDTO> chosenProductsWithAttributes)
        {
            return this.StrategyFormula.Satisfies(userData[this.UserAttributeName]);
        }

        public override bool Validate(string value)
        {
            throw new NotImplementedException();
        }

        public override bool Validate(List<ItemDTO> chosenProductsWithAttributes, string userID)
        {
            throw new NotImplementedException();
        }
    }
}*/