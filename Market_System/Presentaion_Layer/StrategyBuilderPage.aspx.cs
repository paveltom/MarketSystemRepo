using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Market_System.Presentaion_Layer
{
    public partial class StrategyBuilderPage : System.Web.UI.Page
    {
        public string StoreID;
        public string ProductID;

        private DropDownNode statementTree;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            // view some info as in drawio
            this.StoreID = Request.QueryString["store_id"];
            MainPanel.Controls.Remove(StatementDLL);

            Panel newPanel = new Panel();
            newPanel.Attributes.Add("runat", "server");
            newPanel.Style.Add("padding-left", "50px");

            this.statementTree = new DropDownNode();
            statementTree.padding = 0;
            statementTree.myInnerPanel = newPanel;
            statementTree.AutoPostBack = true;
            statementTree.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
            statementTree.DataSource = WebStatement.typeMap.Keys;
            statementTree.DataBind();
            statementTree.Items.Insert(0, new ListItem("--SELECT--"));
            
            MainPanel.Controls.Add(statementTree);
            MainPanel.Controls.Add(new LiteralControl("<br />"));
            MainPanel.Controls.Add(statementTree.myInnerPanel);
            MainPanel.Controls.Add(new LiteralControl("<br />"));

        }

        protected void StatementDLL_SelectedIndexChanged(object sender, EventArgs e)
        {            
            DropDownNode ddlSender = sender as DropDownNode;
            string selected = ddlSender.SelectedValue;
            UpdateStatementTree(ddlSender, selected);

        }

        private void UpdateStatementTree(DropDownNode currNode, string type)
        {
            currNode.node.Text = type;
            switch (type)
            {
                case "OR":
                case "AND":
                case "XOR":
                case "IfThen":
                    DropDownNode c1 = new DropDownNode();
                    DropDownNode c2 = new DropDownNode();
                    Placement(new DropDownNode[] { c1, c2}, WebStatement.typeMap.Keys, currNode);
                    currNode.node.ChildNodes.Add(c1.node);
                    currNode.node.ChildNodes.Add(c2.node);
                    return;

                case "Any":
                case "ForAll":
                    DropDownNode c = new DropDownNode();
                    Placement(new DropDownNode[] { c }, WebStatement.typeMap.Keys, currNode);
                    currNode.node.ChildNodes.Add(c.node);
                    return;
                
                case "AtLeast":
                case "AtMost":
                    HtmlInputText input = new HtmlInputText();
                    // place input accordingly!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    DropDownNode c3 = new DropDownNode();
                    c3.inputValue = input;
                    Placement(new DropDownNode[] { c3 }, WebStatement.typeMap.Keys, currNode);
                    currNode.node.ChildNodes.Add(c3.node);
                    return;

                case "Equal":
                case "SmallerThan":
                case "GreaterThan":
                    HtmlInputText inputRelation = new HtmlInputText();
                    // place input accordingly!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    DropDownNode c4 = new DropDownNode();
                    c4.inputValue = inputRelation;
                    Placement(new DropDownNode[] { c4 }, WebStatement.attributes, currNode);
                    currNode.node.ChildNodes.Add(c4.node);
                    return;


            }

        }


        private void Placement(DropDownNode[] placeUs, object data, DropDownNode parent)
        {
            // Stataement placement
            int padding = parent.padding + 50;
            foreach (DropDownNode placeMe in placeUs)
            {
                Panel newPanel = new Panel();
                newPanel.Attributes.Add("runat", "server");
                newPanel.Style.Add("padding-left", "50px");
                placeMe.myInnerPanel = newPanel;
                placeMe.AutoPostBack = true;
                placeMe.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
                placeMe.DataSource = data;
                placeMe.DataBind();
                statementTree.Items.Insert(0, new ListItem("--SELECT--"));

                parent.myInnerPanel.Controls.Add(placeMe);
                parent.myInnerPanel.Controls.Add(new LiteralControl("<br />"));
                parent.myInnerPanel.Controls.Add(placeMe.myInnerPanel);
                parent.myInnerPanel.Controls.Add(new LiteralControl("<br />"));
            }

            // Attribute placement

        }

        public class DropDownNode : DropDownList
        {
            public TreeNode node;
            public HtmlInputText inputValue;
            public int padding;
            public Panel myInnerPanel;

            public DropDownNode() : base()
            {
                node = new TreeNode();
            }


            public DropDownNode(string type) : base()
            {
                node = new TreeNode(type);
            }

        }




        public class WebStatement
        {
            public string myType;
            public List<WebStatement> myStatements;

            public static Dictionary<string, string> typeMap = new Dictionary<string, string>()
            {
                {"Any","[Any[<Statement>]]"},
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
            };

            public static List<string> attributes = new List<string>(){"User.Age", "User.Address", "User.Name", "StoreID", "ItemID", "Quantity", "ReservedQuantity", "Price", "Name",
                                                                                            "Sale", "Description", "Rating", "Weight",  "TimesBought", "Category",  "Attributes" };


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