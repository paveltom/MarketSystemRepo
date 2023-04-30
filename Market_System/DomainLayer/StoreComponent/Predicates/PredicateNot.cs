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
    public class PredicateNot : Predicate
    {
        public Predicate Statement { get; private set;}

        public PredicateNot() : base() { }

        public PredicateNot(List<Predicate> formula) : base(formula) 
        {
            this.Statement = formula[0];
        }

        public void SetStatement(Predicate stat)
        {
            this.Statement = stat;
        }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            return !this.Statement.Satisfies(quantity, attributess);
        }
    }
}

