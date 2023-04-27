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
            if (password.Equals("abc"))
            {
                Response.Redirect("logged_in_user_master_page");
            }
            else
            {
                error_message.Text = "wrong password !!! the password is: abc";
            }
        }

        
    }
}