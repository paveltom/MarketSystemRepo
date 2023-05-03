using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class EqualRelation : Statement
    {

        public String FocusAttributeName { get; private set; }

        public String FocusAttributeValue { get; private set; }
        private bool userIndicator = false;

        public EqualRelation(string EqualAttributeName, string EqualAttributeValue, bool userAttribute)
        {
            FocusAttributeName = EqualAttributeName;
            FocusAttributeValue = EqualAttributeValue;
            if(userAttribute)
                userIndicator = true;
        }

        public override bool Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            if (userIndicator)
                return userData[FocusAttributeName].CompareTo(FocusAttributeValue) == 0;
            else
                return chosenItemsWithAttributes[0].GetStringValuesDict()[FocusAttributeName].CompareTo(FocusAttributeValue) == 0;
        }
    }

}