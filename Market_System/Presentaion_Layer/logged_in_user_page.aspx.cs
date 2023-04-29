using Market_System.ServiceLayer;
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
          
            string send_me = "hello "+logged_in_username+" !";
            (this.Master as SiteMaster).a_user_logs_in();
            (this.Master as SiteMaster).set_username_message(send_me);

        }




        public void onclick_Logout(object sender, EventArgs e)
        {
            Service_Controller sv = (Service_Controller)Session["service_controller"];
            Response<string> result = sv.log_out();
            Session["username"] = "";
            Response.Redirect("~/");
        }

        
    }
}