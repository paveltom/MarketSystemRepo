using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class PurchaseStrategyManagePage : System.Web.UI.Page
    {
        private string StoreID;
        private string ProductID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.StoreID = Request.QueryString["store_id"];
            if (!IsPostBack)            {
                
                List<string> strategies = ((Service_Controller)Session["service_controller"]).GetStore(StoreID).Value.DefaultStrategies;
                StrategiesList.DataSource = strategies;
                StrategiesList.DataBind();
            }

        }

        protected void SelectStrategiesToViewList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = SelectStrategiesToViewList.SelectedValue;
            if (selected.Equals("Store Strategies"))
            {
                ProductIDToViewStrategies.Visible = false;
                ShowProductStrategiesButton.Visible = false;
                NoProductIdErrorMessage.Visible = false;
                List<string> strategies = ((Service_Controller)Session["service_controller"]).GetStore(this.StoreID).Value.DefaultStrategies;
                StrategiesList.DataSource = strategies;
                StrategiesList.DataBind();
            }
            else
            {
                ProductIDToViewStrategies.Visible = true;
                ShowProductStrategiesButton.Visible = true;
            }
        }

        protected void ShowProductStrategies(object sender, EventArgs e)
        {
            string inputText = ProductIDToViewStrategies.Value;
            if (inputText.Equals(""))
            {
                NoProductIdErrorMessage.Text = "Please enter product ID";
            }
            else
            {
                NoProductIdErrorMessage.Visible = false;
                List<string> strategies = ((Service_Controller)Session["service_controller"]).get_product_by_productID(inputText).Value.PurchaseStrategies.Select(x => x.Value.ToString()).ToList();
                StrategiesList.DataSource = strategies;
                StrategiesList.DataBind();
            }
        }

        protected void AddNewStrategyClick(object sender, EventArgs e)
        {
            string possibleProduct = ProductIDToViewStrategies.Value;
            if(possibleProduct != "")
                Response.Redirect(string.Format("/Presentaion_Layer/StrategyBuilderPage.aspx?product_id={0}", this.ProductID));
            else
                Response.Redirect(string.Format("/Presentaion_Layer/StrategyBuilderPage.aspx?store_id={0}", this.StoreID));           
        }


        protected void ReturnToStoreButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/Presentaion_Layer/store_managing_page.aspx?store_id={0}", this.StoreID));
        }

        protected void RemoveStrategyClick(object sender, EventArgs e)
        {
            Button remove = sender as Button;
            DataListItem item = remove.Parent as DataListItem;

            Label strategyIDLabel = item.Controls[1] as Label;
            string strategyID = strategyIDLabel.Text.Split(':')[0];   
            if (ProductIDToViewStrategies.Visible == true)
            {
                string productID = ProductIDToViewStrategies.Value;
                ((Service_Controller)Session["service_controller"]).RemoveProductPurchaseStrategy(productID, strategyID);
            }
            else
            {
                string storeID = this.StoreID;
                ((Service_Controller)Session["service_controller"]).RemoveStorePurchaseStrategy(storeID, strategyID);
            }

            List<string> strategies = ((Service_Controller)Session["service_controller"]).GetStore(this.StoreID).Value.DefaultStrategies;
            StrategiesList.DataSource = strategies;
            StrategiesList.DataBind();

        }



    }
}