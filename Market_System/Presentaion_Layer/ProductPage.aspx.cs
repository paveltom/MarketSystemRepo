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
    public partial class ProductPage : System.Web.UI.Page
    {
        
      
        private string product_id;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            this.product_id = Request.QueryString["product_id"];
            //Response.Write(product_id);
            Response<ItemDTO> item = ((Service_Controller)Session["service_controller"]).get_product_by_productID(product_id);

            id.Text = "Name: " + item.Value.get_name();
            price.Text = "Price: " + item.Value.Price;
            quantity.Text = "Quantity in stock: " + item.Value.GetQuantity().ToString();
            description.Text = "Description: \n" + item.Value.getDescription();

            Response<List<String>> response = ((Service_Controller)Session["service_controller"]).get_all_comments_of_product(product_id);
            string[] show_me = response.Value.ToArray();

            comments_list.DataSource = show_me;
            comments_list.DataBind();

            Response<Boolean> bought_or_not= ((Service_Controller)Session["service_controller"]).check_if_user_bought_item(product_id);
            if(bought_or_not.Value)
            {
                add_commnet_label.Visible = true;
                commnet_txtbox.Visible = true;
                rating_label.Visible = true;
                ddl_rating.Visible = true;
                comment_button.Visible = true;
            }
            else
            {
                add_commnet_label.Visible = false;
                commnet_txtbox.Visible = false;
                rating_label.Visible = false;
                ddl_rating.Visible = false;
                comment_button.Visible = false;
            }

            


        }

        protected void addToCart_Click(object sender, EventArgs e)
        {

            Response<string> okay = ((Service_Controller)Session["service_controller"]).add_product_to_basket(product_id, quantityToAdd.Text);
            if (okay.ErrorOccured)
            {
                clickmsg.Text = okay.ErrorMessage;
                clickmsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                clickmsg.Text = okay.Value;
                clickmsg.ForeColor = System.Drawing.Color.Green;
            }
 

        }


        protected void bid_click(object sender, EventArgs e)
        {
            if(bid_quantity_text.Text.Equals("") || new_price_text.Text.Equals(""))
            {
                bid_error_message.Text = "please enter valid values";
                return;
            }
            try
            {
                int quantity = Int32.Parse(bid_quantity_text.Text);
                double price = double.Parse(new_price_text.Text);
                Response<BidDTO> okay = ((Service_Controller)Session["service_controller"]).PlaceBid(product_id, price,quantity);
                if(okay.ErrorOccured)
                {
                    bid_error_message.ForeColor = System.Drawing.Color.Red;
                    bid_error_message.Text = okay.ErrorMessage;
                }
                else
                {
                    bid_error_message.ForeColor = System.Drawing.Color.Green;
                    bid_error_message.Text = "bid was sent succefully , waiting for store owners response";
                }

            }
            catch(Exception exe)
            {
                bid_error_message.ForeColor = System.Drawing.Color.Red;
                bid_error_message.Text = exe.Message;
            }
        }


        protected void add_commnet_click(object sender, EventArgs e)
        {
            if (ddl_rating.SelectedValue.Equals("nothing_to_show") || commnet_txtbox.Equals(""))
            {
                comment_message.ForeColor = System.Drawing.Color.Red;
                comment_message.Text = "please choose rating and typein comment properly";
            }
            else
            {
                double rating = Double.Parse(ddl_rating.SelectedValue);

                Response<string> okay = ((Service_Controller)Session["service_controller"]).comment_on_product(product_id, commnet_txtbox.Text, rating);
                if (okay.ErrorOccured)
                {
                    comment_message.Text = okay.ErrorMessage;
                    comment_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    comment_message.Text = okay.Value;
                    comment_message.ForeColor = System.Drawing.Color.Green;
                }
            }


        }
    }
}