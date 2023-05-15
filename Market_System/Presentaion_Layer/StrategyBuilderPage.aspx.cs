using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace Market_System.Presentaion_Layer
{
    public partial class StrategyBuilderPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // view some info as in drawio

        }

        private void BindStatement(IEnumerable<WebStatement> list, TreeNode parentStatement)
        {
            var nodes = list.Where(x => parentNode == null ? x.ParentId == 0 : x.ParentId == int.Parse(parentNode.Value));
            foreach (var node in nodes)
            {
                TreeNode newNode = new TreeNode(node.Name, node.Id.ToString());
                if (parentNode == null)
                {
                    treeView1.Nodes.Add(newNode);
                }
                else
                {
                    parentNode.ChildNodes.Add(newNode);
                }
                BindTree(list, newNode);
            }
        }

        public class WebStatement
        {
            public string myType;
            private WebStatement[] nested;

            public static Dictionary<string, string> typeMap = new Dictionary<string, string>()
            {
                {"AnyStatement","[Any[<Statement>]]" },
                {"ForAll","ForAll[<Statement>]"},
                {"IfThen", "IfThen[[<Statement>][<Statement>]]"},
                {"XOR", "XOR[[<Statement>][<Statement>]...]"},
                {"OR", "OR[[<Statement>][<Statement>]...]"},
                {"AND", "AND[[<Statement>][<Statement>]...]"},
                {"AtLeast", "AtLeast[[<Quantity>][<Statement>]]"},
                {"AtMost", "AtMost[[<StringQuantity>][<Statement>]]"},
                {"Equal", "Equal[[<StringAttributeName>][<StringAttributeValue>]]"},
                {"SmallerThan", "SmallerThan[[<StringAttributeName>][<StringAttributeValue>]]"},
                {"GreaterThan", "GreaterThan[[<StringAttributeName>][<StringAttributeValue>]]"},
                {"UserChar", ""},
                {"ProductAttribute", ""},


            };
            
            
            public WebStatement(string stateType) 
            {
                myType = stateType;
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
            // AtMost: AtMost[[<StringQuantity>][<Statement>]]
            // Equal: Equal[[<StringAttributeName>][<StringAttributeValue>]]
            // SmallerThan: SmallerThan[[<StringAttributeName>][<StringAttributeValue>]]
            // GreaterThan: GreaterThan[[<StringAttributeName>][<StringAttributeValue>]]

            // * StringAttributeName:
            // Product attribute: Category, StoreID, Quantity...
            // User attribute: User.Age, User.Address...
            // ===================================================

        }
    }
}