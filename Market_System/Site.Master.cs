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

            if (this.service_controller == null)
            { 
            this.service_controller = new ServiceLayer.Service_Controller();
        }  
            

            
        }

        public void a_user_logs_in()
        {
            login_href.Visible = false;
            register_href.Visible = false;

        }

        public ServiceLayer.Service_Controller get_service_controller()
        {
            return this.service_controller;
        }


      




    }
}