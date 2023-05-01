using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;

namespace Market_System.DomainLayer.StoreComponent.Predicates
{
    interface IPredicate
    {
        public Boolean SatisfiesProduct(int quantity, ConcurrentDictionary<string, string> attributes);
        public Boolean SatisfiesStore(ConcurrentDictionary<string, string> chosenAttributes);

        public Boolean SatisfiesCategory(ConcurrentDictionary<string, string> chosenAttributes);

        // public double ImplementSale(int quantity, ConcurrentDictionary<string, string> attributes, double initPrice);
        
    }

}