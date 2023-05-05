using Market_System.DomainLayer;
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
        Service sv;
        ItemDTO item;

        protected void Page_Load(object sender, EventArgs e)
        {
            sv = new Service();
            string product_id = Request.QueryString["product_id"];
            //Response.Write(product_id);
            item = sv.getItemDemo(product_id);
            id.Text = "Name: " + item.GetID();
            quantity.Text = "Quantity in stock: " + item.GetQuantity().ToString();
        }

        protected void addToCart_Click(object sender, EventArgs e)
        {
            int amount;
            try
            {
                amount = Int32.Parse(quantityToAdd.Text);
                if (amount > item.GetQuantity())
                {
                    clickmsg.Text = " not enough in stock";
                    clickmsg.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    sv.addToCart(item.GetID(), amount);
                    clickmsg.Text = "product added to cart";
                    clickmsg.ForeColor = System.Drawing.Color.Green;

                }
            }
            catch (FormatException)
            {
                clickmsg.Text = quantityToAdd.Text + " please write a legal number";
            }

        }
    }
}