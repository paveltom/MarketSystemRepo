using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Product : Property
    {
        private int product_ID;

        //TODO:: add more to this class...
        public Product(int product_ID)
        {
            this.product_ID = product_ID;
        }
        //Implement all of the Property Methods here

        public int GetProductID()
        {
            return this.product_ID;
        }
    }
}