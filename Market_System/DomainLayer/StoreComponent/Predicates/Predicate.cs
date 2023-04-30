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
    public abstract class Predicate : IPredicate
    {
        public List<Predicate> Formula { get; protected set; }
        public string AttributeToValidate { get; protected set; }

        public Predicate()
        {
            this.Formula = new List<Predicate>();
        }
        
        public Predicate(List<Predicate> formula)
        {
            this.Formula = formula;
        }

        public abstract Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess);

        public void SetFormula(List<Predicate> newFormula)
        {
            this.Formula = newFormula;
        }

        public void AddPredicate(Predicate toAdd)
        {
            this.Formula.Add(toAdd);
        }

    }
}