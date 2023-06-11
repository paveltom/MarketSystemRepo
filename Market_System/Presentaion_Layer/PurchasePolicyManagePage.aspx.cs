using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Market_System.Presentaion_Layer

// TODO(on it):
//      destruction on higher level selection
//      difference for product page or store page
{
    public partial class PurchasePolicyManagePage : System.Web.UI.Page
    {
        public string StoreID = "";
        public string ProductID = "";
        public TreeNode root;

        public Dictionary<string, string> typeMap = new Dictionary<string, string>()
            {
                {"Any","Any[<Statement>]"},
                {"ForAll","ForAll[<Statement>]"},
                {"IfThen", "IfThen[[<Statement>][<Statement>]]"},
                {"XOR", "XOR[[<Statement>][<Statement>]]"},
                {"OR", "OR[[<Statement>][<Statement>]]"},
                {"AND", "AND[[<Statement>][<Statement>]]"},
                {"AtLeast", "AtLeast[[<Quantity>][<Statement>]]"},
                {"AtMost", "AtMost[[<Quantity>][<Statement>]]"},
                {"Equal", "Equal[[<AttributeName>][<AttributeValue>]]"},
                {"SmallerThan", "SmallerThan[[<AttributeName>][<AttributeValue>]]"},
                {"GreaterThan", "GreaterThan[[<AttributeName>][<AttributeValue>]]"},
            };

        public List<string> attributes;


        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                this.attributes = GenerateAttributes();
                List<string> keys = Request.Form.AllKeys.Where(k => { if (k != null) return k.Contains("dynamicDDN"); return false; }).ToList();
                // create dictionary allkeys by length
                Dictionary<int, List<string>> allkeys = new Dictionary<int, List<string>>();
                foreach (string k in keys)
                {
                    int index = LenOfIDNumber(k);
                    if (allkeys.ContainsKey(index))
                        allkeys[index].Add(k);
                    else
                        allkeys.Add(index, new List<string>() { k });
                }

                GenereateChildren(-50, allkeys, "", null);
            }
        }

        private void GenereateChildren(int newPad, Dictionary<int, List<string>> allkeys, string parentID, DropDownNode parent)
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

                DropDownNode statementTree = new DropDownNode(Request.Form[keys[i]]);
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
                        statementTree.DataSource = this.attributes;
                        statementTree.ID = "atrs" + currID;
                    }
                    else
                        statementTree.DataSource = this.typeMap.Keys;
                    statementTree.DataBind();
                    statementTree.Items.Insert(0, new ListItem("--SELECT--"));
                }

                newdiv.Controls.Add(statementTree);
                statementTree.myContainer = newdiv;
                if (parent != null)
                    parent.node.ChildNodes.Add(statementTree.node);
                else
                    this.root = statementTree.node;

                MainDiv.Controls.Add(newdiv);
                MainDiv.Controls.Add(new LiteralControl("<br />"));
                if (allkeys.ContainsKey(parentIDNumLen + 2) && allkeys[parentIDNumLen + 2].Any(x => x.Contains(currID)))
                    GenereateChildren(statementTree.padding, allkeys, currID, statementTree);

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


            if (Request.QueryString.AllKeys.Contains("product_id"))
            {
                this.ProductID = Request.QueryString["product_id"];
                this.StoreID = this.ProductID.Substring(0, this.ProductID.IndexOf("_"));
                Label header = new Label();
                header.Text = "New policy for product " + ProductID;
                HeaderDiv.Controls.Add(header);

            }
            else
            {
                this.StoreID = Request.QueryString["store_id"];
                this.ProductID = "";
                Label header = new Label();
                header.Text = "New policy for store " + StoreID;
                HeaderDiv.Controls.Add(header);
            }

            this.attributes = GenerateAttributes();

            if (!IsPostBack)
            {
                DropDownNode statementTree = new DropDownNode();
                statementTree.padding = 0;
                statementTree.ID = "dynamicDDN1";
                statementTree.AutoPostBack = true;
                statementTree.Attributes.Add("AutoPostBack", "True");
                statementTree.SelectedIndexChanged += new EventHandler(StatementDLL_SelectedIndexChanged);
                statementTree.EnableViewState = true;
                statementTree.DataSource = this.typeMap.Keys;
                statementTree.DataBind();
                statementTree.Items.Insert(0, new ListItem("--SELECT--"));
                this.root = statementTree.node;


                HtmlGenericControl newdiv = new HtmlGenericControl();
                newdiv.Style.Add("padding-left", 0 + "px");

                newdiv.Controls.Add(statementTree);
                MainDiv.Controls.Add(newdiv);
                MainDiv.Controls.Add(new LiteralControl("<br />"));

                Session.Add("StatementRoot", new TreeNode());
                Session["StatementRoot"] = statementTree.node;
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
                    Placement(new DropDownNode[] { c1, c2 }, this.typeMap.Keys.ToList(), currNode);
                    currNode.node.ChildNodes.Add(c1.node);
                    currNode.node.ChildNodes.Add(c2.node);
                    return;

                case "Any":
                case "ForAll":
                    DropDownNode c = new DropDownNode();
                    Placement(new DropDownNode[] { c }, this.typeMap.Keys.ToList(), currNode);
                    currNode.node.ChildNodes.Add(c.node);
                    return;

                case "AtLeast":
                case "AtMost":
                    HtmlInputText input = new HtmlInputText();
                    DropDownNode inputDDN = new DropDownNode();
                    inputDDN.inputValue = input;
                    DropDownNode c3 = new DropDownNode();
                    Placement(new DropDownNode[] { inputDDN, c3 }, this.typeMap.Keys.ToList(), currNode);
                    currNode.node.ChildNodes.Add(c3.node);
                    return;

                case "Equal":
                case "SmallerThan":
                case "GreaterThan":
                    HtmlInputText inputRelation = new HtmlInputText();
                    DropDownNode inputDDNRelation = new DropDownNode();
                    inputDDNRelation.inputValue = inputRelation;
                    DropDownNode c4 = new DropDownNode();
                    Placement(new DropDownNode[] { inputDDNRelation, c4 }, this.attributes, currNode);
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
                    if (data.Contains("--Attributes--"))
                        placeMe.ID = "atrs" + placeMe.ID;
                }
                newdiv.Controls.Add(placeMe);
                placeMe.myContainer = newdiv;

                int parentDivIndex = MainDiv.Controls.IndexOf(parent.myContainer);
                MainDiv.Controls.AddAt(parentDivIndex + 2 * counter, newdiv);
                MainDiv.Controls.AddAt(parentDivIndex + 2 * counter + 1, new LiteralControl("<br />"));
                counter++;
            }

        }

        // compile the statement to string
        protected void AddPolicyButtonCLick(object sender, EventArgs e)
        {
            string policyName = PolicyNameID.Value;
            string saleTarget = SelectPolicyTypeList.SelectedValue;
            string targetAttribute = PolicyAttributeID.Value;
            string salePercentage = PolicySaleValueID.Value;
            Label error = new Label();
            error.ForeColor = Color.Red;
            if (saleTarget == "--SELECT SALE TARGET--") 
            {
                error.Text = "Choose a sale's target!";
                SaleDiv.Controls.Add(error);
                return;
            }
            if (targetAttribute == "") 
            {
                error.Text = "Enter terget attribute!";
                SaleDiv.Controls.Add(error);
                return;
            }
            if (salePercentage == "" ) 
            {
                error.Text = "Missing percentage value!";
                SaleDiv.Controls.Add(error);
                return;
            }

            try
            {
                double value = Double.Parse(salePercentage);
                if (value < 0 || value > 100) 
                {
                    throw new Exception();
                }
            }
            catch(Exception ex)
            {
                error.Text = "Bad percentage value!";
                SaleDiv.Controls.Add(error);
                return;
            }

            if(policyName == "")
            {
                error.Text = "Missing policy name";
                SaleDiv.Controls.Add(error);
                return;
            }


            // navigate through TreeNode (via root) with recursive function that returns string while parent node responsible to create its Statement code-name and the parenthesis for its children
            string formula = "[" + GatherStatement(this.root) + "]";
            List<string> policyProperties = new List<string>();
            switch (saleTarget)
            {
                case "Product": // type, string polName, double salePercentage, string description, Statement formula, string productID
                    policyProperties = new List<string>() { saleTarget, policyName, salePercentage, PolicyDescriptionID.Text, formula, targetAttribute };
                    break;

                case "Store": // type, string polName, double salePercentage, string description, string storeID, String formula
                    policyProperties = new List<string>() { saleTarget, policyName, salePercentage, PolicyDescriptionID.Text, targetAttribute, formula };
                    break;

                case "Category": // type, string polName, double salePercentage, string description, string category, Statement formula
                    policyProperties = new List<string>() { saleTarget, policyName, salePercentage, PolicyDescriptionID.Text, targetAttribute, formula };
                    break;
            }

            // send the statement to ServiceController
            if (this.ProductID != "")
            {                               
                ((Service_Controller)Session["service_controller"]).AddProductPurchasePolicy(this.ProductID, policyProperties);
            }
            else
            {
                ((Service_Controller)Session["service_controller"]).AddStorePurchasePolicy(this.StoreID, policyProperties);
            }

            Session.Remove("StatementRoot");

            // redirect back to POLICY managing page (with store id in URL)
            Response.Redirect(string.Format("/Presentaion_Layer/PurchasePolicyViewPage.aspx?store_id={0}", this.StoreID));
        }


        protected void CancelButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/Presentaion_Layer/PurchasePolicyViewPage.aspx?store_id={0}", this.StoreID));
        }




        private string GatherStatement(TreeNode me)
        {
            string type = me.Text;
            string statement = "<Statement>";
            string ret = "";
            switch (type)
            {
                case "OR":
                case "AND":
                case "XOR":
                case "IfThen":
                    ret = this.typeMap[type];
                    ret = ret.Insert(ret.IndexOf(statement), "1");
                    ret = ret.Replace("1" + statement, GatherStatement(me.ChildNodes[0]));
                    ret = ret.Replace(statement, GatherStatement(me.ChildNodes[1]));
                    break;

                case "Any":
                case "ForAll":
                    ret = this.typeMap[type];
                    ret = ret.Replace(statement, GatherStatement(me.ChildNodes[0]));
                    break;

                case "AtLeast":
                case "AtMost":
                    ret = this.typeMap[type];
                    ret = ret.Replace("<Quantity>", GatherStatement(me.ChildNodes[0]));
                    ret = ret.Replace(statement, GatherStatement(me.ChildNodes[1]));
                    break;

                case "Equal":
                case "SmallerThan":
                case "GreaterThan":
                    ret = this.typeMap[type];
                    ret = ret.Replace("<AttributeValue>", GatherStatement(me.ChildNodes[0])); // switched places of <AttributeName> and <AttributeValue>
                    ret = ret.Replace("<AttributeName>", GatherStatement(me.ChildNodes[1]));
                    break;

                default:
                    ret = me.Text;
                    break;

            }
            return ret;
        }

        private List<string> GenerateAttributes()
        {
            string prodID = "";
            if (Request.QueryString.AllKeys.Contains("product_id"))
            {
                prodID = Request.QueryString["product_id"];
            }

            List<string> attributes = new List<string>(){"--Attributes--", "User.Age", "User.Address", "User.Name", "StoreID", "ItemID", "Quantity", "ReservedQuantity", "Price", "Name",
                                                                                            "Sale", "Description", "Rating", "Weight",  "TimesBought", "Category" };
            if (prodID != "")
            {
                attributes.Add("--Product Attributes--");
                ItemDTO item = ((Service_Controller)Session["service_controller"]).get_product_by_productID(this.ProductID).Value;
                attributes = attributes.Concat(item.PurchaseAttributes.Keys).ToList();
            }
            return attributes;
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

