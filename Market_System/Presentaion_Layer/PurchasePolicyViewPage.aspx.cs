using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class PurchasePolicyViewPage : System.Web.UI.Page
    {
        private string StoreID;
        private string ProductID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.StoreID = Request.QueryString["store_id"];
            if (!IsPostBack)
            {
                List<string> policies = ((Service_Controller)Session["service_controller"]).GetStore(StoreID).Value.DefaultPolicies;
                PoliciesList.DataSource = policies;
                PoliciesList.DataBind();
            }

        }

        protected void SelectPoliciesToViewList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = SelectPoliciesToViewList.SelectedValue;
            if (selected.Equals("Store Policies"))
            {
                ProductIDToViewPolicies.Visible = false;
                ShowProductPoliciesButton.Visible = false;
                NoProductIdErrorMessage.Visible = false;
                List<string> policies = ((Service_Controller)Session["service_controller"]).GetStore(this.StoreID).Value.DefaultPolicies;
                PoliciesList.DataSource = policies;
                PoliciesList.DataBind();
            }
            else
            {
                ProductIDToViewPolicies.Visible = true;
                ShowProductPoliciesButton.Visible = true;
            }
        }

        protected void ShowProductPolicies(object sender, EventArgs e)
        {
            string inputText = ProductIDToViewPolicies.Value;
            if (inputText.Equals(""))
            {
                NoProductIdErrorMessage.Text = "Please enter product ID";
            }
            else
            {
                NoProductIdErrorMessage.Visible = false;
                List<string> policies = ((Service_Controller)Session["service_controller"]).get_product_by_productID(inputText).Value.PurchasePolicies.Select(x => x.Value.ToString()).ToList();
                PoliciesList.DataSource = policies;
                PoliciesList.DataBind();
            }
        }

        protected void AddNewPolicyClick(object sender, EventArgs e)
        {
            string possibleProduct = ProductIDToViewPolicies.Value;
            if (possibleProduct != "")
                Response.Redirect(string.Format("/Presentaion_Layer/PurchasePolicyManagePage.aspx?product_id={0}", this.ProductID));
            else
                Response.Redirect(string.Format("/Presentaion_Layer/PurchasePolicyManagePage.aspx?store_id={0}", this.StoreID));
        }


        protected void ReturnToStoreButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/Presentaion_Layer/store_managing_page.aspx?store_id={0}", this.StoreID));
        }

        protected void RemovePolicyClick(object sender, EventArgs e)
        {
            Button remove = sender as Button;
            DataListItem item = remove.Parent as DataListItem;

            Label strategyIDLabel = item.Controls[1] as Label;
            string strategyID = strategyIDLabel.Text.Split(':')[0];
            if (ProductIDToViewPolicies.Visible == true)
            {
                string productID = ProductIDToViewPolicies.Value;
                ((Service_Controller)Session["service_controller"]).RemoveProductPurchasePolicy(productID, strategyID);
            }
            else
            {
                string storeID = this.StoreID;
                ((Service_Controller)Session["service_controller"]).RemoveStorePurchasePolicy(storeID, strategyID);
            }

            List<string> strategies = ((Service_Controller)Session["service_controller"]).GetStore(this.StoreID).Value.DefaultStrategies;
            PoliciesList.DataSource = strategies;
            PoliciesList.DataBind();
        }

        
    }
}




