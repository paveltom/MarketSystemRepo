using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.User_Component
{
    //TODO:: Impelement this as a State.
    public class User
    {
        private string username;
        private User_State user_State;
        private Cart my_cart;


        public User(string username)
        {
            this.username = username;
            user_State = new Guest();
        }

        public string GetUsername()
        {
            return this.username;
        }

        public void Login()
        {
            this.user_State = new Member();
        }

        public void Logout()
        {
            this.user_State = new Guest();
        }
        public string GetUserState()
        {
            return this.user_State.GetUserState();
        }

        public void add_product_to_basket(string product_id)
        {
            this.my_cart.add_product(product_id);
        }

        internal Cart getcart()
        {
            return this.my_cart;
        }

        public void update_total_price_of_cart(double price)
        {
            this.my_cart.update_total_price(price);
        }
    }
}
