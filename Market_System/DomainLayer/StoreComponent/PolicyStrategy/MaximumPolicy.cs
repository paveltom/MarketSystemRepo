using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class MaximumPolicy : Purchase_Policy
    {
        public Dictionary<string, double> productsSale;
        public MaximumPolicy(string polID, string polName, string description, Dictionary<string, double> productsIDsAndSale) :
            base(polID, polName, 0, description, "")
        { this.productsSale = productsIDsAndSale; }

        public override List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes, string userid)
        {
            List<ItemDTO> saledItems = new List<ItemDTO>();
            string productWithHighestSaleValue = "";
            double maxSaleValue = 0;
            double newMaxSale = 0;
            double preSaleTotalPrice = 0;
            double calculatedPrice = 0;
            foreach (ItemDTO item in chosenProductsWithAttributes)
            {
                double currSale = 0;
                if (productsSale.TryGetValue(item.GetID(), out currSale))
                {
                    preSaleTotalPrice = item.Price * item.GetQuantity();
                    calculatedPrice = preSaleTotalPrice - preSaleTotalPrice / 100 * currSale;
                    if (preSaleTotalPrice - calculatedPrice > maxSaleValue)
                    {
                        maxSaleValue = preSaleTotalPrice - calculatedPrice;
                        newMaxSale = calculatedPrice;
                        productWithHighestSaleValue = item.GetID();
                    }
                }
            }

            foreach (ItemDTO item in chosenProductsWithAttributes)
            {
                ItemDTO currItem = item;
                if (productWithHighestSaleValue == currItem.GetID())
                    currItem.SetPrice(newMaxSale / currItem.GetQuantity());
                saledItems.Add(currItem);
            }

            return saledItems;

        }
    }
}