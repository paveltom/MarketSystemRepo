using Market_System.Domain_Layer.Communication_Component;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class MessageModel
    {
        [Key]
        public string NotificationID { get; set; } // msg.to + msg.from + msg.dateTime.Ticks;
        public string Message { get; set; }
        public string From { get; set; } //username or storename or Market System 
        public string To { get; set; } // userID
        public bool IsNewMessage { get; set; }
        public string DateAndTime { get; set; } //DateTime

        public virtual UserModel user { get; set; }



        public void UpdateWholeModel(Message msg)
        {
            this.NotificationID = msg.to + msg.from + msg.dateTime.ToString();
            this.Message = msg.message;
            this.From = msg.from; 
            this.To = msg.to;
            this.IsNewMessage = msg.isNewMessage;
            this.DateAndTime = msg.dateTime.ToString();
        }

    }
}