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
    public class PredicateSmaller : Predicate
    {
        public Object Smaller { get; private set; }
        public Object Greater { get; private set; }
        public string SmallerName { get; private set; }
        public string GreaterName { get; private set; }

        public PredicateSmaller() : base() 
        {
            this.Smaller = null;
            this.Greater = null;
        }

        public void SetSmaller(Object smaller)
        {
            this.Smaller = smaller;
        }

        public void SetGreater(Object greater)
        {
            this.Greater = greater;
        }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            string small;
            string great;
            if (this.Smaller == null && attributess.TryGetValue(SmallerName, out small))
                this.Smaller = small;
            if (this.Greater == null && attributess.TryGetValue(GreaterName, out great))
                this.Greater = great;
            return false;
        }
        public override double ImplementSale(int quantity, ConcurrentDictionary<string, string> attributess, double initPrice)
        {
            // if Satisfies() -> implement sale percentage and return the prce
            return 0;
        }
    }
}
