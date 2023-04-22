using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    //TODO:: Impelement this as a State.
    public class User
    {
        private string username;
        private User_State user_State;
        private Cart my_cart;
        private string address;


        public User(string username,string address)
        {
            this.username = username;
            user_State = new Guest();
            this.my_cart = new Cart();
            this.address = address;
        }

        public string GetUsername()
        {
            return this.username;
        }

        public string get_Address()
        {
            return this.address;
        }
        public void Login()
        {
            this.user_State = new Member();
        }

        public void AdminLogin()
        {
            this.user_State = new Administrator();
        }

        public void Logout()
        {
            this.user_State = new Guest();
        }
        public string GetUserState()
        {
            return this.user_State.GetUserState();
        }

        public void add_product_to_basket(string product_id,int quantity)
        {
            this.my_cart.add_product(product_id,quantity);
        }

        internal Cart getcart()
        {
            return this.my_cart;
        }

        public void update_total_price_of_cart(double price)
        {
            this.my_cart.update_total_price(price);
        }

        internal void remove_product_from_basket(string product_id)
        {
            this.my_cart.remove_product(product_id);
        }

        internal void reset_cart()
        {
            my_cart = new Cart();
        }
    }
}
