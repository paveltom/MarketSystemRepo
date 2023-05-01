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
    public class PredicateEqual : Predicate
    {
        public Object First { get; private set; }
        public Object Second { get; private set; }
        public string FirstName { get; private set; }
        public string SecondName { get; private set; }

        public PredicateEqual() : base()
        {
            this.First = null;
            this.Second = null;
            this.FirstName = "";
            this.SecondName = "";
        }

        public void SetFirst(Object first)
        {
            this.First = first;
        }

        public void SetSecond(Object second)
        {
            this.Second = second;
        }

        public override Boolean Satisfies(int quantity, ConcurrentDictionary<string, string> attributess)
        {
            string small;
            string great;
            if (FirstName == "Quantity")
                First = quantity;
            else if (this.First == null && attributess.TryGetValue(FirstName, out small))
                this.First = small;
            if (SecondName == "Quantity")
                this.Second = quantity;
            else if (this.Second == null && attributess.TryGetValue(SecondName, out great))
                this.Second = great;
            return First.ToString().CompareTo(Second.ToString()) == 0;
        }
    }
}
