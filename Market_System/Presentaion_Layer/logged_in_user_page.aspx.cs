using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class logged_in_user_page : System.Web.UI.Page
    {
        private string logged_in_username;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.logged_in_username = Request.QueryString["name"];
            username.Text = "hello "+logged_in_username+" !";
        }
    }
}