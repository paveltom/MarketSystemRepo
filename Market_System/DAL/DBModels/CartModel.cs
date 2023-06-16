using Market_System.DomainLayer.UserComponent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Market_System.DAL.DBModels
{
    public class CartModel
    {
        [Key]
        public string CartID { get; set; } // username+Cart
        public double TotalPrice { get; set; }
        public virtual ICollection<BucketModel> Buckets { get; set; }


        public Cart ModelToCart()
        {
            Cart cart = new Cart();
            cart.update_total_price(this.TotalPrice);
            cart.SetBuckets(this.Buckets.ToList().Select(b => b.ModelToBucket()).ToList());
            return cart;
        }

    }
}