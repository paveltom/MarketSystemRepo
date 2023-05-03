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
    public class AnyStatement : Statement
    {

        public int AtLeastQuantity { get; private set; }

        public AnyStatement(Statement[] formula) : base(formula) { }

        public override Boolean Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            foreach (ItemDTO item in chosenItemsWithAttributes)
            {
                if (this.Formula[0].Satisfies(new List<ItemDTO>() { item }, userData))
                    return true;
            }
            return false;
        }
    }

}