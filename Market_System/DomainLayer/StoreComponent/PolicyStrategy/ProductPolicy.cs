using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class ProductPolicy : Purchase_Policy
    {
        public String SaledProductID { get; private set; }
        public ProductPolicy(string polID, string polName, double salePercentage, string description, string formula, string productID) :
            base(polID, polName, salePercentage, description, formula)
        { this.SaledProductID = productID; }

        public override List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes, string userid)
        {
            if (Validate(chosenProductsWithAttributes, userid))
            {
                List<ItemDTO> saledItems = new List<ItemDTO>();
                foreach (ItemDTO item in chosenProductsWithAttributes)
                {
                    if (item.GetID() == this.SaledProductID)
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