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
    public partial class auction_page : System.Web.UI.Page
    {
        private string product_id;
        private System.Timers.Timer timer;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.product_id = Request.QueryString["product_id"];
            Response<ItemDTO> price_retriever = ((Service_Controller)Session["service_controller"]).get_product_by_productID(product_id);

            price.Text = price_retriever.Value.Auction.Value[0];

            Response<System.Timers.Timer> timer = ((Service_Controller)Session["service_controller"]).get_timer_of_auciton(product_id + "_" + "auction" + "_timer");
            this.timer = timer.Value;



        }

        protected void auction_Click(object sender, EventArgs e)
        {
            Response<string> okay = ((Service_Controller)Session["service_controller"]).UpdateAuction(product_id, double.Parse(new_price_text.Text), creditTextBox.Text,TextOfMonth.Text,TextOfYear.Text,TextOfHolder.Text,CVVTextBox.Text, TextOfId.Text);
            if (okay.ErrorOccured)
            {
                auction_message.ForeColor = System.Drawing.Color.Red;
                auction_message.Text = okay.ErrorMessage;
            }
            else
            {
                auction_message.ForeColor = System.Drawing.Color.Green;
                auction_message.Text = okay.Value;
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {



            put_me_time.Text = "will add it later";
            
        }
    }
}