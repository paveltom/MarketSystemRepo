using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market_System.user_component_DAL.Models
{
    public class Product_in_basket_model
    {

        [Key]
        public string product_id { get; set; }
        public int quantity { get; set; }
        [ForeignKey("basket_id")]
        public string basket_id { get; set; }
        

        public Product_in_basket_model()
        {

        }

        public Product_in_basket_model(string product_id,string basket_id)
        {
            this.product_id = product_id;
            this.quantity = 0;
            this.basket_id = basket_id;
        }

        internal void update_quantity_of_product_id(int new_quantity)
        {
            this.quantity = new_quantity;
        }
    }
}