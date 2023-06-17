using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class lottery_page : System.Web.UI.Page
    {
        private string product_id;
        private TimerPlus timer;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.product_id = Request.QueryString["product_id"];
            product_id_label.Text = product_id;
            Response<ItemDTO> name_retriever = ((Service_Controller)Session["service_controller"]).get_product_by_productID(product_id);
            product_name.Text = name_retriever.Value.Name;


            Response<TimerPlus> timer = ((Service_Controller)Session["service_controller"]).get_timer_of_auciton(product_id + "_" + "lottery" + "_timer");
            if (!timer.ErrorOccured)
            {
                this.timer = timer.Value;
            }
            else
            {
                Response.Redirect("/Presentaion_Layer/ProductsPage.aspx");
            }
        }

        protected void lottery_Click(object sender, EventArgs e)
        {
            Response<string> okay = ((Service_Controller)Session["service_controller"]).AddLotteryTicket(product_id, int.Parse(percentage_text.Text), creditTextBox.Text, TextOfMonth.Text, TextOfYear.Text, TextOfHolder.Text, CVVTextBox.Text, TextOfId.Text);
            if (okay.ErrorOccured)
            {
                lottery_message.ForeColor = System.Drawing.Color.Red;
                lottery_message.Text = okay.ErrorMessage;
            }
            else
            {
                lottery_message.ForeColor = System.Drawing.Color.Green;
                lottery_message.Text = okay.Value;
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {



            put_me_time.Text = timer.MinutesRemains().ToString();

        }
    }
}