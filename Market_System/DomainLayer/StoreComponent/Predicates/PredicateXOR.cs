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
    public class PredicateXOR : Predicate
    {
        public PredicateXOR(Predicate[] formula) : base(formula) { }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            // go over array of predicates - validate only one is true
            return false;
        }
        public override double ImplementSale(int quantity, ConcurrentDictionary<string, string> attributess, double initPrice)
        {
            // if Satisfies() -> implement sale percentage and return the prce
            return 0;
        }
    }
}