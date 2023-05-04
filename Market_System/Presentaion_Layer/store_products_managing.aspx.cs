using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class store_products_managing : System.Web.UI.Page
    {
        private string store_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.store_id = Request.QueryString["store_id"];
            Response<List<String>> response = ((Service_Controller)Session["service_controller"]).get_products_from_shop_as_list_of_string(store_id);
            if (response.ErrorOccured)
            {

            }
            else
            {
                if ((List<string>)Session["cart_page_drop_down_list_datasoruce"] != null && response.Value.All(((List<string>)Session["cart_page_drop_down_list_datasoruce"]).Contains)) // this so it wont always load the list
                {


                }
            }
        }
    }
}