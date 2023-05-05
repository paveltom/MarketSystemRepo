/*using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class CategoryStrategy : Purchase_Strategy
    {
        public String UserAttributeName { get; private set; }

        public CategoryStrategy(string polID, string polName, string description, Statement formula) : base(polID, polName, description, formula) { }


        public override Boolean Validate(List<ItemDTO> chosenProductsWithAttributes)
        {
            return this.StrategyFormula.Satisfies(chosenProductsWithAttributes);
        }

        public Boolean Validate(Dictionary<string, string> userData, List<ItemDTO> chosenProductsWithAttributes)
        {
            throw new NotImplementedException();

        }

        public override bool Validate(string value)
        {
            throw new NotImplementedException();
        }
    }
}*/