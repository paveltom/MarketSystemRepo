using Market_System.DomainLayer.UserComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class UserPurchaseHistoryObjModel
    {
        [Key]
        public string HisstoryID { get; set; } // username + PurchaseDateTicks
        public string Username;        
        public string PurchaseDateTicks;
        public double TotalPrice;

        public virtual ICollection<BucketModel> Buckets { get; set; }
    }
}