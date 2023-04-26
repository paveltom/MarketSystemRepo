using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void register_button(object sender, EventArgs e)
        {
            Service_Controller sv = new Service_Controller();
            sv.register(txt_username.Text,txt_password.Text,txt_address.Text);
        }
    }
}