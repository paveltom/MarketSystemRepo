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
    public class PredicateAnd : Predicate
    {
        public PredicateAnd(List<Predicate> formula) : base(formula) { }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            return this.Formula.All(x => x.Satisfies(quantity, attributess));
        }
    }
}
