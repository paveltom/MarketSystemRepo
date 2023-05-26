using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class ProductModel
    {
        [Key]
        public int ProductID { get; set; }
        public virtual StoreModel Store { get; set; }
        public string StoreID { get; set; }
        public string Name { get; set; }



    }
}