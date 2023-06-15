using Market_System.DomainLayer.UserComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
            return user;
        }

        public void UpdateWholeModel(User update)
        {

        }
    }
}