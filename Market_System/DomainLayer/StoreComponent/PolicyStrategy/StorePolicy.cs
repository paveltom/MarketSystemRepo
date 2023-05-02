using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public abstract class StorePolicy : Purchase_Policy
    {
        public String SaledStoreID { get; private set; }
        public StorePolicy(string polID, string polName, double salePercentage, string description, string storeID) :
            base(polID, polName, salePercentage, description)
        { this.SaledStoreID = storeID; }

        public override List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes)
        {
            List<ItemDTO> saledItems = new List<ItemDTO>();
            foreach (var item in chosenProductsWithAttributes)
            {
                if (item.StoreID == this.SaledStoreID)
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