using System;
using System.Collections.Generic;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Market_System.DomainLayer.StoreComponent.PolicyStrategy
{
    public class EqualRelation : Statement
    {

        public String FocusAttributeName { get; private set; }

        public String FocusAttributeValue { get; private set; }
        private bool userIndicator;
        private bool productIndicator;

        public EqualRelation(string EqualAttributeName, string EqualAttributeValue, bool userAttribute, bool productAttribute)
        {
            FocusAttributeName = EqualAttributeName;
            FocusAttributeValue = EqualAttributeValue;
            userIndicator = userAttribute;
            productIndicator = productAttribute;            
        }

        public override bool Satisfies(List<ItemDTO> chosenItemsWithAttributes, Dictionary<string, string> userData)
        {
            if (userIndicator)
                return userData[FocusAttributeName].CompareTo(FocusAttributeValue) == 0;
            else if (productIndicator)
            {
                String attributes = chosenItemsWithAttributes[0].GetStringValuesDict()["Attributes"];
                Dictionary<string, string> coupleParsing = attributes.Substring(0, attributes.LastIndexOf(';')).Split(';').ToDictionary(s => s.Split('_')[0], s => s.Split('_')[1]);
                return coupleParsing[FocusAttributeName].CompareTo(FocusAttributeValue) == 0;
            }
            else
                return chosenItemsWithAttributes[0].GetStringValuesDict()[FocusAttributeName].CompareTo(FocusAttributeValue) == 0;
        }
    }

}