using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void Login_click(object sender, EventArgs e)
        {
            
            string username = txt_username.Text;
            string password = txt_password.Text;
            
            Response<string> result = ((Service_Controller)Session["service_controller"]).login_member(txt_username.Text, txt_password.Text);
            if (!result.ErrorOccured)
            {
                Session["username"] = username;
                Response.Redirect(string.Format("/Presentaion_Layer/logged_in_user_page.aspx?name={0}", username));
            }
            else
            {
                error_message.Text = result.ErrorMessage;
                error_message.ForeColor = System.Drawing.Color.Red;
            }
           
         
        }

        
    }
}