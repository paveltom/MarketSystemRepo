using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    public class UserRepo
    {
        private static Dictionary<string, string> userDatabase;
        

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

        public void register(string username, string password)
        {
            userDatabase.Add(username, password);
        }

        
    }
}