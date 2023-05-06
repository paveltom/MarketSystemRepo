using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System
{
    public partial class SiteMaster : MasterPage
    {
        
        private string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (this.service_controller == null)
            { 
            this.service_controller = new ServiceLayer.Service_Controller();
                this.username = " ";
            }  
            if(!this.username.Equals(" "))
            {
                this.a_user_logs_in();
            }
            */
            if (Session["service_controller"] == null)
            {
                Session["service_controller"] = new ServiceLayer.Service_Controller();
                Session["username"] = ""; 
               // logout_button.Visible = false ;
              // change_password_button.Visible = false;
            }

            if (Session["username"].Equals(""))
            {
                username_hello_label.Text = "";
                Logout_href.Visible = false;
                notfications_href.Visible = false;
                change_password_button.Visible = false;
                open_new_store_button.Visible = false;
                manage_store_button.Visible = false;
                admin_spec_ops_button.Visible = false;
                Label1.Text = "";
            }
            else
            {
                a_user_logs_in();
                username_hello_label.Text = "hello " + Session["username"] + " !";
                 Label1.Text="hello " + Session["username"] + " !";
            }
           


        }

        internal void set_username_message(string hello)
        {
            Label1.Text = hello;
        }

        public string get_username()
        {
            return this.username;
        }

        public void set_username(string username)
        {
            this.username = username;
        }

        public void a_user_logs_in()
        {
            
            
            notfications_href.Visible = true;
            login_href.Visible = false;
            register_href.Visible = false;
          
            Logout_href.Visible = true; ;
            change_password_button.Visible = true;
            open_new_store_button.Visible = true;
            if(((Service_Controller)Session["service_controller"]).check_if_working_in_a_store().Value)
            {
                manage_store_button.Visible = true;
            }
            else
            {
                manage_store_button.Visible = false;
            }
            if (((Service_Controller)Session["service_controller"]).HasNewMessages())
            {
                notification_messages.Text = "you have a new message!";
            }
            if (((Service_Controller)Session["service_controller"]).CheckIfAdmin((string)Session["username"]).Value.Equals("The user is an admin"))
            {
                admin_spec_ops_button.Visible = true;
            }





        }

        public void Logout_click(object sender, EventArgs e)
        {
            Service_Controller sv = (Service_Controller)Session["service_controller"];
            Response<string> result = sv.log_out();
            Session["username"] = "";
            login_href.Visible = true;
            register_href.Visible = true;
            Response.Redirect("~/");

        }

        public void change_password_click(object sender, EventArgs e)
        {

            Response.Redirect("/Presentaion_Layer/change_password_page.aspx");

        }

        public void open_new_store_click(object sender, EventArgs e)
        {

            Response.Redirect("/Presentaion_Layer/open_new_store_page.aspx");

        }

        public void store_managing_click(object sender, EventArgs e)
        {

            Response.Redirect("/Presentaion_Layer/store_managing_page.aspx");

        }

        public void spec_ops_click(object sender, EventArgs e)
        {

            Response.Redirect("/Presentaion_Layer/Admin_operations_page.aspx");

        }







    }
}