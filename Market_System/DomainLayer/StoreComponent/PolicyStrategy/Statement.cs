using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public abstract class Statement
    {
        public Statement[] Formula { get; private set; }

        public Statement() { }

        public Statement(Statement[] formula) 
        {
            this.Formula = formula;
        }

        public abstract Boolean Satisfies(List<ItemDTO> chosenItemsWithAttributes);
        //public double ImplementSale(int quantity, ConcurrentDictionary<string, string> attributes, double initPrice);

    }

}