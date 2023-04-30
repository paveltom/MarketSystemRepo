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

        public PredicateXOR(List<Predicate> formula) : base(formula) { }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            bool onlyOne = false;
            foreach (Predicate p in this.Formula)
            {
                if (p.Satisfies(quantity, attributess))
                    if (onlyOne)
                        return false;
            }
            return onlyOne;
        }
    }
}
