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
    public partial class ProductPage : System.Web.UI.Page
    {
        
      
        private string product_id;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            this.product_id = Request.QueryString["product_id"];
            //Response.Write(product_id);
            Response<ItemDTO> item = ((Service_Controller)Session["service_controller"]).get_product_by_productID(product_id);
            id.Text = "Name: " + item.Value.get_name();
            quantity.Text = "Quantity in stock: " + item.Value.GetQuantity().ToString();
        }

        protected void addToCart_Click(object sender, EventArgs e)
        {

            Response<string> okay = ((Service_Controller)Session["service_controller"]).add_product_to_basket(product_id, quantityToAdd.Text);
            if (okay.ErrorOccured)
            {
                clickmsg.Text = okay.ErrorMessage;
                clickmsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                clickmsg.Text = okay.Value;
                clickmsg.ForeColor = System.Drawing.Color.Green;
            }
 

        }
    }
}