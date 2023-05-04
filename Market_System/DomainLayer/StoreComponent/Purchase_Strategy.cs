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

        public Purchase_Strategy(string stratID, string stratName, string description, String formula)
        {
            this.StrategyID = stratID;
            this.StrategyName = stratName;
            this.Description = description;
            this.StrategyFormula = GenerateFormula(formula) ;
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
            Dictionary<string, string> rellevantUserPolicyData = new Dictionary<string, string>();
            if (currUser != null)
                rellevantUserPolicyData = new Dictionary<string, string>
                {
                    { "username", currUser.GetUsername() },
                    { "address", currUser.get_Address() }
                };
            else
                rellevantUserPolicyData = new Dictionary<string, string>
                {
                    { "username", userID },
                    { "address", "" }
                };
            return this.StrategyFormula.Satisfies(chosenProductsWithAttributes, rellevantUserPolicyData);
        }

        // store strategy exmpl: ForAll[IfThen[[Equals[Category.<SomeCategory>]][GreaterThan.UserAge.<SomeAge>]]]
        // ForAll[  -  implemented to every item that sent 
        //      IfThen[
        //          if:   [Equals[Category.<SomeCategory>]]  -  AttributeType, AttributeValue
        //          then: [GreaterThan[UserAge.<SomeAge>]]  -  then
        //            ]
        //       ]

        // ====================== Legend ======================
        // * Statements:
                // ForAll: ForAll[<Statement>]
                // Any: Any[<Statement>]]
                // IfThen: IfThen[[<Statement>][<Statement>]]
                // XOR: XOR[[<Statement>][<Statement>]...]
                // OR: OR[[<Statement>][<Statement>]...]
                // AND: AND[[<Statement>][<Statement>]...]
                // AtLeast: AtLeast[[<Quantity>][<Statement>]]
                // AtMost: AtMost[[<StringQuantity>][<Statement>]]
                // Equal: Equal[[<StringAttributeName>][<StringAttributeValue>]]
                // SmallerThan: SmallerThan[[<StringAttributeName>][<StringAttributeValue>]]
                // GreaterThan: GreaterThan[[<StringAttributeName>][<StringAttributeValue>]]

        // * StringAttributeName:
                // Product attribute: Category, StoreID, Quantity...
                // User attribute: User.Age, User.Address...
        // ===================================================
        public Statement GenerateFormula(String formula) // [ <formula> ]
        {
            
            String withoutBrakets = formula.Substring(1, formula.Length - 2);
            int continueIndex = withoutBrakets.IndexOf('[');
            // int endIndex = withoutBrakets.LastIndexOf("]");
            string statementType = withoutBrakets.Substring(0, continueIndex);
            string continueString = withoutBrakets.Substring(continueIndex, withoutBrakets.Length - continueIndex);
            switch (statementType) {
                case "ForAll": return new ForAllStatement(new Statement[] { GenerateFormula(continueString) });
                
                case "Any": return new AnyStatement(new Statement[] { GenerateFormula(continueString) });

                case "IfThen":
                    int index = GetIndexOfNextStatement(continueString);
                    return new IfThenStatement(new Statement[] { GenerateFormula(continueString.Substring(0, index)), GenerateFormula(continueString.Substring(index)) });

                case "XOR": return new LogicXOR(new Statement[] {. GenerateFormula(continueString) });

                case "OR": return new LogicOR(new Statement[] {. GenerateFormula(continueString) });

                case "AND": return new LogicAND(new Statement[] {. GenerateFormula(continueString) });

                case "AtLeast": return new AtLeastStatement(new Statement[] {. GenerateFormula(continueString) });

                case "AtMost": return new AtMostStatement(new Statement[] {. GenerateFormula(continueString) });

                case "Equal": return new EqualRelation(new Statement[] {. GenerateFormula(continueString) });

                case "SmallerThan": return new SmallerThanThisRelation(new Statement[] { .GenerateFormula(continueString) });

                case "GreaterThan": return new GreaterThanThisRelation(new Statement[] {. GenerateFormula(continueString) });
                
            }
              
            throw new NotImplementedException();
        }

        public int GetIndexOfNextStatement(String formula) 
        {
            if (formula[0] == '[')
            {
                int equality = 0;
                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == '[')
                        equality++;
                    else if (formula[i] == ']')
                        equality--;
                    if (equality == 0 && i != formula.Length - 1)
                        return i + 1;
                }
            }            
            return 0;
        }


        //public Boolean Validate(String value);


    }
}