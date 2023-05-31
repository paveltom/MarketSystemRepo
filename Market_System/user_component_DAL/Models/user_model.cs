using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    public class user_model
    {
        [Key]
        public string username { get; set; }

        public Cart_model my_cart { get; set; }
        public string address { get; set; }
        public bool is_admin { get; set; }
        public string hashed_password { get; set; }
        public string user_ID { get; set; }
        public string user_state { get; set; }


        public user_model()
        {

        }
        public user_model(string username1,string address1, bool is_admin,string user_id,string hashed_pass)
        {
            this.username = username1;
            this.address = address1;
            this.my_cart = new Cart_model();
            this.is_admin = is_admin;
            this.hashed_password = hashed_pass;
            this.user_ID = user_id;
            this.user_state = "Guest";
        }
    }
}