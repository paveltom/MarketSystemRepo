using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
namespace Market_System.DomainLayer.StoreComponent
{
    interface IPredicate
    {
        public Boolean Satisfies(int quantity, List<string> attributes);
        public double ImplementSale(int quantity, List<string> attributes, double initPrice);
        
    }

}