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

        protected void remove_product_button_Click(object sender, EventArgs e)
        {
            if (product_to_remove_id.Text.Equals(""))
            {
                remove_message.Text = "please type in the ID of the product that you wish to remove.";
            }
            else
            {
                Response<string> response = ((Service_Controller)Session["service_controller"]).remove_product_from_store(store_id, product_to_remove_id.Text);
                if (response.ErrorOccured)
                {
                    remove_message.Text = response.ErrorMessage;
                    remove_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    remove_message.Text = response.Value;
                    remove_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }
        protected void edit_dimensions_click(object sender, EventArgs e)
        {
            if (edit_dimensions_txt.Text.Equals("")|| edit_dimensions_txt.Text.Equals("new dimensions") || product_ID_txt.Text.Equals(""))
            {
                edit_dimensions_message.Text = "please type in the new dimensions.";
            }
            else
            {
                
                Double[] why_the_fuck_is_this_not_string_array= Array.ConvertAll(edit_dimensions_txt.Text.Split('_'), Double.Parse);
                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductDimenssions(product_ID_txt.Text, why_the_fuck_is_this_not_string_array);
                if (response.ErrorOccured)
                {
                    edit_dimensions_message.Text = response.ErrorMessage;
                    edit_dimensions_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_dimensions_message.Text = response.Value;
                    edit_dimensions_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void edit_name_click(object sender, EventArgs e)
        {
            if (edit_name_txt.Text.Equals("") || edit_name_txt.Text.Equals("new name")|| product_ID_txt.Text.Equals(""))
            {
                edit_name_message.Text = "please type in the new name.";
            }
            else
            {


                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductName(product_ID_txt.Text, edit_name_txt.Text);
                if (response.ErrorOccured)
                {
                    edit_name_message.Text = response.ErrorMessage;
                    edit_name_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_name_message.Text = response.Value;
                    edit_name_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void edit_description_click(object sender, EventArgs e)
        {
            if (edit_description_txt.Text.Equals("") || edit_description_txt.Text.Equals("new description") || product_ID_txt.Text.Equals(""))
            {
                edit_description_message.Text = "please type in the new description.";
            }
            else
            {


                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductDescription(product_ID_txt.Text, edit_description_txt.Text);
                if (response.ErrorOccured)
                {
                    edit_description_message.Text = response.ErrorMessage;
                    edit_description_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_description_message.Text = response.Value;
                    edit_description_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void edit_price_click(object sender, EventArgs e)
        {
            if (edit_price_txt.Text.Equals("") || edit_price_txt.Text.Equals("new price") || product_ID_txt.Text.Equals(""))
            {
                edit_description_message.Text = "please type in the new price.";
            }
            else
            {

                
                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductPrice(product_ID_txt.Text, double.Parse( edit_price_txt.Text));
                if (response.ErrorOccured)
                {
                    edit_price_message.Text = response.ErrorMessage;
                    edit_price_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_price_message.Text = response.Value;
                    edit_price_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }



        protected void edit_quantity_click(object sender, EventArgs e)
        {
            if (edit_quantity_txt.Text.Equals("") || edit_quantity_txt.Text.Equals("new quantity") || edit_quantity_txt.Text.Equals(""))
            {
                edit_quantity_message.Text = "please type in the new quantity.";
            }
            else
            {


                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductQuantity(product_ID_txt.Text,int.Parse(edit_quantity_txt.Text));
                if (response.ErrorOccured)
                {
                    edit_quantity_message.Text = response.ErrorMessage;
                    edit_quantity_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_quantity_message.Text = response.Value;
                    edit_quantity_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }






        protected void edit_wieght_click(object sender, EventArgs e)
        {
            if (edit_wieght_txt.Text.Equals("") || edit_price_txt.Text.Equals("new wieght") || product_ID_txt.Text.Equals(""))
            {
                edit_wieght_message.Text = "please type in the new wieght.";
            }
            else
            {


                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductWeight(product_ID_txt.Text, double.Parse(edit_wieght_txt.Text));
                if (response.ErrorOccured)
                {
                    edit_wieght_message.Text = response.ErrorMessage;
                    edit_wieght_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_wieght_message.Text = response.Value;
                    edit_wieght_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }


        protected void edit_sale_click(object sender, EventArgs e)
        {
            if (edit_sale_txt.Text.Equals("") || edit_sale_txt.Text.Equals("new sale") || product_ID_txt.Text.Equals(""))
            {
                edit_sale_message.Text = "please type in the new sale.";
            }
            else
            {


                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductSale(product_ID_txt.Text, double.Parse(edit_sale_txt.Text));
                if (response.ErrorOccured)
                {
                    edit_sale_message.Text = response.ErrorMessage;
                    edit_sale_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_sale_message.Text = response.Value;
                    edit_sale_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }





        protected void edit_category_click(object sender, EventArgs e)
        {
            if (edit_category_txt.Text.Equals("") || edit_category_txt.Text.Equals("new category") || product_ID_txt.Text.Equals(""))
            {
                edit_category_message.Text = "please type in the new category.";
            }
            else
            {


                Response<string> response = ((Service_Controller)Session["service_controller"]).ChangeProductCategory(product_ID_txt.Text, edit_category_txt.Text);
                if (response.ErrorOccured)
                {
                    edit_category_message.Text = response.ErrorMessage;
                    edit_category_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    edit_category_message.Text = response.Value;
                    edit_category_message.ForeColor = System.Drawing.Color.Green;
                }
            }
        }


    }
    
}