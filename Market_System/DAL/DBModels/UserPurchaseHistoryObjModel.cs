using Market_System.DomainLayer.UserComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Ajax.Utilities;




namespace Market_System.DAL.DBModels
{
    public class UserPurchaseHistoryObjModel
    {
        [Key]
        public string HisstoryID { get; set; } // username + PurchaseDateTicks
        public string Username { get; set; }
        public string PurchaseDateTicks { get; set; }
        public double TotalPrice { get; set; }

        public virtual ICollection<BucketModel> Buckets { get; set; }


        public PurchaseHistoryObj ModelToHistory()
        {
            PurchaseHistoryObj history = new PurchaseHistoryObj(this.Username, this.Buckets.ToList().Select(b => b.ModelToBucket()).ToList(), this.TotalPrice);
            history.SetPurchaseDates(new DateTime(long.Parse(this.PurchaseDateTicks)));
            return history;
        }
    }
}