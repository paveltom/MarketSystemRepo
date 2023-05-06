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
    public class IfThenStatement : Statement
    {


        public IfThenStatement(Statement[] formula) : base(formula) { }

        public override Boolean Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            if (this.Formula[0].Satisfies(chosenItemsWithAttributes, userData)) 
            {
                if (this.Formula[1].Satisfies(chosenItemsWithAttributes, userData))
                    return true;
                else
                    return false;
            }
            return true;
        }
    }

}