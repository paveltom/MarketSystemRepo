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
                Response.Redirect(string.Format("/Presentaion_Layer/store_products_managing_page.aspx?store_id={0}", entered_store_id.Text));
            }
        }

        protected void assign_new_manager_click(object sender, EventArgs e)
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

        protected void assign_new_owner_click(object sender, EventArgs e)
        {
            string username = new_owner_username.Text;
            string store_id = entered_store_id.Text;
            Response<string> ok = ((Service_Controller)Session["service_controller"]).assign_new_owner(store_id, username);
            if (ok.ErrorOccured)
            {
                new_owner_message.ForeColor = System.Drawing.Color.Red;
                new_owner_message.Text = ok.ErrorMessage;
            }
            else
            {
                new_owner_message.ForeColor = System.Drawing.Color.Green;
                new_owner_message.Text = ok.Value;
            }

        }

        protected void remove_owner_click(object sender, EventArgs e)
        {
            string username = owner_to_remove.Text;
            string store_id = entered_store_id.Text;
            Response<string> ok = ((Service_Controller)Session["service_controller"]).Remove_Store_Owner(store_id, username);
            if (ok.ErrorOccured)
            {
                owner_remove_message.ForeColor = System.Drawing.Color.Red;
                owner_remove_message.Text = ok.ErrorMessage;
            }
            else
            {
                owner_remove_message.ForeColor = System.Drawing.Color.Green;
                owner_remove_message.Text = ok.Value;
            }

        }

        protected void show_managers_click(object sender, EventArgs e)
        {
            
            string store_id = entered_store_id.Text;
            Response<List<string>> ok = ((Service_Controller)Session["service_controller"]).get_managers_of_store(store_id);
            if (ok.ErrorOccured)
            {
                show_managers_message.ForeColor = System.Drawing.Color.Red;
                show_managers_message.Text = ok.ErrorMessage;
            }
            else
            {
                show_info_label.Text = "Managers of the store:  " + store_id;
                string[] show_me = ok.Value.ToArray();

                show_info_list.DataSource = show_me;
                show_info_list.DataBind();
            }

        }

        protected void show_owners_click(object sender, EventArgs e)
        {
            string username = new_manager_username.Text;
            string store_id = entered_store_id.Text;
            Response<List<string>> ok = ((Service_Controller)Session["service_controller"]).get_owners_of_store(store_id);
            if (ok.ErrorOccured)
            {
                show_owners_message.ForeColor = System.Drawing.Color.Red;
                show_owners_message.Text = ok.ErrorMessage;
            }
            else
            {
                show_info_label.Text = "Owners of the store:  " + store_id;
                string[] show_me = ok.Value.ToArray();

                show_info_list.DataSource = show_me;
                show_info_list.DataBind();
            }

        }

        protected void show_purchase_history_click(object sender, EventArgs e)
        {
            string username = new_manager_username.Text;
            string store_id = entered_store_id.Text;
            Response<string> ok = ((Service_Controller)Session["service_controller"]).get_purchase_history_from_store(store_id);
            if (ok.ErrorOccured)
            {
                show_purchase_history_message.ForeColor = System.Drawing.Color.Red;
                show_purchase_history_message.Text = ok.ErrorMessage;
            }
            else
            {
                show_info_label.Text = "Purchase history of the store:  " + store_id;
                string[] show_me = { ok.Value };

                show_info_list.DataSource = show_me;
                show_info_list.DataBind();
            }

        }

        protected void closeStoreClick(object sender, EventArgs e)
        {
            string store_id = entered_store_id.Text;
            Response<string> closeResponse = ((Service_Controller)Session["service_controller"]).close_store_temporary(store_id);
            if (closeResponse.ErrorOccured)
            {
                closeStoreMsg.ForeColor = System.Drawing.Color.Red;
                closeStoreMsg.Text = closeResponse.ErrorMessage;
            }
            else
            {
                closeStoreMsg.Text = "store "+store_id+" closed ";
            }
        }
    }
}