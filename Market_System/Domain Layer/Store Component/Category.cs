using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Category
    {
        private string categoryName;
        //private List<Category> subCategories;

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public Category(string name)
        {
            categoryName = name;
        }

    }
}
