using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Communication_Component
{
    public class NotificationRepo
    {
        private static Dictionary<string, List<Message>> messages; //key = userID

        private static NotificationRepo Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static NotificationRepo GetInstance()
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
                        messages = new Dictionary<string, List<Message>>();
                        Instance = new NotificationRepo();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

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
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Message> GetMessages(string userID)
        {
            return messages[userID];
        }

        public void destroy_me()
        {
            Instance = null;
        }
    }
}