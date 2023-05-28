using System;
using System.Collections.Generic;
using Market_System.DomainLayer.StoreComponent;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Market_System.DAL.DBModels
{
    public class EmployeeModel
    {
        [Key]
        public string EmployeeID { get; set; } // userID _storeID_role
        public string UserID { get; set; }
        public string StoreID { get; set; }//can change into Store
        public string Role { get; set; } // enum
        public string OwnerAssignner { get; set; }
        public string ManagerAssigner { get; set; }
        public string Permissions { get; set; } // enum LIST!!!
    }
}