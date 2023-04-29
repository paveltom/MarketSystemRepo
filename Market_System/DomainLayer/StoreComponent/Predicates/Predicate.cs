using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;

namespace Market_System.DomainLayer.StoreComponent.Predicates
{
    public abstract class Predicate : IPredicate
    {
        public Predicate[] Formula { get; protected set; }
        public double SalePercentage { get; protected set; }
        public string AttributeToValidate { get; protected set; }


        public Predicate()
        {

        }

        public void SetFormula(Predicate[] newFormula)
        {
            this.Formula = newFormula;
        }
        public abstract Boolean Satisfies(int quantity, List<string> attributes);
        public abstract double ImplementSale(int quantity, List<string> attributes, double initPrice);

    }
}