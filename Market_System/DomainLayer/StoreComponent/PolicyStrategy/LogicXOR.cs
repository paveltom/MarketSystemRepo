using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class LogicXOR : Statement
    {
        public LogicXOR(Statement[] formula) : base(formula) { }
        public override bool Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            bool onlyOne = false;
            foreach (Statement s in this.Formula)
            {
                if (s.Satisfies(chosenItemsWithAttributes, userData))
                    if (onlyOne)
                        return false;
                    else
                        onlyOne = true;
            }
            return onlyOne;
        }
    }

}