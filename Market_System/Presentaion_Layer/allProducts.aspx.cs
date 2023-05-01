using Market_System.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class allProducts : System.Web.UI.Page
    {
        Service_Controller sv;
        protected void Page_Load(object sender, EventArgs e)
        {
            sv = (Service_Controller)Session["service_controller"];
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<Market_System.DomainLayer.ItemDTO> productList_GetData()
        {
            return sv.get_products_from_shop("store1").Value.AsQueryable();
        }
    }
}