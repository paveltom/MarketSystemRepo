using System.ComponentModel.DataAnnotations;

namespace Market_System.user_component_DAL.Models
{
    public class Product_in_basket_model
    {

        [Key]
        public string product_id { get; set; }
        public int quantity { get; set; }
    }
}