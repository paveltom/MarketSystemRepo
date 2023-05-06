using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class checkoutPage : System.Web.UI.Page
    {
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            username = (string)Session["username"];
            Response<String> response = ((Service_Controller)Session["service_controller"]).getAdressOrEmpty();
            if (response.ErrorOccured)
            {
                adressTextBox.Text = response.ErrorMessage;
            }
            else adressTextBox.Text = response.Value;

            //checking adress for making availble the pay button
            Response<String> availDelivery = ((Service_Controller)Session["service_controller"]).check_delivery(adressTextBox.Text);
            if (availDelivery.ErrorOccured)
            {
                payButton.Enabled = false;
            }
            else payButton.Enabled= true;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void payClick(object sender, EventArgs e)
        {
            
            //Response<Cart> response = ((Service_Controller)Session["service_controller"]).get_cart(username);
            Response<String> response = ((Service_Controller)Session["service_controller"]).check_out(username,creditTextBox);
            if (response.ErrorOccured)
            {
                adressTextBox.Text = response.ErrorMessage;
            }
            else adressTextBox.Text = response.Value;
        }
        }
    }
}