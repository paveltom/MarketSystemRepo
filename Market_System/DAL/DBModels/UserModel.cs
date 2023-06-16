using Market_System.DomainLayer.UserComponent;
using System.ComponentModel.DataAnnotations;


namespace Market_System.DAL.DBModels
{
    public class UserModel
    {
        [Key]
        public string Username { get; set; }
        public string UserID { get; set; }        
        public string HashedPassword { get; set; }
        public string State { get; set; }
        public string Address { get; set; }

        public virtual CartModel Cart { get; set; }
        

        public User ModelToUser()
        {
            User user = new User(this.Username, this.Address);
            if (this.Cart != null)
                user.SetCart(this.Cart.ModelToCart());            
            return user;
        }

        public void UpdateWholeModel(User update)
        {

        }
    }
}