using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;
using System.Linq;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class AtLeastStatement : Statement
    {
        
        public int AtLeastQuantity { get; private set; }

        public AtLeastStatement(int atLeastQuantity, Statement[] formula) : base(formula)
        {
            AtLeastQuantity = atLeastQuantity;
        }

        public override Boolean Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            int counter = 0;
            foreach(ItemDTO item in chosenItemsWithAttributes)
            {
                if(this.Formula[0].Satisfies(new List<ItemDTO>() { item }, userData))
                    counter++;
            }

            return counter >= this.AtLeastQuantity;
        }
    }

}