using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.UserComponent;

namespace Market_System.DomainLayer.StoreComponent
{
    public class StatementBuilder
    {

        public StatementBuilder() { }

        public static Statement GenerateFormula(String formulaWithWhiteSpaces) // [ <formula> ]
        {
            if (formulaWithWhiteSpaces == "")
                return null;
            String formula = Regex.Replace(formulaWithWhiteSpaces, " ", "");
            String withoutBrakets = formula.Substring(1, formula.Length - 2);
            int continueIndex = withoutBrakets.IndexOf('[');
            // int endIndex = withoutBrakets.LastIndexOf("]");
            string statementType = withoutBrakets.Substring(0, continueIndex);
            string continueString = withoutBrakets.Substring(continueIndex, withoutBrakets.Length - continueIndex);
            switch (statementType)
            {
                case "ForAll":
                    return new ForAllStatement(new Statement[] { GenerateFormula(continueString) });

                case "Any":
                    return new AnyStatement(new Statement[] { GenerateFormula(continueString) });

                case "IfThen":
                    return new IfThenStatement(GetContinuousStatement(continueString));

                case "XOR":
                    return new LogicXOR(GetContinuousStatement(continueString));

                case "OR":
                    return new LogicOR(GetContinuousStatement(continueString));

                case "AND":
                    return new LogicAND(GetContinuousStatement(continueString));

                case "AtLeast":
                    int nextIndex = GetIndexOfNextStatement(continueString.Substring(1, continueString.Length - 2));
                    int quantity = Int32.Parse(continueString.Substring(2, nextIndex - 2)); // [[<Quantity>][<Statement>]] --- substr ---> <Quantity>
                    return new AtLeastStatement(quantity, new Statement[] { GenerateFormula(continueString.Substring(nextIndex + 1)) });

                case "AtMost":
                    int nextIndexAtMost = GetIndexOfNextStatement(continueString.Substring(1, continueString.Length - 2));
                    int quantityAtMost = Int32.Parse(continueString.Substring(1, nextIndexAtMost - 1)); // [<Quantity>][<Statement>] --- substr ---> <Quantity>
                    return new AtMostStatement(quantityAtMost, new Statement[] { GenerateFormula(continueString.Substring(nextIndexAtMost)) });

                case "Equal":
                    List<string> listEqual = GetRelationParams(continueString);
                    return new EqualRelation(listEqual[0], listEqual[1], Boolean.Parse(listEqual[2]), Boolean.Parse(listEqual[3]));

                case "SmallerThan":
                    List<string> listSmaller = GetRelationParams(continueString);
                    return new SmallerThanThisRelation(listSmaller[0], listSmaller[1], Boolean.Parse(listSmaller[2]), Boolean.Parse(listSmaller[3]));

                case "GreaterThan":
                    List<string> listGreater = GetRelationParams(continueString);
                    return new GreaterThanThisRelation(listGreater[0], listGreater[1], Boolean.Parse(listGreater[2]), Boolean.Parse(listGreater[3]));
            }

            throw new Exception("Bad formula.");
        }

        public static int GetIndexOfNextStatement(String formula)
        {
            int equality = 0;
            if (formula[0] == '[')
            {
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
            else
                return -1;
            if (equality != 0)
                return -1;
            return 0;
        }

        public static Statement[] GetContinuousStatement(String formulaWithBrackets)
        {
            String formula = formulaWithBrackets.Substring(1, formulaWithBrackets.Length - 2); // [ [] [] ] ---> [] []
            int nextIndex = GetIndexOfNextStatement(formula);
            List<Statement> statements = new List<Statement>();
            while (nextIndex > 0)
            {
                int length = nextIndex;
                string generate = formula.Substring(0, length);
                formula = formula.Substring(nextIndex);
                statements.Add(GenerateFormula(generate));
                nextIndex = GetIndexOfNextStatement(formula);

            }
            if (nextIndex < 0)
                throw new Exception("Brackets ineqality");
            statements.Add(GenerateFormula(formula));
            return statements.ToArray();
        }

        public static List<string> GetRelationParams(string formula) // [ [] [] ]
        {
            string continueString = formula.Substring(1, formula.Length - 2); // [ [] [] ] ---> [] []
            int equalNameLastIndex = GetIndexOfNextStatement(continueString);
            string equalName = continueString.Substring(1, equalNameLastIndex - 2); // without brackets
            bool userAttribute = equalName.Substring(0, 4) == "User";
            bool productAttribute = false;
            if(equalName.Length > 9)
                productAttribute = equalName.Substring(0, 9) == "Attribute";
            if (userAttribute)
                equalName = equalName.Substring(5);
            else if(productAttribute)
                equalName = equalName.Substring(10);
            continueString = continueString.Substring(equalNameLastIndex);
            string equalValue = continueString.Substring(1, continueString.Length - 2); // without brackets
            return new List<string>() { equalName, equalValue, userAttribute.ToString(), productAttribute.ToString() };
        }


        // ====================== Legend ======================
        // * Statements: [<Statement>]
        // ForAll: ForAll[<Statement>]
        // Any: Any[<Statement>]]
        // IfThen: IfThen[[<Statement>][<Statement>]]
        // XOR: XOR[[<Statement>][<Statement>]...]
        // OR: OR[[<Statement>][<Statement>]...]
        // AND: AND[[<Statement>][<Statement>]...]
        // AtLeast: AtLeast[[<Quantity>][<Statement>]]
        // AtMost: AtMost[[<Quantity>][<Statement>]]
        // Equal: Equal[[<StringAttributeName>][<StringAttributeValue>]]
        // SmallerThan: SmallerThan[[<StringAttributeName>][<StringAttributeValue>]]
        // GreaterThan: GreaterThan[[<StringAttributeName>][<StringAttributeValue>]]

        // * StringAttributeName:
        // Product attribute: Category, StoreID, Quantity...
        // User attribute: User.Age, User.Address...
        // ===================================================



        // STORE STRATEGY EXAMPLE: ForAll[IfThen[[Equals[Category.<SomeCategory>]][GreaterThan.UserAge.<SomeAge>]]]

        // ForAll[  -  implemented to every item that sent 
        //      IfThen[
        //          if:   [Equals[Category.<SomeCategory>]]  -  AttributeType, AttributeValue
        //          then: [GreaterThan[UserAge.<SomeAge>]]  -  then
        //            ]
        //       ]




    }
}