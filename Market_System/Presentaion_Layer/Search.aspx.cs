using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market_System.Presentaion_Layer
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void searchItems(object sender, EventArgs e)
        {
            /*
                string searchValue = searchBar.Text.ToLower();

                for (let i = 0; i < items.length; i++)
                {
                    const itemText = items[i].textContent.toLowerCase();

                    if (itemText.includes(searchValue))
                    {
                        items[i].style.display = '';
                    }
                    else
                    {
                        items[i].style.display = 'none';
                    }
                }
            */
            
        }

        
             protected void filterItems(object sender, EventArgs e)
        {
            /*
            const categoryValue = categorySelect.value;

            for (let i = 0; i < items.length; i++)
            {
                const itemCategory = items[i].classList.contains(categoryValue);

                if (categoryValue === '' || itemCategory)
                {
                    items[i].style.display = '';
                }
                else items[i].style.display = 'none';
            }
            */
        }
    }
}