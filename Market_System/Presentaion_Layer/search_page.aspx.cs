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
    public partial class search_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void go_to_product_page(object sender, EventArgs e)
        {
            Response<ItemDTO> item = ((Service_Controller)Session["service_controller"]).get_product_by_productID(product_id_text.Text);
            if (item.ErrorOccured)
            {
                error_message.Text = item.ErrorMessage;
                return;
            }
            Response.Redirect(string.Format("~/Presentaion_Layer/ProductPage.aspx?product_id={0}", product_id_text.Text));
        
    }


        protected void search_button_click(object sender, EventArgs e)
        {

            string selected_search_function = search_by_ddl.SelectedValue;
            if (selected_search_function.Equals("nothing_to_show"))
            {
                search_result.DataSource = null;
                search_result.DataBind();
                search_error.Text = "please choose how to search by!";
                return;
            }
            Response<List<ItemDTO>> response = null;
            switch (selected_search_function)
            {
                case "category":
                    response = ((Service_Controller)Session["service_controller"]).search_product_by_category(search_text_box.Text);
                    break;
                case "name":
                    response = ((Service_Controller)Session["service_controller"]).search_product_by_name(search_text_box.Text);
                    break;
                case "keyword":
                    response = ((Service_Controller)Session["service_controller"]).search_product_by_keyword(search_text_box.Text);

                    break ;
            }
            if (response.ErrorOccured)
            {
                search_error.Text = response.ErrorMessage;
                return;
            }
            List<string> list = new List<string>();
            foreach (ItemDTO item in response.Value)
            {
                list.Add("id:         " + item.GetID() + "        name:     " + item.get_name() + "             quantity:     " + (item.GetQuantity() - item.GetReservedQuantity()));
            }

            string[] show_me = list.ToArray();

            search_result.DataSource = show_me;
            search_result.DataBind();

        }

    }
}