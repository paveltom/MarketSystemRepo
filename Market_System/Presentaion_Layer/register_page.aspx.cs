using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Market_System;

namespace Market_System.Presentaion_Layer
{
    public partial class Register_page : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void register_button(object sender, EventArgs e)
        {
            Service_Controller sv = (Service_Controller)Session["service_controller"];
            Response<string> result = sv.register(txt_username.Text, txt_password.Text, txt_address.Text);
            if (!result.ErrorOccured)
            {
                message.Text = result.Value;
                message.ForeColor = System.Drawing.Color.Green;
              }
            else
            {
                message.Text = result.ErrorMessage;
                message.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}