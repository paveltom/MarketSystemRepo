using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market_System.user_component_DAL.Models
{
    public class Product_in_basket_history_model
    {

        [Key]
        public string product_id { get; set; }
        public int quantity { get; set; }
        [ForeignKey("basket_history_id")]
        public string basket_history_id { get; set; }


        public Product_in_basket_history_model()
        {

        }

        public Product_in_basket_history_model(string product_id,int quantity,string basket_history_id)
        {
            this.product_id = product_id;
            this.quantity = quantity;
            this.basket_history_id = basket_history_id;
        }
    }
}