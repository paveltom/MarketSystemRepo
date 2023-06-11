using Market_System.DomainLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class LotteryModel
    {
        [Key]
        public string LotteryID { get; set; } // productID_userID_lottery
        public int Percantage { get; set; }
        public string UserID { get; set; }
        public string TransactionID { get; set; }
        public virtual ProductModel Product{ get; set; }
    }
}