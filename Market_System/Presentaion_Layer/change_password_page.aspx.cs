using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class change_password_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void change_password_click(object sender, EventArgs e)
        {
            if(!new_password.Text.Equals(confirm_new_password.Text))
            {
                message.ForeColor = System.Drawing.Color.Red;
                message.Text = "the passwords you provided does not match.";
            }
            else
            {
              Response<string> result=  ((Service_Controller)Session["service_controller"]).change_password(new_password.Text);
                if(result.ErrorOccured)
                {
                    message.ForeColor = System.Drawing.Color.Red;
                    message.Text = result.ErrorMessage;
                }
                else
                {
                    message.ForeColor = System.Drawing.Color.Green;
                    message.Text = result.Value;
                }
            }
        }
    }
}