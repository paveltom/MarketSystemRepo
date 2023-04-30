using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent.Predicates;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;

namespace Market_System.DomainLayer.StoreComponent.Predicates
{
    public class PredicateIfThen : Predicate
    {
        public Predicate IfPart { get; private set; }
        public Predicate ThenPart { get; private set; }

        public PredicateIfThen() : base() { }

        public PredicateIfThen(List<Predicate> formula) : base(formula) 
        {
            this.IfPart = formula[0];
            this.ThenPart = formula[1];
        }

        public void SetIfPart(Predicate ifPred)
        {
            this.IfPart = ifPred;
        }

        public void SetThenPart(Predicate thenPred)
        {
            this.ThenPart = thenPred;
        }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            PredicateNot notIfPred = new PredicateNot(this.Formula);
            Predicate orBoth = new PredicateOr(new List<Predicate>() { notIfPred, this.ThenPart});
            return orBoth.Satisfies(quantity, attributess); 
        }
    }
}
