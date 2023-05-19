using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!((Service_Controller)Session["service_controller"]).HasNewMessages())
            {
                no_messages_label.Text = "there are no NEW messages!";
               
            }
            Response<List<string>> ok = ((Service_Controller)Session["service_controller"]).GetMessages();
            if (ok!=null)
            {
                string[] messages = ok.Value.ToArray();
                messages_list.DataSource = messages;
                messages_list.DataBind();
            }


        }
    }
}