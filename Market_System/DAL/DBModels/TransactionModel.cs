using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class TransactionModel
    {
        [Key]
        public string TransactioID { get; set; }
        public double Price { get; set; }
        public string UserID { get; set; }
        public bool Cancelled { get; set; }
    }
}