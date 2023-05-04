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
    public partial class Products : System.Web.UI.Page
    {
        Service_Controller sv;
        List<ItemDTO> itemsInView;
        protected void Page_Load(object sender, EventArgs e)
        {
            //todo -> return this line in the end sv = (Service_Controller)Session["service_controller"];
            sv = new Service_Controller();
            
            itemsInView = sv.getRandomProducts();
            //clear the list view
            ListView1.Items.Clear();
            List<string> itemsList = new List<string>();
           for(int i = 0; i < itemsInView.Count; i++)
            {
                string item = "name: "+itemsInView[i].GetID()+"\n\tquantity: "+itemsInView[i].GetQuantity().ToString()+"<br />";
                itemsList.Add(item);

            }  
            ListView1.DataSource = itemsList.ToArray();
            ListView1.DataBind();
        }

        protected void searchByNameBot_Click(object sender, EventArgs e)
        {
           // Response.Write(searchByNameBox.Text);
            //Session["productNameSearch"] = searchByNameBox.Text;
            //update list of products by the 'productNameSearch'name
        }
    }
}