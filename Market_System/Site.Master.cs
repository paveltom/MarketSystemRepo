using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System
{
    public partial class SiteMaster : MasterPage
    {
        private ServiceLayer.Service_Controller service_controller;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.service_controller = new ServiceLayer.Service_Controller();
            Session["service_controller"] = service_controller;
        }

        protected void Home_click(object sender, EventArgs e)
        {
            Response.Redirect("Presentaion_Layer/Home_page");
        }

   
    }
}