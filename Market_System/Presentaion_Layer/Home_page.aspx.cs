using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class Home_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void register_click(object sender, EventArgs e)
        {
            Response.Redirect("/Presentaion_Layer/register_page");
        }

        protected void login_Click(object sender, EventArgs e)
        {

            Response.Redirect("/Presentaion_Layer/Login");

        }
    }
}