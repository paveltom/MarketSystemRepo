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

        protected void show_basket_of_selected_store_id(object sender, EventArgs e)
        {
            string selected_store_id = ddl_store_id.SelectedValue;


            
            Response<List<String>> response = ((Service_Controller)Session["service_controller"]).show_basket_in_cart(selected_store_id);
            string[] show_me = response.Value.ToArray();
            products_list.DataSource = show_me;
            products_list.DataBind();

        }
    }
}