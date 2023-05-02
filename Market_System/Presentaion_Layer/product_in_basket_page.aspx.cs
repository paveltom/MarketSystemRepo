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
    public partial class product_in_basket_page : System.Web.UI.Page
    {
        private string product_id;
        protected void Page_Load(object sender, EventArgs e)
        {
           this.product_id = Request.QueryString["product_id"];
            Response<Dictionary<string, string>> item = Response<Dictionary<string, string>>.FromValue(((Service_Controller)Session["service_controller"]).extract_item_from_basket(product_id));
            product_name_label.Text = item.Value["name"];
            price_label.Text = item.Value["price"];
            quantity_label.Text = item.Value["quantity"];
 
        }


        protected void add_button_click(object sender, EventArgs e)
        {
            Response<string> ok = ((Service_Controller)Session["service_controller"]).add_product_to_basket(this.product_id, add_quantity.Text);

            if (ok.ErrorOccured)
            {
                add_error.Text = ok.ErrorMessage;
            }
            else
            {
                Response<Dictionary<string, string>> item = Response<Dictionary<string, string>>.FromValue(((Service_Controller)Session["service_controller"]).extract_item_from_basket(product_id));
                price_label.Text = item.Value["price"];
                quantity_label.Text = item.Value["quantity"];
            }

        }

        protected void remove_button_click(object sender, EventArgs e)
        {
            Response<string> ok =((Service_Controller)Session["service_controller"]).remove_product_from_basket(this.product_id,remove_quantity.Text);

            if(ok.ErrorOccured)
            {
                remove_error.Text = ok.ErrorMessage;
            }
            else
            {
                Response<Dictionary<string, string>> item = Response<Dictionary<string, string>>.FromValue(((Service_Controller)Session["service_controller"]).extract_item_from_basket(product_id));
                if (remove_quantity.Equals(quantity_label.Text))
                {
                    
                    Response.Redirect(string.Format("/Presentaion_Layer/cart_page.aspx"));
                }
               
                
                price_label.Text = item.Value["price"];
                quantity_label.Text = item.Value["quantity"];
            }

        }

     
    }
    


}