/*using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class ProductStrategy : Purchase_Strategy
    {
        public ProductStrategy(string polID, string polName, string description, Statement formula) : base(polID, polName, description, formula) { }


        public override Boolean Validate(List<ItemDTO> chosenProductsWithAttributes) // list of size 1
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