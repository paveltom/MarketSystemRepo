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
    public class PredicateOr : Predicate
    {

        public PredicateOr(List<Predicate> formula) : base(formula) { }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            return this.Formula.Any(x => x.Satisfies(quantity, attributess));
        }
    }
}
