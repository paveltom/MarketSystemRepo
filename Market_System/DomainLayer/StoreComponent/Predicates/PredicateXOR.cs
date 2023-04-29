using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;

namespace Market_System.DomainLayer.StoreComponent
{
    public class PredicateXOR : Predicate
    {
        public PredicateXOR() { }

        public override Boolean Satisfies(int quantity, List<string> attributes)
        {
            // go over array of predicates - validate only one is true
            return false;
        }
        public override double ImplementSale(int quantity, List<string> attributes, double initPrice)
        {
            // if Satisfies() -> implement sale percentage and return the prce
            return 0;
        }
    }
}