using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Market_System.Presentaion_Layer
{
    public partial class StrategyBuilderPage : System.Web.UI.Page
    {
        public string StoreID;
        protected void Page_Load(object sender, EventArgs e)
        {
            // view some info as in drawio
            this.StoreID = Request.QueryString["store_id"];
            
        }

        private void BindStatement(WebStatement statement, TreeNode parentStatement)
        {
            
            /*foreach (WebStatement node in statement.nested)
            {
                TreeNode newNode = new TreeNode(node.myType, WebStatement.typeMap[node.myType]);                
                if (parentNode == null)
                {
                    StatementTree.Nodes.Add(newNode);
                }
                else
                {
                    parentNode.ChildNodes.Add(newNode);
                }
                BindStatement(list, newNode);
            }*/
        }


        protected void AddStatement(object sender, EventArgs e)
        {
            StatementTree = null;
            TreeNode node = sender as TreeNode;

            // get typeX from sender

            switch (node.Text)
            {
                case "OR":
                    node.ChildNodes.Add(new TreeNode());
                    node.ChildNodes.Add(new TreeNode());
                    return;

                default:
                    node.ChildNodes.Add(new TreeNode());
                    return;

            }


            // considering type create a WebStatement
            // send to BindStatement
        }



        public class DropDownNode : TreeNode
        {
            public DropDownList DDL;

            public DropDownNode() : base()
            {
                this.SelectAction = TreeNodeSelectAction.None;
                this.DDL = new DropDownList();
                DDL.DataSource = WebStatement.typeMap.Keys;
                DDL.DataBind();
            }

            public DropDownNode(string str) : base(str)
            {
                this.SelectAction = TreeNodeSelectAction.None;
                this.DDL = new DropDownList();
                DDL.DataSource = WebStatement.typeMap.Keys;
                DDL.DataBind();
            }

        }




        public class WebStatement
        {
            public string myType;
            public List<WebStatement> myStatements;

            public static Dictionary<string, string> typeMap = new Dictionary<string, string>()
            {
                {"Statement", "<Statement>"},
                {"AnyStatement","[Any[<Statement>]]"},
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
                {"ProductAttribute", ""}


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