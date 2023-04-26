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
        private static Dictionary<string, string> userID_sessionID_linker; // key = session_id    value=userID if it is a guest then guest username
        private static List<string> Admins; //saved by the userID
        //*  Admin - 6.1, 6.2, 6.3, 6.4, 6.5 הרשאות מנהל מערכת=מנהל שוק
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
                        Admins = new List<string>();

                        //Register an admin:
                        userRepo.AddFirstAdmin("admin");

                        //Critical Section End
                        //Once the thread releases the lock, the other thread allows entering into the critical section
                        //But only one thread is allowed to enter the critical section
                    }
                }
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
                    try
                    {
                        string user_id = userRepo.get_userID_from_username(username);
                        if (userRepo.CheckIfAdmin(username, username)) //Check if admin - login as admin if so
                        {
                            if (!Admins.Contains(user_id))
                            {
                                Admins.Add(user_id);
                            }
                            user.AdminLogin();
                        }
                    }

                    catch(Exception e)
                    {
                        user.Login();
                    }
                    return;
                }
            }

            throw new ArgumentException("Incorrect login information has been provided");
        }

        internal void link_guest_with_session(string guest_name, string session_id)
        {
            userID_sessionID_linker.Add(session_id, guest_name);
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
            try
            {
                foreach (User user in users)
                {
                    if (user.GetUsername().Equals(username))
                    {
                        throw new Exception("a user with same name exists, please change name!");
                    }
                }
                userRepo.register(username, password);
                users.Add(new User(username, address));
            }

            catch (Exception e)
            {
                throw e;
            }

          //  users.Add(new User(username,address));
           // string new_user_id=userRepo.register(username, password);

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


        public string get_user_id_from_username(string username)
        {

            return userRepo.get_userID_from_username(username);
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

        public void remove_product_from_basket(string product_id, string username,int quantity)
        {
            
            foreach (User u in users)
            {
                if (u.GetUsername().Equals(username))
                {
                    u.remove_product_from_basket(product_id, quantity);
                }
            }
        }

        public string change_password( string username, string new_password)
        {
            try
            {
                userRepo.change_password(username, new_password);
                return username + " changed password successfully";
            }

            catch(Exception e)
            {
                throw e;
            }
        }

        public bool isLoggedInAdministrator(string user_ID, string username)
        {
            foreach (User u in users)
            {
                if (u.GetUsername().Equals(username) && u.GetUserState().Equals("Administrator") && CheckIfAdmin(user_ID, username))
                {
                    return true;
                }
            }

            throw new Exception("The user is not an administrator of the system");
        }

        public void link_user_with_session(string username, string session_id)
        {
            try
            {
                string user_id = userRepo.get_userID_from_username(username);
                userID_sessionID_linker.Add(session_id, user_id);
                try
                {
                    if (userRepo.CheckIfAdmin(user_id, user_id)) //If the logged-in user is an admin - add it to the list
                    {
                        Admins.Add(user_id);
                        return;
                    }
                }

                catch(Exception e)
                {
                    return;
                }
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
                    return !u.GetUserState().Equals("Guest");
                }
            }
            return false;
        }

        public List<PurchaseHistoryObj> get_purchase_history_of_a_member(string username)
        {
            
            if (check_if_user_is_logged_in(username))
            {
                return PurchaseRepo.GetInstance().get_history(username);
            }
           else
            {
                throw new Exception("user is not logged in! or he does not exists");
            }
        }

        public List<PurchaseHistoryObj> get_purchase_history_of_other_member(string sessionID, string otherUsername)
        {
            string currUserID = get_user_id_from_session_id(sessionID);
            string currUsername = get_username_from_user_id(currUserID);
            if (CheckIfAdmin(sessionID, currUsername) && check_if_user_is_logged_in(currUsername))
            {
                return PurchaseRepo.GetInstance().get_history(otherUsername);
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

        public void save_purhcase_in_user(string user_id,Cart cart)
        {
            
            string username = get_username_from_user_id(user_id);
            PurchaseRepo.GetInstance().save_purchase(username, new PurchaseHistoryObj(username, cart.gett_all_baskets(), cart.get_total_price()));
        }

        public void AddNewAdmin(string curr_Admin_Session_ID, string Other_username)
        {
            try
            {
                string user_id_1 = userID_sessionID_linker[curr_Admin_Session_ID];
                string curr_Admin_userName = get_username_from_user_id(user_id_1);
                string user_id_2 = userRepo.get_userID_from_username(Other_username);

                //The admin exists and is logged-in -> State == Admin
                if (Admins.Contains(user_id_1) && !Admins.Contains(user_id_2) && getUserfromUsersByUsername(curr_Admin_userName).GetUserState().Equals("Administrator"))
                {
                    userRepo.AddNewAdmin(curr_Admin_userName, Other_username);
                    Admins.Add(user_id_2);
                }

                else
                {
                    throw new Exception("Admin cannot be added (already exists, or the performing user isn't an admin or isn't logged-in)");
                }
            }

            catch(Exception e)
            {
                throw e;
            }
        }

        public bool CheckIfAdmin(string curr_Admin_Session_ID, string Other_username)
        {
            try
            {
                string user_id_1 = userID_sessionID_linker[curr_Admin_Session_ID];
                string curr_Admin_userName = get_username_from_user_id(user_id_1);
                string user_id_2 = userRepo.get_userID_from_username(Other_username);

                //The admin exists and is logged-in -> State == Admin
                if (Admins.Contains(user_id_1) && Admins.Contains(user_id_2) && getUserfromUsersByUsername(curr_Admin_userName).GetUserState().Equals("Administrator")) 
                {
                    return userRepo.CheckIfAdmin(curr_Admin_userName, Other_username);
                }

                else if (Admins.Contains(user_id_1) && !Admins.Contains(user_id_2) && getUserfromUsersByUsername(curr_Admin_userName).GetUserState().Equals("Administrator"))
                {
                    return false;
                }
                else
                {
                    throw new Exception("The performing Member isn't an admin or he isn't logged-in");
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string get_session_id_from_username(string username)
        {
            string userid = userRepo.get_userID_from_username(username);
            foreach(string session_id in userID_sessionID_linker.Keys)
            {
                if(userID_sessionID_linker[session_id].Equals(userid))
                {
                    return session_id;
                }
            }
            return null;
        }

        public string get_user_id_from_session_id(string session_id)
        {
            if (userID_sessionID_linker.ContainsKey(session_id))
            {
                return userID_sessionID_linker[session_id];
            }
            return null;
        }

        private User getUserfromUsersByUsername(string username)
        {
            foreach (User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    return user;
                }
            }

            return null;
        }

        internal void reset_cart(string session_id)
        {
            string userid = get_userID_from_session(session_id);
            string username = get_username_from_user_id(userid);
            foreach (User user in users)
            {
                if(user.GetUsername().Equals(username))
                {
                    user.reset_cart();
                }
            }
        }

        public void Remove_A_Member(string member_Username)
        {
            try
            {
                string user_ID = get_user_id_from_username(member_Username);

                //remove from Users list
                foreach(User user in users.ToList())
                {
                    if (user.GetUsername().Equals(member_Username))
                    {
                        users.Remove(user);
                        break;
                    }
                }

                //remove from UserRepo
                userRepo.Remove_A_User(member_Username, user_ID);

                //remove from linker
                foreach(KeyValuePair<string, string> pair in userID_sessionID_linker.ToList())
                {
                    if (pair.Value.Equals(user_ID))
                    {
                        userID_sessionID_linker.Remove(pair.Key);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal User getUser(string username)
        {
            foreach(User user in users)
            {
                if (user.GetUsername().Equals(username))
                {
                    return user;
                }
            }
            return null;
        }
    }
}