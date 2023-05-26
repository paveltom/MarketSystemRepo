using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class CategoryModel
    {
        public string CategoryName;

        public virtual ICollection<ProductModel> Products { get; set; }
    }
}