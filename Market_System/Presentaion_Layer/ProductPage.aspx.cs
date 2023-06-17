﻿using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            //check auction 
            Response<ItemDTO> auction_checker = ((Service_Controller)Session["service_controller"]).get_product_by_productID(product_id);
            if(auction_checker.Value.Auction.Value[0].Equals("-1.0"))
            {
                auction_button.Visible = false;
            }
            else
            {
                Response<TimerPlus> timer = ((Service_Controller)Session["service_controller"]).get_timer_of_auciton(product_id + "_" + "auction" + "_timer");
                if (timer.ErrorOccured)
                {
                    auction_button.Visible = false;
                }
                else
                {
                    auction_button.Visible = true;
                }
            }

            Response<TimerPlus> lottery_timer = ((Service_Controller)Session["service_controller"]).get_timer_of_auciton(product_id + "_" + "lottery" + "_timer");

            if (lottery_timer.ErrorOccured)
            {
                lottery_button.Visible = false;
            }
            else
            {
                lottery_button.Visible = true;
            }


            bool checkCounterBid = false;
            try
            {
                checkCounterBid = ((Service_Controller)Session["service_controller"]).CheckCounterBid(product_id);
            }
            catch (Exception ex)
            {
                //Do nothing!
            }

            if (checkCounterBid) //Counter bidding buttons
            {
                Button2.Visible = true;
                Button3.Visible = true;
                bid_id0.Visible = true;
                Label11.Visible = true;
            }

            else
            {
                Button2.Visible = false;
                Button3.Visible = false;
                bid_id0.Visible = false;
                Label11.Visible = false;
            }
            counter_bid_error_message.Text = "";
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

                if (bid_card_number.Text.Equals("") || bid_month.Text.Equals("") || bid_year.Text.Equals("") || bid_holder.Text.Equals("") || bid_ccv.Text.Equals("") || bid_id.Text.Equals(""))
                {
                    bid_error_message.ForeColor = System.Drawing.Color.Red;
                    bid_error_message.Text = "Please fill in all fields.";
                    return;
                }

                try
                {
                    Response<bool> bidDTO = ((Service_Controller)Session["service_controller"]).checkIfBidPlacesAlready(product_id);
                    if (bidDTO.Value)
                    {
                        bid_error_message.ForeColor = System.Drawing.Color.Red;
                        bid_error_message.Text = "You've already placed a bid for this product! please wait for the store's reply.";
                        return;
                    }
                }

                catch (Exception ex2)
                {
                    //do nothing
                }

                Response<BidDTO> okay = ((Service_Controller)Session["service_controller"]).PlaceBid(product_id, price,quantity, bid_card_number.Text, bid_month.Text, bid_year.Text, bid_holder.Text, bid_ccv.Text, bid_id.Text);
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

        protected void approve_counter_bid(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Response<BidDTO> bidDTO = ((Service_Controller)Session["service_controller"]).GetBid(bid_id0.Text);
                    if (bidDTO.Value.ApprovedByUser)
                    {
                        counter_bid_error_message.ForeColor = System.Drawing.Color.Red;
                        counter_bid_error_message.Text = "You've already accepted the counter bid.";
                        return;
                    }
                }

                catch(Exception ex2)
                {
                    //do nothing
                }

                Response<string> okay = ((Service_Controller)Session["service_controller"]).ApproveBid_2(bid_id0.Text);
                if (okay.ErrorOccured)
                {
                    counter_bid_error_message.ForeColor = System.Drawing.Color.Red;
                    counter_bid_error_message.Text = okay.ErrorMessage;
                }
                else
                {
                    counter_bid_error_message.ForeColor = System.Drawing.Color.Green;
                    counter_bid_error_message.Text = "You've accepted the counter bidding, and the payment was successful";
                }

            }
            catch (Exception exe)
            {
                counter_bid_error_message.ForeColor = System.Drawing.Color.Red;
                counter_bid_error_message.Text = exe.Message;
            }
        }

        protected void reject_counter_bid(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Response<BidDTO> bidDTO = ((Service_Controller)Session["service_controller"]).GetBid(bid_id0.Text);
                    if (bidDTO.Value.DeclinedByUser)
                    {
                        counter_bid_error_message.ForeColor = System.Drawing.Color.Red;
                        counter_bid_error_message.Text = "You've already rejected the counter bid.";
                        return;
                    }
                }
                catch(Exception ex3)
                {
                    //do nothing
                }

                Response<string> okay = ((Service_Controller)Session["service_controller"]).RemoveBid_2(bid_id0.Text);
                if (okay.ErrorOccured)
                {
                    counter_bid_error_message.ForeColor = System.Drawing.Color.Red;
                    counter_bid_error_message.Text = okay.ErrorMessage;
                }
                else
                {
                    counter_bid_error_message.ForeColor = System.Drawing.Color.Green;
                    counter_bid_error_message.Text = "You've rejected the counter bidding. If you wish to counter bid, please place a new bid.";
                }

            }
            catch (Exception exe)
            {
                counter_bid_error_message.ForeColor = System.Drawing.Color.Red;
                counter_bid_error_message.Text = exe.Message;
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

        protected void lottery_button_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Presentaion_Layer/lottery_page.aspx?product_id={0}", product_id));
        }
        

        protected void auction_button_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Presentaion_Layer/auction_page.aspx?product_id={0}", product_id));
        }
    }
}