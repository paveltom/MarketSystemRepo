using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class BucketModel
    {
        [Key]
        public string BucketID { get; set; }
        public bool Purchased { get; set; }
    }
}