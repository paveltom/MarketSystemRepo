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
    public partial class ProductsPage : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ListView1.Items.Clear();
            Response<List<ItemDTO>> items = ((Service_Controller)Session["service_controller"]).get_products_from_all_shop();
            
            List<string> list = new List<string>();
            foreach (ItemDTO item in items.Value)
            {
                list.Add("id: " + item.GetID() +"    name: "+item.get_name()+ "       quantity: " + item.GetQuantity());
            }

            ListView1.DataSource = list;
            ListView1.DataBind();

        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Presentaion_Layer/ProductPage.aspx?product_id={0}", product_id_text.Text));
        }
    }

  /*public class Service
    {
        public Service() { }

        public List<ItemDTO> getItemsDemo()
        {
            List<ItemDTO> items = new List<ItemDTO>();
            items.Add(new ItemDTO("item1", 1));
            items.Add(new ItemDTO("item2", 2));
            items.Add(new ItemDTO("prod3", 3));
            items.Add(new ItemDTO("prod4", 4));
            return items;
        }

        public ItemDTO getItemDemo(string id)
        {
            return new ItemDTO(id, 100);
        }

        internal void addToCart(string v, int amount)
        {

        }

    }
  */
}