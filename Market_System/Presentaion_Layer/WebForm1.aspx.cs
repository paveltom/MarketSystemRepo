using Market_System.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] strings = {"a",  "b", "c"};
            ListView1.DataSource = strings;
            ListView1.DataBind();
        }

    }
}