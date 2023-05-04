using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class myShops : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMyShops();
            }
        }

        public void LoadMyShops()
        {
            ListItem shop1 = new ListItem("store1");
            //add to list: ###.Items.Add(shop1);//todo
            ListItem shop2 = new ListItem("store2");
            ListItem shop3 = new ListItem("store3");

        }

        protected void Unnamed2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void shops(object sender, EventArgs e)
        {

        }
    }
}