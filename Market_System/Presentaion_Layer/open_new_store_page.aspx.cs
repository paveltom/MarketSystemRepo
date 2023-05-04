using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class open_new_store_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Open_store_click(object sender, EventArgs e)
        {
            string new_store_name = store_name_txt.Text;
            if(new_store_name.Equals(""))
            {
                message.ForeColor = System.Drawing.Color.Red;
                message.Text = "please enter a name!!!";
            }
            else
            {
                List<String> why_the_fuck_is_this_a_list = new List<string>();
                why_the_fuck_is_this_a_list.Add(new_store_name);
                Response<StoreDTO> ok = ((Service_Controller)Session["service_controller"]).open_new_store(why_the_fuck_is_this_a_list);
                if(!ok.ErrorOccured)
                {
                    message.ForeColor = System.Drawing.Color.Green;
                    message.Text = "a new store has just been opened! , store name:  " + ok.Value.Name + " , store ID: " + ok.Value.StoreID;
                }
                else
                {
                    message.ForeColor = System.Drawing.Color.Red;
                    message.Text = ok.ErrorMessage;
                }
            }
           
        }

    }
}