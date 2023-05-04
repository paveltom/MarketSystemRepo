using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Communication_Component
{
    public class NotificationFacade
    {
        //This variable is going to store the Singleton Instance
        private static NotificationFacade Instance = null;
        private static NotificationRepo notificationRepo = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static NotificationFacade GetInstance()
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
                        notificationRepo = NotificationRepo.GetInstance();
                        Instance = new NotificationFacade();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        /*public Message SendMessage(string message, string from) 
        {
            return new Message(message, from);
        }*/

        public void AddNewMessage(string userID, string from, string mesg) //from = userID (if from user), storeID(if from store), or 'System'
        {
            try
            {
                Message message = new Message(mesg, from);
                notificationRepo.addNewMessage(userID, message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> GetMessages(string userID)
        {
            try
            {
                List<string> messages = new List<string>();
                foreach (Message message in notificationRepo.GetMessages(userID))
                {
                    messages.Add(message.GetAndReadMessage());
                }
                return messages;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal bool HasNewMessages(string userID)
        {
            try
            {
                foreach (Message message in notificationRepo.GetMessages(userID))
                {
                    if (message.IsNewMessage())
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
