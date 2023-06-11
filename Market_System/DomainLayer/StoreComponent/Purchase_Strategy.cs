using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.UserComponent;
using static System.Net.Mime.MediaTypeNames;

namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Implement this as chain of responsibility
    public class Purchase_Strategy
    {
        public string StrategyID { get; private set; }
        public string StrategyName { get; private set; }
        public string Description { get; private set; }
        public Statement StrategyFormula { get; private set; }
        public string strFormula { get; private set; }


        /*
        public Purchase_Strategy(string stratID, string stratName, string description, Statement formula)
        {
            this.StrategyID = stratID;
            this.StrategyName = stratName;
            this.Description = description;
            this.StrategyFormula = formula;
        }
        */

        public Purchase_Strategy(string stratID, string stratName, string description, String formula)
        {
            this.StrategyID = stratID;
            this.StrategyName = stratName;
            this.Description = description;
            this.strFormula = formula;
            this.StrategyFormula = StatementBuilder.GenerateFormula(formula) ;
        }

        public void SetName(string name)
        {
            this.StrategyName = name;
        }

        public void SetDescription(string description)
        {
            this.Description = description;
        }

        public void SetFormula(Statement formula)
        {
            this.StrategyFormula = formula;
        }

        public Boolean Validate(List<ItemDTO> chosenProductsWithAttributes, string userID)
        {
            User currUser = UserFacade.GetInstance().getUser(userID);
            Dictionary<string, string> rellevantUserPolicyData = new Dictionary<string, string>();
            if (currUser != null)
                rellevantUserPolicyData = new Dictionary<string, string>
                {
                    { "Username", currUser.GetUsername() },
                    { "Address", currUser.get_Address() },
                    { "Age" , "17" }
                };
            else
                rellevantUserPolicyData = new Dictionary<string, string>
                {
                    { "Username", userID },
                    { "Address", "" },
                    { "Age" , "17" }
                };
            return this.StrategyFormula.Satisfies(chosenProductsWithAttributes, rellevantUserPolicyData);
        }

        public override string ToString()
        {
            return this.StrategyID + ": " + this.StrategyName + ". " + this.Description;
        }
    }
}