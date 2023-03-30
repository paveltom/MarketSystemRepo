using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Product : Property
    {
        private String product_ID; // composed of 2 parts: *storeId*_*inStoreProductId* - 1234_9876 for exmpl

        private String Name; 
        private String Catagory;
        private String Description;
        private double Price;
        private int Quantity;
        private int ReservedQuantity;
        private List<String> Comments;




        // =======Fields ToDo===========:
        // Price
        // Purchase_Policy productSpecificPolicy - ChainOfResponsibility 
        // Purchase_Strategy productSpecificStrategy - ChainOfResponsibility 
        // Quantity - maybe should be held by Store
        // ReservedQuantity
        // Description
        // Attributes -  Dictionary<String>: {atb1: opt1->opt2->op3...., atb2: opt1->opt2....,...}
        // Comments
        // Rating - mabye private class/enum or somthing simpler
        // Size (x*y*z)
        // Weight
        // Catagory (mabye will be implementing by composition design pattern to support a sub catagoring.)


        public Product(String product_ID)
        {
            this.product_ID = product_ID;
        }
        //Implement all of the Property Methods here

        public String GetProductID()
        {
            return this.product_ID;
        }

        // ========Methods ToDo==========:
        // getters and setters almost for each field
        // passing a data for store representation - probably ItemDTO
        // return price after sale appliement
        // 
        


    }

 
}