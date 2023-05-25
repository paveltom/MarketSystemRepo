﻿using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class checkoutPage : System.Web.UI.Page
    {
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                adressTextBox.Focus();
               // username = (string)Session["username"];
                //filling adress textbox (if it is a member)
                Response<String> response = ((Service_Controller)Session["service_controller"]).getAdressOrEmpty();
                if (response.ErrorOccured)
                {
                    adressTextBox.Text = response.ErrorMessage;
                }
                else adressTextBox.Text = response.Value;

            }
            availDelivery();
            Response<String> price = ((Service_Controller)Session["service_controller"]).calculataePrice();
            if (price.ErrorOccured)
            {
                priceLabel2.Text = price.ErrorMessage;
            }
            else priceLabel2.Text = price.Value;

        }

        protected void payClick(object sender, EventArgs e)
        {
            Response<String> response = ((Service_Controller)Session["service_controller"]).check_out(creditTextBox.Text,TextOfMonth.Text,TextOfYear.Text,TextOfHolder.Text,CVVTextBox.Text, TextOfId.Text);
            if (response.ErrorOccured)
            {
                payButtonMsg.Text = response.ErrorMessage;
                payButtonMsg.ForeColor = Color.Red;
            }
            else
            {
                payButtonMsg.Text = response.Value;
                payButtonMsg.ForeColor = Color.Green;
            }
            Page_Load(null,null);
        }

        protected void adressTextBox_TextChanged(object sender, EventArgs e)
        {
            availDelivery();
        }

        protected void availDelivery()
        {
            //checking adress for making availble the pay button
            Response<String> availDelivery = ((Service_Controller)Session["service_controller"]).check_delivery(TextOfName.Text, adressTextBox.Text,CityTextBox.Text,CountryTextBox.Text,ZipTextBox.Text);
            if (availDelivery.ErrorOccured)
            {
                payButton.Enabled = false;
            }
            else payButton.Enabled = true;
        }
    }
}