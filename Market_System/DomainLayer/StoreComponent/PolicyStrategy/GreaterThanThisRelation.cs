using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class GreaterThanThisRelation : Statement
    {

        public String FocusAttributeName { get; private set; }

        public String FocusAttributeValue { get; private set; }
        private bool userIndicatore = false;

        public GreaterThanThisRelation(string SmallerAttributeName, string SmallerAttributeValue, bool userAttribute)
        {
            FocusAttributeName = SmallerAttributeName;
            FocusAttributeValue = SmallerAttributeValue;
            if (userAttribute)
                userIndicatore = true;
        }


        public override Boolean Satisfies(List<ItemDTO> choseProductsWithAttributes, Dictionary<string, string> userData)
        {
            if (userIndicatore)
                return userData[FocusAttributeName].CompareTo(FocusAttributeValue) > 0;
            else
                return choseProductsWithAttributes[0].GetStringValuesDict()[FocusAttributeName].CompareTo(FocusAttributeValue) > 0;
        }
    }

}