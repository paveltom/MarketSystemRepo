using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Communication_Component
{
    public class Message : Notification
    {
        public string message;
        public string from; //username or storename or Market System 
        public bool isNewMessage;
        public string to;
        public DateTime dateTime;

        public Message(string message, string from_Username)
        {
            this.message = message;
            this.from = from_Username;
            isNewMessage = true;
            dateTime = DateTime.Now;
        }

        public string GetAndReadMessage()
        {
            isNewMessage = false;
            NotificationRepo.GetInstance().ReadMessage(this);
            return dateTime.ToString() + " New message received from: " + from + ".\n" + "The message is: \n" + message;
        }

        public string GetMessageWithoutReading()
        {
            return dateTime.ToString() + " New message received from: " + from + ".\n" + "The message is: \n" + message;
        }

        public bool IsNewMessage()
        {
            return this.isNewMessage;
        }
    }
}