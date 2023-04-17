using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class UserFacade
    {
        private static UserRepo userRepo;
        private static List<User> users;
        private static Dictionary<string, string> userID_sessionID_linker; // key = session_id    value=userID
        
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
                        userID_sessionID_linker = new Dictionary<string, string>();
                        
                        
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

        internal string get_userID_from_session(string session_id)
        {
            return userID_sessionID_linker[session_id];
        }

        public void Logout(string username)
        {
            
            foreach(User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    if (!user.GetUserState().Equals("Guest"))
                    {
                        user.Logout();
                        
                        return;
                    }
                    else
                    {

                        throw new ArgumentException("You're already logged-out");
                    }
                    
                }
            }

            throw new ArgumentException("user does not exists");

        }


        public void register(string username, string password,string address)
        {
            foreach (User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    throw new Exception("a user with same name exists, please change name!");
                }
            }
            users.Add(new User(username,address));
            string new_user_id=userRepo.register(username, password);

             
         
        }

       

        public void add_product_to_basket(string product_id, string username,int quantity)
        {
           
            foreach (User u in users)
            {
              if (u.GetUsername().Equals(username))
                {
                    u.add_product_to_basket(product_id,quantity);
                }
            }
        }



         public string get_username_from_user_id(string userid)
        {
            return userRepo.get_username_from_userID(userid);
        }
        internal void update_cart_total_price(string username, double price)
        {
            
            foreach (User u in users)
            {
                if(u.GetUsername().Equals(username))
                {
                    u.update_total_price_of_cart(price);
                }
            }
        }
        public string get_address_of_username(string username)
        {
            foreach (User u in users)
            {
                if (u.GetUsername().Equals(username))
                {
                    return u.get_Address();
                }
            }

            throw new Exception("user does not exists");
        }
        public Cart get_cart(string username)
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

        public void remove_product_from_basket(string product_id, string username)
        {
            
            foreach (User u in users)
            {
                if (u.GetUsername().Equals(username))
                {
                    u.remove_product_from_basket(product_id);
                }
            }
        }

        public string change_password( string username, string new_password)
        {
            
            userRepo.change_password(username, new_password);
            return username+" changed password successfully";
        }

        public void link_user_with_session(string username, string session_id)
        {
            try
            {
                string user_id = userRepo.get_userID_from_username(username);
                userID_sessionID_linker.Add(session_id, user_id);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        internal void unlink_userID_with_session(string session_id)
        {
            userID_sessionID_linker.Remove(session_id);
        }

        public void Login_guset(string guest_name)
        {
            users.Add(new User(guest_name,null));
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

        //[Throws Exception]
        public string Get_User_State(string username)
        {
            foreach (User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    return user.GetUserState();
                }
            }

            throw new Exception("User doesn't exist!");
        }

        public bool check_if_user_is_logged_in(string username)
        {
            
            foreach(User u in users)
            {
                if(u.GetUsername().Equals(username))
                {
                    return u.GetUserState().Equals("Member");
                }
            }
            return false;
        }

        public List<PurchaseHistoryObj> get_purchase_history_of_a_member(string user_id)
        {
            string username = userRepo.get_username_from_userID(user_id);
            if (check_if_user_is_logged_in(username))
            {
                return PurchaseRepo.GetInstance().get_history(username);
            }
           else
            {
                throw new Exception("user is not logged in! or he does not exists");
            }
        }
        /*
        public void Check_Delivery(string username)
        {
            if (Check_Delivery_Availability(username))
            {
                return;
            }
            else
            {
                throw new Exception("Delivery is not available");
            }
        }

        private bool Check_Delivery_Availability(string username)
        {
            //TODO:: Check here with the dilvery company...
            //חשוב: לשריין את המשלוח פה.... !!!!!!
            return true;
        }
        */
        public void Check_Out(string username)
        {
            if (Check_Payment(username))
            {
                return;
            }
            else
            {
                throw new Exception("Payment has failed, either your cart is empty");
            }
        }
        private bool Check_Payment(string username)
        {
            //TODO:: Check here with the payment company...
            foreach(User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    Cart cart = user.getcart();
                    if(cart == null) //Empty cart, no products...
                    {
                        return false; 
                    }
                    //get cart here and pay for the products here...
                    //save in the history of purchases too!

                    //if payment was successfull return true:
                    return true;
                }
            }
            return false;
        }

        internal void save_purhcase_in_user(string username,Cart cart)
        {
            PurchaseRepo.GetInstance().save_purchase(username, new PurchaseHistoryObj(username, cart.gett_all_baskets(), cart.get_total_price()));
        }
    }
}