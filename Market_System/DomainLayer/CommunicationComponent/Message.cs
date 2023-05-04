using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Communication_Component
{
    public class Message : Notification
    {
        private string message;
        private string from; //username or storename or Market System 
        private bool isNewMessage;

        public Message(string message, string from_Username)
        {
            this.message = message;
            this.from = from_Username;
            isNewMessage = true;
        }

        public string GetAndReadMessage()
        {
            isNewMessage = false;
            return "New message received from: " + from + ".\n" + "The message is: \n" + message;
        }

        public string GetMessageWithoutReading()
        {
            return "New message received from: " + from + ".\n" + "The message is: \n" + message;
        }

        public bool IsNewMessage()
        {
            return this.isNewMessage;
        }
    }
}