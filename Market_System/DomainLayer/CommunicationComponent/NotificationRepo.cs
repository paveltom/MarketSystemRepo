using Market_System.DAL;
using Market_System.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Communication_Component
{
    public class NotificationRepo
    {
        //private static Dictionary<string, List<Message>> messages; //key = userID

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
                        //messages = new Dictionary<string, List<Message>>();
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
                using (StoreDataContext context = new StoreDataContext())
                {
                    MessageModel model = new MessageModel();
                    model.UpdateWholeModel(message);
                    context.Messages.Add(model);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void ReadMessage(Message message)
        {
            try
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    MessageModel model;
                    if ((model = context.Messages.SingleOrDefault(m => m.NotificationID == message.to + message.from + message.dateTime.Ticks)) != null)
                    {
                        model.IsNewMessage = false;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public List<Message> GetMessages(string userID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                List<Message> ret = context.Messages.Where(m => m.To == userID).ToList().Select(m => ModelToMessage(m)).ToList();
                return ret;
            }
        }


        public Message ModelToMessage(MessageModel model)
        {
            Message msg = new Message(model.Message, model.From);
            msg.to = model.To;
            DateTime.TryParse(model.DateAndTime, out msg.dateTime);
            msg.isNewMessage = model.IsNewMessage;
            return msg;
        }

        public void destroy_me()
        {
            Instance = null;
        }
    }
}