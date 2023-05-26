﻿using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class Admin_operations_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            show_market_sale();
        }

        protected void add_new_admin_click(object sender, EventArgs e)
        {
            if(new_admin_name.Text.Equals("") || new_admin_name.Text.Equals("type-in new admin's name"))
            {
                add_new_admin_message.ForeColor = System.Drawing.Color.Red;
                add_new_admin_message.Text = "please type-in new Admin username";
                return;
            }
            Response<string> result = ((Service_Controller)Session["service_controller"]).AddNewAdmin(new_admin_name.Text);
            if (!result.ErrorOccured)
            {
                add_new_admin_message.ForeColor = System.Drawing.Color.Green;
                add_new_admin_message.Text = result.Value;
            }
            else
            {
                add_new_admin_message.Text = result.ErrorMessage;
                add_new_admin_message.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void show_user_click(object sender, EventArgs e)
        {
            if (member_to_show_txt.Text.Equals("") || member_to_show_txt.Text.Equals("type-in member's username"))
            {
                show_user_message.Text = "please type an username";
                show_user_message.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Response<MemberDTO> okay = ((Service_Controller)Session["service_controller"]).GetMemberInfo(member_to_show_txt.Text);
                if(okay.ErrorOccured)
                {
                    show_user_message.Text = okay.ErrorMessage;
                    show_user_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    member_info.Text = okay.Value.tostring();
                }
                
            }
        }


        protected void show_store_sale(object sender, EventArgs e)
        {
        }

            protected void remove_user_click(object sender, EventArgs e)
        {
            if (member_to_delete_txt.Text.Equals("") || member_to_delete_txt.Text.Equals("type-in member's username"))
            {
                remove_user_message.Text = "please type an username";
                remove_user_message.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Response<string> okay = ((Service_Controller)Session["service_controller"]).Remove_A_Member(member_to_delete_txt.Text);
                if (okay.ErrorOccured)
                {
                    remove_user_message.Text = okay.ErrorMessage;
                    remove_user_message.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    remove_user_message.Text = okay.Value;
                    remove_user_message.ForeColor = System.Drawing.Color.Green;
                }
                
            }
        }


        protected void show_market_sale()
        {

            string[] x = { "25/05/2023", DateTime.Now.ToShortDateString(), "22/05/2023" };
            int[] y = { 8091, 10894 ,1564};
            market_sale_chart.Series[0].Points.DataBindXY(x, y);
            market_sale_chart.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
            foreach(DataPoint point in market_sale_chart.Series[0].Points)
            {
                if (point.AxisLabel.Equals(DateTime.Now.ToShortDateString()))
                {
                    point.Color = System.Drawing.Color.Red;
                }
            }
        }
    }
}