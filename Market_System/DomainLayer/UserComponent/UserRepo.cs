using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class UserRepo
    {
        private static Dictionary<string, string> userDatabase;
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
                        userID_generator = new Random();
                        Instance = new UserRepo();
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
            if(userDatabase.TryGetValue(username, out pass) && password.Equals(pass))
            {
                return true;
            }
            return false;
        }

        public string register(string username, string password)
        {
            userDatabase.Add(username, password);
            string new_user_id = userID_generator.Next().ToString();
            while (!unique_user_ID(new_user_id))
            {
                new_user_id = userID_generator.Next().ToString();
            }
            user_ID_username_linker.Add(new_user_id, username);
            return new_user_id;

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

        internal string get_username_from_userID(string userID)
        {
            foreach (string userid in user_ID_username_linker.Keys)
            {
                if (userid.Equals(userID))
                {
                    return user_ID_username_linker[userID];
                }
            }
            throw new Exception("can't recive username because userID does not exists");
        }

        internal void change_password(string username, string new_password)
        {
            userDatabase[username] = new_password;
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
    }
}