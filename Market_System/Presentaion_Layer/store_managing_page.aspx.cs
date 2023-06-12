﻿using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
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

            List<string> current_shown_store_ids = new List<string>();

            foreach (ListItem item in ddl_store_id.Items)
            {

                current_shown_store_ids.Add(item.Text);

            }

            Response<List<String>> store_ids = ((Service_Controller)Session["service_controller"]).get_stores_id_that_user_works_in();


            if (store_ids.Value.All(current_shown_store_ids.Contains)) // this so it wont always load the list
            {
                return;
            }

            ddl_store_id.AppendDataBoundItems = true;
            ddl_store_id.DataSource = store_ids.Value;
            ddl_store_id.DataBind();


            manage_store_products.Visible = false;

          
        }

            


        protected void manage_store_products_Click(object sender, EventArgs e)
        {

            string selected_store_id = ddl_store_id.SelectedValue;
            if (selected_store_id.Equals("nothing_to_show"))
            {
                error_message.Text = "please enter store ID";
            }
            else
            {
                Response.Redirect(string.Format("/Presentaion_Layer/store_products_managing_page.aspx?store_id={0}", selected_store_id));
            }
        }

        protected void assign_new_manager_click(object sender, EventArgs e)
        {
            string username = new_manager_username.Text;
            string store_id = ddl_store_id.SelectedValue;
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
            string store_id = ddl_store_id.SelectedValue;
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
            string store_id = ddl_store_id.SelectedValue;
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
            
            string store_id = ddl_store_id.SelectedValue;
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
                show_managers_message.Text = "";
                show_info_list.DataSource = show_me;
                show_info_list.DataBind();
            }

        }

        protected void show_owners_click(object sender, EventArgs e)
        {
            string username = new_manager_username.Text;
            string store_id = ddl_store_id.SelectedValue;
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
                show_owners_message.Text = "";
                show_info_list.DataSource = show_me;
                show_info_list.DataBind();
            }

        }

        protected void show_purchase_history_click(object sender, EventArgs e)
        {
            string username = new_manager_username.Text;
            string store_id = ddl_store_id.SelectedValue; 
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
                show_purchase_history_message.Text = "";
            }

        }


        protected void closeStoreClick(object sender, EventArgs e)
        {
            string store_id = ddl_store_id.SelectedValue;
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

            //yotam
            if (!((Service_Controller)Session["service_controller"]).check_if_working_in_a_store().Value)
            {
                Response.Redirect("~/");
            }
            //yotam
        }
            
            
        protected void add_employee_permission_Click(object sender, EventArgs e)
        {
            string username = add_employee_permissionT.Text;

            string store_id = ddl_store_id.SelectedValue;
            string permission = add_permission.Text;
            Response<string> ok = ((Service_Controller)Session["service_controller"]).AddEmployeePermission(store_id,username, permission);
            if (ok.ErrorOccured)
            {
                Add_Permission_Message.ForeColor = System.Drawing.Color.Red;
                Add_Permission_Message.Text = ok.ErrorMessage;
                }
            else
            {
                Add_Permission_Message.ForeColor = System.Drawing.Color.Green;
                Add_Permission_Message.Text = ok.Value;
            }

        }

        protected void remove_permission_Click(object sender, EventArgs e)
        {
            string username=remove_username.Text;
            string storeID = ddl_store_id.SelectedValue;
            string permission =remove_employee_permission.Text;
            Response<string> ok = ((Service_Controller)Session["service_controller"]).RemoveEmployeePermission(storeID,username,permission);
            if (ok.ErrorOccured)
            {
                Remove_Permission_Message.ForeColor=System.Drawing.Color.Red;
                Remove_Permission_Message.Text=ok.ErrorMessage;
            }
            else
            {
                Remove_Permission_Message.ForeColor= System.Drawing.Color.Green;
                Remove_Permission_Message.Text= ok.Value;

            }
        }

        protected void selected_store_id(object sender, EventArgs e)
        {
            string storeID = ddl_store_id.SelectedValue;
            Response<bool> product_managing_checker = ((Service_Controller)Session["service_controller"]).check_if_user_can_manage_stock(storeID);
            if (product_managing_checker.Value)
            {
                manage_store_products.Visible = true;
                ManageStrategiesButton.Visible = true;
                ManagePoliciesButton.Visible = true;
            }
            else
            {
                manage_store_products.Visible = false;
            }
            Response<bool> managing_owners_and_managers_checker = ((Service_Controller)Session["service_controller"]).check_if_can_assign_manager_or_owner(storeID);
            if (managing_owners_and_managers_checker.Value)
            {
                Label6.Visible = true;
                Label8.Visible = true;
                Label7.Visible = true;
                Label15.Visible = true;
                Label16.Visible = true;
                Label17.Visible = true;
                new_owner_username.Visible = true;
                new_manager_username.Visible = true;
                owner_to_remove.Visible = true;
                assign_new_manager_button.Visible = true;
                assign_new_owner_button.Visible = true;
                owner_remove_button.Visible = true;
                daily_sales_label.Visible = true;
                store_sale_chart.Visible = true;
                show_store_sale(storeID);

            }
            else
            {
                Label6.Visible = false;
                Label8.Visible = false;
                Label7.Visible = false;
                Label15.Visible = false;
                Label16.Visible = false;
                Label17.Visible = false;
                new_owner_username.Visible = false;
                new_manager_username.Visible = false;
                owner_to_remove.Visible = false;
                assign_new_manager_button.Visible = false;
                assign_new_owner_button.Visible = false;
                owner_remove_button.Visible = false;
            }

            Response<bool> add_remove_permession_checker = ((Service_Controller)Session["service_controller"]).check_if_can_remove_or_add_permessions(storeID);
            if (add_remove_permession_checker.Value)
            {
                Label9.Visible = true;
                Label10.Visible = true;
                Label18.Visible = true;
                Label19.Visible = true;
                Label20.Visible = true;
                Label21.Visible = true;
                add_employee_permissionT.Visible = true;
                remove_username.Visible = true;
                add_permission.Visible = true;
                remove_employee_permission.Visible = true;
                add_employee_permission.Visible = true;
                remove_permission_button5.Visible = true;
            }
            else
            {
                Label9.Visible = false;
                Label10.Visible = false;
                Label18.Visible = false;
                Label19.Visible = false;
                Label20.Visible = false;
                Label21.Visible = false;
                add_employee_permissionT.Visible = false;
                remove_username.Visible = false;
                add_permission.Visible = false;
                remove_employee_permission.Visible = false;
                add_employee_permission.Visible = false;
                remove_permission_button5.Visible = false;
            }
                Response<bool> close_store_checker = ((Service_Controller)Session["service_controller"]).check_if_can_close_store(storeID);
            if (close_store_checker.Value)
            {
                Label11.Visible = true;
                closeStoreButton.Visible = true;
            }
            else
            {
                Label11.Visible = false;
                closeStoreButton.Visible = false;
            }
                Response<bool> show_info_checker = ((Service_Controller)Session["service_controller"]).check_if_can_show_infos(storeID);
            if (show_info_checker.Value)
            {
                add_product_button7.Visible = true;
                add_product_button8.Visible = true;
                add_product_button9.Visible = true;
                Label12.Visible = true;
                Label13.Visible = true;
                Label14.Visible = true;
                

            }
            else
            {
                add_product_button7.Visible = false;
                add_product_button8.Visible = false;
                add_product_button9.Visible = false;
                Label12.Visible = false;
                Label13.Visible = false;
                Label14.Visible = false;
                daily_sales_label.Visible = false;
                store_sale_chart.Visible = false;
                sdasd.Visible = false;
                store_profit_txt.Visible = false;
                store_profit.Visible = false;
                Label22.Visible = false;

            }



        }


        protected void ManageStrategiesClick(object sender, EventArgs e)
        {
            string selected_store_id = ddl_store_id.SelectedValue;
            if (selected_store_id.Equals("nothing_to_show"))
            {
                error_message.Text = "please enter store ID";
            }
            else
            {
                Response.Redirect(string.Format("/Presentaion_Layer/PurchaseStrategyManagePage.aspx?store_id={0}", selected_store_id));
            }
        }

        protected void ManagePoliciesClick(object sender, EventArgs e)
        {
            string selected_store_id = ddl_store_id.SelectedValue;
            if (selected_store_id.Equals("nothing_to_show"))
            {
                error_message.Text = "please enter store ID";
            }
            else
            {
                Response.Redirect(string.Format("/Presentaion_Layer/PurchasePolicyViewPage.aspx?store_id={0}", selected_store_id));
            }
        }

        protected void show_store_sale(string store_id)
        {

            if (store_profit_txt.Text.Equals(""))
            {
                daily_profit_of_store_message.Text = "please enter a date";
                return;
            }

            try
            {
                DateTime date = DateTime.Parse(store_profit_txt.Text);
                DateTime day_before = DateTime.Parse(store_profit_txt.Text).AddDays(-1);
                DateTime day_after = DateTime.Parse(store_profit_txt.Text).AddDays(1);
                // 
                string[] x = { day_before.ToShortDateString(), date.ToShortDateString(), day_after.ToShortDateString() };
                int[] y = { Int32.Parse(((Service_Controller)Session["service_controller"]).GetStoreProfitForDate(store_id,day_before).Value), Int32.Parse(((Service_Controller)Session["service_controller"]).GetStoreProfitForDate(store_id,date).Value), Int32.Parse(((Service_Controller)Session["service_controller"]).GetStoreProfitForDate(store_id,day_after).Value) };
                store_sale_chart.Series[0].Points.DataBindXY(x, y);
                store_sale_chart.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
                foreach (DataPoint point in store_sale_chart.Series[0].Points)
                {
                    if (point.AxisLabel.Equals(DateTime.Now.ToShortDateString()))
                    {
                        point.Color = System.Drawing.Color.Red;
                    }
                }

            }
            catch (Exception expe)
            {
                daily_profit_of_store_message.Text = expe.Message;

            }

        }





    }
}