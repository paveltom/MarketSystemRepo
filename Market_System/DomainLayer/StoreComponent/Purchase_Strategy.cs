using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.UserComponent;

namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility
    public class Purchase_Strategy
    {
        public string StrategyID { get; private set; }
        public string StrategyName { get; private set; }
        public string Description { get; private set; }
        public Statement StrategyFormula { get; private set; }

        public Purchase_Strategy(string stratID, string stratName, string description, Statement formula)
        {
            this.StrategyID = stratID;
            this.StrategyName = stratName;
            this.Description = description;
            this.StrategyFormula = formula;
        }

        public void SetName(string name)
        {
            this.StrategyName = name;
        }

        public void SetDescription(string description)
        {
            this.Description = description;
        }

        public Boolean Validate(List<ItemDTO> chosenProductsWithAttributes, string userID)
        {
            User currUser = UserFacade.GetInstance().getUser(userID);
            Dictionary<string, string> rellevantUserPolicyData = new Dictionary<string, string>
                    {
                        { "username", currUser.GetUsername() },
                        { "address", currUser.get_Address() }
                    };
            return this.StrategyFormula.Satisfies(chosenProductsWithAttributes, rellevantUserPolicyData);
        }

        //public Boolean Validate(String value);


    }
}