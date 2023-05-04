using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class store_managing_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response<List<String>> response = ((Service_Controller)Session["service_controller"]).get_stores_that_user_works_in();
            string[] show_me = response.Value.ToArray();

            stores_user_works_in.DataSource = show_me;
            stores_user_works_in.DataBind();
        }
    }
}