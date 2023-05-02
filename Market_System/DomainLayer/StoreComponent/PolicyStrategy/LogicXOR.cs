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


        public override Boolean Satisfies(List<ItemDTO> choseProductsWithAttributes)
        {
            bool onlyOne = false;
            foreach(Statement s in this.Formula)
            {
                if (s.Satisfies(choseProductsWithAttributes))
                    if (onlyOne)
                        return false;
                    else
                        onlyOne = true;
            }
            return onlyOne;
        }

    }

}