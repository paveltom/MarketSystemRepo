using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public abstract class UserPolicy : Purchase_Policy
    {
        public String UserAttributeName { get; private set; }
        public UserPolicy(string polID, string polName, double salePercentage, string description, string userAttribute, Statement formula) :
            base(polID, polName, salePercentage, description, formula)
        { this.UserAttributeName = userAttribute; }

        public override List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes)
        {
            List<ItemDTO> saledItems = new List<ItemDTO>();
            foreach (var item in chosenProductsWithAttributes)
            {
                if (item.ProductCategory.CategoryName != this.UserAttributeName)
                    if (Validate(chosenProductsWithAttributes))
                        item.SetPrice(item.Price - item.Price / 100 * this.SalePercentage);
                saledItems.Add(item);

            }
            return saledItems;
        }

        public override Boolean Validate(List<ItemDTO> chosenProductsWithAttributes)
        {
            return this.SalePolicyFormula.Satisfies(chosenProductsWithAttributes);
        }
    }
}