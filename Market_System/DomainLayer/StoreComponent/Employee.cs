using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace Market_System.DomainLayer.StoreComponent
{
    //TODO:: Add an Enum class here called: <Permission> - all of the available permissions that can be assigned to managers (by store owners).
    //TODO:: need to enforce that only the assginging owner may change the manager's permissions - or fire him. 

    public enum Role { Admin, Founder, Owner, Manager };

    public class Employee
    {
        string userID;
        string storeID;//can change into Store
        Role role;
        string ownerAssignner;
        string managerAssigner;
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

        public Role Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        public Boolean isFounder()
        {
            return Role.Equals(Role.Founder);
        }

        public string OwnerAssignner
        {
            get { return this.ownerAssignner; }
            set { this.ownerAssignner = value; }
        }

        public string ManagerAssigner
        {
            get { return this.managerAssigner; }
            set { this.managerAssigner = value; }
        }

        public Employee(string userID, string storeID, Role role)
        {
            UserID = userID;
            StoreID = storeID;
            Permissions = new List<Permission>();
            Role = role;
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

        public Boolean isMyOwnerAssignner(string OwnerID)
        {
            return ManagerAssigner.Equals(OwnerAssignner);
        }

        public Boolean isMyManagerAssignner(string managerID)
        {
            return ManagerAssigner.Equals(managerID);
        }
        
        public Boolean isOwner()
        {
            return Role.Equals(Role.Owner);
        }

        public Boolean isManager()
        {
            return Role.Equals(Role.Manager);
        }


    }
}

     
