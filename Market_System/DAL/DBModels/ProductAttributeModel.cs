using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class ProductAttributeModel
    {
        [Key]
        public string AttributeID { get; set; } // = ProductID + AttributeName
        public string AttributeName { get; set; }
        public string AttributeOptions { get; set; } // string format: atr1_atr2_...

        public virtual ProductModel Product { get; set; }
    }
}