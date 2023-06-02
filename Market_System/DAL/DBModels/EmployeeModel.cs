using System;
using System.Collections.Generic;
using Market_System.DomainLayer.StoreComponent;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Market_System.DomainLayer.StoreComponent;

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
        public string Permissions { get; set; } // enum LIST: perm1_perm2_...
        public bool StoreClosed { get; set; }






        public void UpdateWholeModel(Employee updatedEmp)
        {
            this.UserID = updatedEmp.UserID;
            this.Role = updatedEmp.Role.ToString();
            this.EmployeeID = updatedEmp.UserID + updatedEmp.StoreID + this.Role; // key for TABLE
            this.StoreID = updatedEmp.StoreID;
            this.Permissions = updatedEmp.Permissions.Select(e => e.ToString()).ToList().Aggregate("", (acc, p) => acc += "_" + p, ret => ret.Substring(1));
            this.ManagerAssigner = updatedEmp.ManagerAssigner;
            this.OwnerAssignner = updatedEmp.OwnerAssignner;
            this.StoreClosed = false;
        }


        public Employee ModelToEmployee()
        {
            Role eRole;
            Enum.TryParse<Role>(this.Role, out eRole);
            Employee emp = new Employee(this.UserID, this.StoreID, eRole);
            return emp;
        }
    }
}