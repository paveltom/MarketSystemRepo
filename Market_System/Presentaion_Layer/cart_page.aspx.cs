using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class cart_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           Response<List<String>> response = ((Service_Controller)Session["service_controller"]).get_store_ids_from_cart();
            if(response.ErrorOccured)
            {
                error_message.Text = response.ErrorMessage;
            }
            else
            {
                ddl_store_id.DataSource = response.Value;
                ddl_store_id.DataBind();
            }
        }
    }
}