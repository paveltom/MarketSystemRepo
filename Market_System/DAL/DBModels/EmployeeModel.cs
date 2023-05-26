using System;
using System.Collections.Generic;
using Market_System.DomainLayer.StoreComponent;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class EmployeeModel
    {        
        public string userID;
        public string storeID;//can change into Store
        public string role; // enum
        public string ownerAssignner;
        public string managerAssigner;
        public string permissions; // enum LIST!!!
    }
}