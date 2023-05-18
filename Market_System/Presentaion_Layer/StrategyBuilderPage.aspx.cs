using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("dynamicDDN")).ToList();
                // create dictionary allkeys by length
                Dictionary<int, List<string>> allkeys = new Dictionary<int, List<string>>();
                foreach(string k in keys)
                {
                    int index = LenOfIDNumber(k);
                    if (allkeys.ContainsKey(index))
                        allkeys[index].Add(k);
                    else
                        allkeys.Add(index, new List<string>() { k });
   
                    
                }

                GenereateChildren(-50, allkeys, "");
            }
        }

        private void GenereateChildren(int newPad, Dictionary<int, List<string>> allkeys, string parentID)
        {
            int parentIDNumLen = 0;
            if (parentID.Length > 0)
                parentIDNumLen = LenOfIDNumber(parentID);
            List<string> keys = allkeys[parentIDNumLen + 1].Where(x => x.Contains(parentID)).ToList();
            for (int i = 0; i < keys.Count; i++)
            {                

                int indexOfDDNString = keys[i].IndexOf("dynamicDDN");
                string currID = keys[i].Substring(indexOfDDNString);
                indexOfDDNString += 10; // index of id
                string currIDNumber = keys[i].Substring(indexOfDDNString);

                DropDownNode statementTree = new DropDownNode();
                statementTree.padding = newPad + 50;
                statementTree.ID = currID;

                HtmlGenericControl newdiv = new HtmlGenericControl();
                newdiv.Style.Add("padding-left", statementTree.padding + "px");

                if (keys[i].Contains("inputdynamicDDN"))
                {
                    statementTree.ID = "input" + currID;
                    statementTree.Visible = false;
                    HtmlInputText inputRelation = new HtmlInputText();
                    statementTree.inputValue = inputRelation;

                    inputRelation.Visible = true;
                    inputRelation.ID = "actualInput" + statementTree.ID;
                    inputRelation.Attributes.Add("placeholder", "Enter your value");
                    inputRelation.Attributes.Add("type", "text");
                    newdiv.Controls.Add(inputRelation);
                }
                else
                {
                    statementTree.AutoPostBack = true;
                    statementTree.Attributes.Add("AutoPostBack", "True");
                    statementTree.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
                    statementTree.EnableViewState = true;
                    if (keys[i].Contains("atrs" + currID))
                    {
                        statementTree.DataSource = WebStatement.attributes;
                        statementTree.ID = "atrs" + currID;
                    }
                    else
                        statementTree.DataSource = WebStatement.typeMap.Keys;
                    statementTree.DataBind();
                    statementTree.Items.Insert(0, new ListItem("--SELECT--"));
                }               

                newdiv.Controls.Add(statementTree);
                statementTree.myContainer = newdiv;

                MainDiv.Controls.Add(newdiv);
                MainDiv.Controls.Add(new LiteralControl("<br />"));
                if(allkeys.ContainsKey(parentIDNumLen + 2) && allkeys[parentIDNumLen + 2].Any(x => x.Contains(currID)))
                    GenereateChildren(statementTree.padding, allkeys, currID);

            }
        }

        private int LenOfIDNumber(string id)
        {
            if (id.Length < 10)
                return 0;
            int index = id.IndexOf("dynamicDDN") + 10;
            string sub = id.Substring(index);
            return sub.Length;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // view some info as in drawio
                this.StoreID = Request.QueryString["store_id"];


                DropDownNode statementTree = new DropDownNode();
                statementTree.padding = 0;
                statementTree.ID = "dynamicDDN1";
                statementTree.AutoPostBack = true;
                statementTree.Attributes.Add("AutoPostBack", "True");
                statementTree.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
                statementTree.EnableViewState = true;
                statementTree.DataSource = WebStatement.typeMap.Keys;
                statementTree.DataBind();
                statementTree.Items.Insert(0, new ListItem("--SELECT--"));



                HtmlGenericControl newdiv = new HtmlGenericControl();
                newdiv.Style.Add("padding-left", 0 + "px");

                newdiv.Controls.Add(statementTree);
                MainDiv.Controls.Add(newdiv);
                MainDiv.Controls.Add(new LiteralControl("<br />"));
                return;
            }
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
                    Placement(new DropDownNode[] { c1, c2 }, WebStatement.typeMap.Keys.ToList(), currNode);
                    currNode.node.ChildNodes.Add(c1.node);
                    currNode.node.ChildNodes.Add(c2.node);
                    return;

                case "Any":
                case "ForAll":
                    DropDownNode c = new DropDownNode();
                    Placement(new DropDownNode[] { c }, WebStatement.typeMap.Keys.ToList(), currNode);
                    currNode.node.ChildNodes.Add(c.node);
                    return;

                case "AtLeast":
                case "AtMost":
                    HtmlInputText input = new HtmlInputText();
                    DropDownNode inputDDN = new DropDownNode();
                    inputDDN.inputValue = input;
                    DropDownNode c3 = new DropDownNode();
                    Placement(new DropDownNode[] { inputDDN, c3 }, WebStatement.typeMap.Keys.ToList(), currNode);
                    currNode.node.ChildNodes.Add(c3.node);
                    return;

                case "Equal":
                case "SmallerThan":
                case "GreaterThan":
                    HtmlInputText inputRelation = new HtmlInputText();
                    DropDownNode inputDDNRelation = new DropDownNode();
                    inputDDNRelation.inputValue = inputRelation;
                    DropDownNode c4 = new DropDownNode();
                    Placement(new DropDownNode[] { inputDDNRelation, c4 }, WebStatement.attributes, currNode);
                    currNode.node.ChildNodes.Add(c4.node);
                    return;

            }

        }


        private void Placement(DropDownNode[] placeUs, List<string> data, DropDownNode parent)
        {
            string indexDDN = parent.ID;
            int newPad = parent.padding + 50;
            int counter = 1;
            
            foreach (DropDownNode placeMe in placeUs)
            {

                HtmlGenericControl newdiv = new HtmlGenericControl();
                newdiv.Style.Add("padding-left", newPad + "px");

                placeMe.ID = indexDDN + counter;                
                placeMe.padding = newPad;
                if (placeMe.inputValue != null)
                {
                    placeMe.ID = "input" + placeMe.ID;
                    placeMe.Visible = false;
                    HtmlInputText inputRelation = new HtmlInputText();
                    inputRelation.Visible = true;
                    inputRelation.ID = "actualInput" + placeMe.ID;
                    inputRelation.Attributes.Add("placeholder", "Enter your value");
                    inputRelation.Attributes.Add("type", "text");
                    placeMe.inputValue = inputRelation;
                    newdiv.Controls.Add(inputRelation);
                }
                else
                {
                    placeMe.AutoPostBack = true;
                    placeMe.EnableViewState = true;                    
                    placeMe.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
                    placeMe.DataSource = data;
                    placeMe.DataBind();
                    placeMe.Items.Insert(0, new ListItem("--SELECT--"));
                    if (data.Contains("---Attributes---"))
                        placeMe.ID = "atrs" + placeMe.ID;
                }
                newdiv.Controls.Add(placeMe);
                placeMe.myContainer = newdiv;

                int parentDivIndex = MainDiv.Controls.IndexOf(parent.myContainer);
                MainDiv.Controls.AddAt(parentDivIndex + 2*counter, newdiv);
                MainDiv.Controls.AddAt(parentDivIndex + 2*counter + 1, new LiteralControl("<br />"));


                counter++;
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
                                                                                            "Sale", "Description", "Rating", "Weight",  "TimesBought", "Category",  "---Attributes---" };


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

        public class DropDownNode : DropDownList
        {
            public TreeNode node;
            public HtmlInputText inputValue = null;
            public HtmlGenericControl myContainer;
            public int padding;

            public DropDownNode() : base()
            {
                node = new TreeNode();
            }


            public DropDownNode(string type) : base()
            {
                node = new TreeNode(type);
            }

        }
    }

}




