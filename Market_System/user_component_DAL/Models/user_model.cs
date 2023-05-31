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


        public user_model()
        {

        }
        public user_model(string username1,string address1)
        {
            this.username = username1;
            this.address = address1;
            this.my_cart = new Cart_model();
        }
    }
}