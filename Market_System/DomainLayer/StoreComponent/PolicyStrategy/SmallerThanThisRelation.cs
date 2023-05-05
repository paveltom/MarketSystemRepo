using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;
using System.Linq;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class SmallerThanThisRelation : Statement
    {

        public String FocusAttributeName { get; private set; }

        public String FocusAttributeValue { get; private set; }
        private bool userIndicatore = false;

        public SmallerThanThisRelation(string GreaterAttributeName, string GreaterAttributeValue, bool userAttribute) 
        {
            FocusAttributeName = GreaterAttributeName;
            FocusAttributeValue = GreaterAttributeValue;
            if(userAttribute)
                userIndicatore = true;
        }


        public override Boolean Satisfies(List<ItemDTO> choseProductsWithAttributes, Dictionary<string, string> userData)
        {
            if (userIndicatore)
                return userData[FocusAttributeName].CompareTo(FocusAttributeValue) < 0;
            else
                return choseProductsWithAttributes[0].GetStringValuesDict()[FocusAttributeName].CompareTo(FocusAttributeValue) < 0;
        }

        private String GetValue(ItemDTO item, string attribute)
        {
            string value = "";
            item.GetStringValuesDict().TryGetValue(attribute, out value);
            return value;
        }

    }

}