/*        private void RestorePage()
        {

            List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("dynamicPanel")).ToList();
            List<string> keys2 = Request.Form.AllKeys.Where(key => key.Contains("MainContent")).ToList();

            string[] panel1 = Request.Form.GetValues(keys2[0]);

            //int i = 1;
            foreach (string key in keys)
            {
                string[] panel = Request.Form.GetValues(key);
            }
        }*/




/*protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {

        // view some info as in drawio
        this.StoreID = Request.QueryString["store_id"];

        this.MainPanel = new Panel();
        this.MainPanel.Style.Add("padding-left", "0px");
        MainPanel.EnableViewState = true;
        this.MainDiv.Controls.Add(MainPanel);


        Panel newPanel = new Panel();
        newPanel.Attributes.Add("runat", "server");
        newPanel.Style.Add("padding-left", "50px");
        newPanel.EnableViewState = true;

        int indexDDN = MainPanel.Controls.OfType<Panel>().ToList().Count + 1;

        DropDownNode statementTree = new DropDownNode();
        statementTree.myContainer = newPanel;
        statementTree.padding = 0;
        statementTree.ID = "dynamicDDN" + indexDDN;
        statementTree.AutoPostBack = true;
        statementTree.Attributes.Add("AutoPostBack", "True");
        statementTree.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
        statementTree.EnableViewState = true;
        statementTree.DataSource = WebStatement.typeMap.Keys;
        statementTree.DataBind();
        statementTree.Items.Insert(0, new ListItem("--SELECT--"));
        newPanel.Controls.Add(statementTree);
        //MainPanel.ContentTemplateContainer.Controls.Add(new LiteralControl("<br />"));

        MainPanel.Controls.Add(newPanel);
        MainPanel.Controls.Add(new LiteralControl("<br />"));
        Session.Add("MainPanel", MainPanel);
        return;
    }*/
//RestorePage();






/*private void RecreatePage(DropDownNode currDDN, Panel addInThis)
{
    TreeNode currNode = currDDN.node;
    string type = currNode.Text;
    switch (type)
    {
        case "OR":
        case "AND":
        case "XOR":
        case "IfThen":
            DropDownNode c1 = new DropDownNode();
            DropDownNode c2 = new DropDownNode();
            Placement(new DropDownNode[] { c1, c2 }, WebStatement.typeMap.Keys, currDDN);
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

}*/