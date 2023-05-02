using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public abstract class GreaterRelation : Statement
    {

        public String FocusAttributeName { get; private set; }

        public String FocusAttributeValue { get; private set; }

        public GreaterRelation(string SmallerAttributeName, string SmallerAttributeValue)
        {
            FocusAttributeName = SmallerAttributeName;
            FocusAttributeValue = SmallerAttributeValue;
        }


        public override Boolean Satisfies(List<ItemDTO> choseProductsWithAttributes)
        {
            return GetValue(choseProductsWithAttributes[0], FocusAttributeName).CompareTo(FocusAttributeValue) > 0;
        }

        private String GetValue(ItemDTO item, string attribute)
        {
            string value = "";
            item.GetStringValuesDict().TryGetValue(attribute, out value);
            return value;
        }

    }

}