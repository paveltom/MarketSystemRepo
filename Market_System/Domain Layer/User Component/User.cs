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

          public User_State get_user_state()
        {
            return this.user_State;
        }
    }
}
