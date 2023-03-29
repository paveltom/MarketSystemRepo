using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.Domain_Layer.User_Component;
using Market_System.Domain_Layer.Store_Component;

namespace Market_System.Domain_Layer
{
    //TODO:: Implement as a Mediator.
    public sealed class MarketSystem
    {
        private static UserFacade userFacade;
        private static StoreFacade storeFacade;
        private Random guest_id_generator;

        //This variable is going to store the Singleton Instance
        private static MarketSystem Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static MarketSystem GetInstance()
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
                        userFacade = UserFacade.GetInstance();
                        storeFacade = StoreFacade.GetInstance();
                        Instance = new MarketSystem();
                        Instance.guest_id_generator = new Random();
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
            try
            {
                userFacade.Login(username, password);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void Logout(string username)
        {
            try
            {
                userFacade.Logout(username);
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        public void register(string username, string password)
        {
            try
            {
                userFacade.register(username, password);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void login_guest()
        {
            string guest_name = "guest" + this.guest_id_generator.Next();
            try
            {
                userFacade.Login_guset(guest_name);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add_New_Store(string username, int storeID)
        {
            try
            {
                storeFacade.Add_New_Store(username, storeID);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void Add_Product_To_Store(int store_ID, string founder, Product product, int quantity)
        {
            try
            {
                storeFacade.Add_Product_To_Store(store_ID, founder, product, quantity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Remove_Product_From_Store(int store_ID, string founder, Product product)
        {
            try
            {
                storeFacade.Remove_Product_From_Store(store_ID, founder, product);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
