using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class CartModel
    {
        [Key]
        public string CartID { get; set; } // username+Cart

        public virtual ICollection<BucketModel> Buckets { get; set; }

    }
}