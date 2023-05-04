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

        protected void manage_store_products_Click(object sender, EventArgs e)
        {
            if(entered_store_id.Text=="")
            {
                error_message.Text = "please enter store ID";
            }
            else
            {
                Response.Redirect(string.Format("/Presentaion_Layer/store_products_managing.aspx?store_id={0}", entered_store_id.Text));
            }
        }

        protected void add_product_button1_Click(object sender, EventArgs e)
        {
            string username = new_manager_username.Text;
            string store_id = entered_store_id.Text;
            Response<string> ok= ((Service_Controller)Session["service_controller"]).assign_new_manager(store_id, username);
            if(ok.ErrorOccured)
            {
                new_manager_message.ForeColor = System.Drawing.Color.Red;
                new_manager_message.Text = ok.ErrorMessage;
            }
            else
            {
                new_manager_message.ForeColor = System.Drawing.Color.Green;
                new_manager_message.Text = ok.Value;
            }

        }
    }
}