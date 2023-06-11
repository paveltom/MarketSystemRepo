using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class CommentModel
    {
        [Key]
        public string CommentID { get; set; } //  = productID + userID
        public string Comment { get; set; }
        public virtual ProductModel Product { get; set; }

    }
}