using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.User_Component
{
    public class UserFacade
    {
        private static UserRepo userRepo;
        private static List<User> users;
        //This variable is going to store the Singleton Instance
        private static UserFacade Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static UserFacade GetInstance()
        {
            //This is thread-Safe - Performing a double-lock check.
            if (Instance == null)
            {
                //As long as one thread locks the resource, no other thread can access the resource
                //As long as one thread enters into the Critical Section, 
                //no other threads are allowed to enter the critical section
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        users = new List<User>();
                        userRepo = UserRepo.GetInstance();
                        Instance = new UserFacade();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        public void Login(string username, string password)
        {
            foreach(User user in users)
            {
                if(user.GetUsername().Equals(username) && userRepo.checkIfExists(username, password))
                {
                    user.Login();
                    return;
                }
            }

            throw new ArgumentException("Incorrect login information has been provided");
        }

        public void Logout(string username)
        {
            foreach(User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    user.Logout();
                }
            }

            throw new ArgumentException("You're already logged-out");
        }


        public void register(string username, string password)
        {
            foreach (User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    throw new Exception("a user with same name exists, please change name!");
                }
            }
            users.Add(new User(username));
            userRepo.register(username, password);
        }

        internal void add_product_to_basket(string product_id, string username)
        {
            foreach (User u in users)
            {
              if (u.GetUsername().Equals(username))
                {
                    u.add_product_to_basket(product_id);
                }
            }
        }

        internal void update_cart_total_price(string username, double price)
        {
           foreach(User u in users)
            {
                if(u.GetUsername().Equals(username))
                {
                    u.update_total_price_of_cart(price);
                }
            }
        }

        internal Cart get_cart(string username)
        {
            foreach (User u in users)
            {
                if (u.GetUsername().Equals(username))
                {
                    return u.getcart();
                }
            }

            throw new Exception("user does not exists");
            
        }

        internal void Login_guset(string guest_name)
        {
            users.Add(new User(guest_name));
            //TODO:: remove the guest when he leaves...
        }

        //this for tests , so after each test we can destroy the singleton to start brand new test without previous data
        public void Destroy_me()
        {
            Instance = null;
        }

        public bool check_if_user_exists(string user_name,string pass)
        {
            return userRepo.checkIfExists(user_name, pass);
        }

        public bool check_if_user_is_logged_in(string username)
        {
            foreach(User u in users)
            {
                if(u.GetUsername().Equals(username))
                {
                    return u.get_user_state().tostring().Equals("member");
                }
            }
            return false;

        }
    }
}