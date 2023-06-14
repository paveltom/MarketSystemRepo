using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class UserModel
    {
        [Key]
        public string UserID { get; set; }
    }
}