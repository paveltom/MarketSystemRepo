﻿using System;
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

        public void Login_guset(string guest_name)
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
    }
}