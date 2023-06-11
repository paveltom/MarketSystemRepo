using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class CategoryPolicy : Purchase_Policy
    {
        public String SaledCategoryName { get; private set; }
        public CategoryPolicy(string polID, string polName, double salePercentage, string description, string category, String formula) :
            base(polID, polName, salePercentage, description, formula)
        { this.SaledCategoryName = category; }

        public override List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes)
        {
            if (Validate(chosenProductsWithAttributes, ""))
            {
                List<ItemDTO> saledItems = new List<ItemDTO>();
                foreach (var item in chosenProductsWithAttributes)
                {
                    if (item.ProductCategory.CategoryName == this.SaledCategoryName)
                        item.SetPrice(item.Price - item.Price / 100 * this.SalePercentage);
                    saledItems.Add(item);
                }
                return saledItems;
            }
            else
                return chosenProductsWithAttributes;
            
        }
    }
}