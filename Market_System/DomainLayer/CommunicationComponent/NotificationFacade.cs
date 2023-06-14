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

        public delegate void NotificationEventHandler(object sender, string userID);
        public static event NotificationEventHandler NotificationEvent;

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

        private static object addNewMessage = new object();
        public void AddNewMessage(string userID, string from, string mesg) //from = userID (if from user), storeID(if from store), or 'System'
        {
            lock (addNewMessage)
            {
                try
                {
                    Message message = new Message(mesg, from);
                    message.to = userID;
                    notificationRepo.addNewMessage(userID, message);

                    // Raise the event
                    NotificationEvent?.Invoke(this, userID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private static object getMessages = new object();
        public List<string> GetMessages(string userID)
        {
            lock (getMessages)
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
        }

        private static object hasMessages = new object();
        internal bool HasNewMessages(string userID)
        {
            lock (hasMessages)
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
}
