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
    public class PredicateSmallerOrGreater : Predicate
    {
        public Object Smaller { get; private set; }
        public Object Greater { get; private set; }
        public string SmallerName { get; private set; }
        public string GreaterName { get; private set; }

        
        public PredicateSmallerOrGreater() : base() 
        {
            this.Smaller = null;
            this.Greater = null;
            this.SmallerName = "";
            this.GreaterName = "";
        }

        public void SetSmaller(Object smaller)
        {
            this.Smaller = smaller;
        }

        public void SetGreater(Object greater)
        {
            this.Greater = greater;
        }

        public void SetSmallerName(string smaller)
        {
            this.SmallerName = smaller;
        }

        public void SetGreaterName(string greater)
        {
            this.GreaterName = greater;
        }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            string small;
            string great;
            if (SmallerName == "Quantity")
                Smaller = quantity;
            else if (this.Smaller == null && attributess.TryGetValue(SmallerName, out small))
                this.Smaller = small;
            if (GreaterName == "Quantity")
                this.Greater = quantity;
            else if (this.Greater == null && attributess.TryGetValue(GreaterName, out great))
                this.Greater = great;
            return Smaller.ToString().CompareTo(Greater.ToString()) < 0;
        }
    }
}
