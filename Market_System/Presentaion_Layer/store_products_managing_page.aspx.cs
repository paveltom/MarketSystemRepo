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
    public partial class store_products_managing_page : System.Web.UI.Page
    {
        private string store_id;
        protected void Page_Load(object sender, EventArgs e)
        {
             store_id = Request.QueryString["store_id"];
            string store_name=((Service_Controller)Session["service_controller"]).GetStore(store_id).Value.Name;
            title_label.Text = "managing " + store_name + "'s products page.";
            Response<List<string>> products = ((Service_Controller)Session["service_controller"]).get_products_from_shop_as_list_of_string(store_id);
            if(products.Value.Count>0)
            {
                products_list.DataSource = products.Value;
                products_list.DataBind();
            }

            Response<List<String>> response = ((Service_Controller)Session["service_controller"]).get_all_categories();
            List<string> current_shown_categoreis = new List<string>();

            foreach (ListItem item in ddl_categories.Items)
            {

                current_shown_categoreis.Add(item.Text);
                
            }


          
            //if((List<string>)Session["categories_drop_down_list_datasoruce"] != null && response.Value.All(((List<string>)Session["categories_drop_down_list_datasoruce"]).Contains)) // this so it wont always load the list
            if ( response.Value.All(current_shown_categoreis.Contains)) // this so it wont always load the list
            {
                return;
            }

            ddl_categories.AppendDataBoundItems = true;
            ddl_categories.DataSource = response.Value;
            
            ddl_categories.DataBind();
               // Session["categories_drop_down_list_datasoruce"] = response.Value;
            }

        protected void add_product_click(object sender, EventArgs e)
        {
            if(name_txt.Text.Equals("") || desc_txt.Text.Equals("") || price_txt.Text.Equals("") || quantity_txt.Text.Equals("") || sale_txt.Text.Equals("") || weight_txt.Text.Equals("") || dimenstios_txt.Text.Equals("") || attribute_txt.Text.Equals("") || ddl_categories.SelectedValue.Equals("nothing_to_show"))
            {
                add_message.Text = "some fields are missing!";
            }
            else
            {
                Response<ItemDTO> response = ((Service_Controller)Session["service_controller"]).add_product_to_store(store_id,name_txt.Text,desc_txt.Text,price_txt.Text,quantity_txt.Text,"0","0",sale_txt.Text,weight_txt.Text,dimenstios_txt.Text,attribute_txt.Text,ddl_categories.SelectedValue);
                if(response.ErrorOccured)
                {
                    add_message.Text = response.ErrorMessage;
                    add_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    add_message.Text = "added product successfully";
                    add_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }
        
    }
    
}