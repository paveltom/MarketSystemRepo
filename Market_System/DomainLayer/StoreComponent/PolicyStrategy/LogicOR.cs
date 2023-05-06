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
    public class LogicOR : Statement
    {
        public LogicOR(Statement[] formula) : base(formula) { }
        public override bool Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            return this.Formula.Any(s => s.Satisfies(chosenItemsWithAttributes, userData));
        }
    }

}