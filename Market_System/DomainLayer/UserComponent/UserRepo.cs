using Market_System.Domain_Layer.Communication_Component;
using Market_System.user_component_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class UserRepo
    {

        private static Dictionary<string, string> userDatabase;
        //private static Dictionary<string, List<Message>> messages; //key = userID
        private static List<string> Admins; //saved by username
        private static Dictionary<string, string> user_ID_username_linker; // key is user ID , val is username
        private static Random userID_generator;


        private static UserRepo Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static UserRepo GetInstance()
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
                        userDatabase = new Dictionary<string, string>();
                        Admins = new List<string>();
                        user_ID_username_linker = new Dictionary<string, string>();
                        userID_generator = new Random();
                        Instance = new UserRepo();    
                        //messages = new Dictionary<string, List<Message>>();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        public void destroy_me()
        {
            Instance = null;
            
        }

        public bool checkIfExists(string username, string password)
        {
            var pass = "";
            if(userDatabase.TryGetValue(username, out pass) && PasswordHasher.VerifyPassword(password, pass))
            {
                return true;
            }
            return false;
        }

        internal void remove_guest_id_from_userRepo(string guest_id)
        {
            user_ID_username_linker.Remove(guest_id);
        }

        internal void AddFirstAdmin(string username)
        {
            Admins.Add(username);
        }

        public string register(string username, string password)
        {
            string hashed_Password = PasswordHasher.HashPassword(password);
            userDatabase.Add(username, hashed_Password);
            
            string new_user_id = userID_generator.Next().ToString();
            while (!unique_user_ID(new_user_id))
            {
                new_user_id = userID_generator.Next().ToString();
            }
            user_ID_username_linker.Add(new_user_id, username);
            return new_user_id;

        }

        public void Remove_A_User(string username, string userID)
        {
            userDatabase.Remove(username);
            user_ID_username_linker.Remove(userID);
        }

        private bool unique_user_ID(string new_user_id)
        {
            foreach (string user_id in user_ID_username_linker.Keys)
            {
                if(user_id.Equals(new_user_id))
                {
                    return false;
                }
            }
            return true;
        }

        internal string link_guest_with_user_id(string guest_name)
        {
            string new_user_id = userID_generator.Next().ToString();
            while (!unique_user_ID(new_user_id))
            {
                new_user_id = userID_generator.Next().ToString();
            }
            user_ID_username_linker.Add(new_user_id, guest_name);
            return new_user_id;
        }

        internal string get_username_from_userID(string userID)
        {
            foreach (string userid in user_ID_username_linker.Keys)
            {
                if (userid.Equals(userID))
                {
                    return user_ID_username_linker[userID];
                }
            }
            
            user_model um = User_component_context.GetInstance().GetUserByUserID(userID);
            if(um!=null)
            {
                return um.username;
            }
            throw new Exception("can't recive username because userID does not exists");

        }

        internal void change_password(string username, string new_password)
        {
            if(PasswordHasher.VerifyPassword(new_password, userDatabase[username]))
            {
                throw new Exception("You can't change a password to the same one, you need to provide an other new password");
            }

            string new_hashed_Password = PasswordHasher.HashPassword(new_password);
            userDatabase[username] = new_hashed_Password;
        }

        internal string get_userID_from_username(string username)
        {
            foreach (KeyValuePair<string, string> entry in user_ID_username_linker)
            {
                if(entry.Value.Equals(username))
                {
                    return entry.Key;
                }
            }
            throw new Exception("can't recive userID because username does not exists");
        }



        public void AddNewAdmin(string curr_Admin_username, string other_username)
        {
            if (curr_Admin_username == null)//it means it is the inizilating of the system
            {
                Admins.Add(other_username);
            }
            else
            {
                if (Admins.Contains(curr_Admin_username) && !Admins.Contains(other_username))
                {
                    Admins.Add(other_username);
                }

                else
                {
                    throw new Exception("Admin cannot be added (already exists, or the performing user isn't an admin)");
                }
            }
        }

        public bool CheckIfAdmin(string curr_Admin_username, string other_username)
        {
            if (Admins.Contains(curr_Admin_username) && Admins.Contains(other_username))
            {
                return true;
            }

            else if (Admins.Contains(curr_Admin_username) && !Admins.Contains(other_username))
            {
                return false; //userID isn't an admin
            }

            else
            {
                throw new Exception("The checking user isn't an Admin");
            }
        }

        /*
        public void addNewMessage(string userID, Message message)
        {
            try
            {
                if (messages.ContainsKey(userID))
                {
                    messages[userID].Add(message);
                }
                else
                {
                    messages.Add(userID, new List<Message> { message });
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public List<Message> GetMessages(string userID)
        {
            return messages[userID];
        }*/
    }
}