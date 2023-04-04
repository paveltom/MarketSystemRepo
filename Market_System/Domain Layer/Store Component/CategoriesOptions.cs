using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Domain_Layer.Store_Component
{

    /**in this class there is all the categories exists as an option in all the market.
     * every product in the Market belongs to a category.
     * if a product added to the Market without sepcifying his category
     * it will have the 'other' category.
     * the admisnistartors of the Market can add or remove a category option.
     * a seller can request the admistators to add new category option if they have a product with no appropriate category option.
     */
    public class CategoriesOptions
    {
        private List<Category> categories;

        public List<Category> Categories
        {
            get { return categories; }
            set { categories = value; }
        }
        public CategoriesOptions()
        {
            categories = new List<Category>();
            categories.Add(new Category("Other"));
            categories.Add(new Category("Clothes"));
            categories.Add(new Category("Books"));
            categories.Add(new Category("Electronics"));
            categories.Add(new Category("For the baby"));
        }

        public void addCategoryOption(string categoryName)
        {
            Categories.Add(new Category(categoryName));
        }

        /*
         * public void addSubCategoryOption(string fatherCategory,string subcategoryName){ }
         */

        public void removeCategoryOption(string categoryName)      //TODO *** low priority function
        {
            throw new NotImplementedException();
            //changes all products in the system with 'categoryName' to 'other' option.
            Categories.Remove(new Category(categoryName));
        }

        public void requestToAddCategoryOption(string categoryName) //TODO *** low priority function
        {
            throw new NotImplementedException();
        }
    }
}
