using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace Market_System.Domain_Layer.Store_Component
{
    //TODO:: Add an Enum class here called: <Permission> - all of the available permissions that can be assigned to managers (by store owners).
    //TODO:: need to enforce that only the assginging owner may change the manager's permissions - or fire him. 


    public class EmployeePermissions
    {
        string userID;
        string storeID;//can change into Store
        List<Permission> permissions;

        public string UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public string StoreID
        {
            get { return this.storeID; }
            set { this.storeID = value; }
        }

        public List<Permission> Permissions
        {
            get { return this.permissions; }
            set { this.permissions = value; }
        }

        /*
         *giving Guest permission as default 
         */
        public EmployeePermissions(string userID, string storeID)
        {
            UserID = userID;
            StoreID = storeID;
            Permissions = new List<Permission>();
        }

        /**
         * checks if a certain permission is exist
         */
        public Boolean confirmPermission(Permission permission)
        {
            return Permissions.Contains(permission);
        }

        public void addPermission(Permission permission)
        {
            Permissions.Add(permission);
        }

        public void removePermission(Permission permission)
        {
            Permissions.Remove(permission);
        }


    }
}

     
