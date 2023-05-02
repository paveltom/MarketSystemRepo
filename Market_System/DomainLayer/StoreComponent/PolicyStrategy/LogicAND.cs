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
    public class LogicAND : Statement
    {


        public override Boolean Satisfies(List<ItemDTO> choseProductsWithAttributes)
        {
            return this.Formula.All(x => x.Satisfies(choseProductsWithAttributes));
        }

    }

}