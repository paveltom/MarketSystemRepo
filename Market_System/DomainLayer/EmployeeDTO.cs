using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.PaymentComponent;
using Market_System.DomainLayer.DeliveryComponent;

namespace Market_System.DomainLayer
{
    public class EmployeeDTO
    {

        public string UserID { get; private set; }
        public string StoreID { get; private set; }
        public string Role { get; private set; }
        public string OwnerAssignner { get; private set; }
        public string ManagerAssigner { get; private set; }
        public List<string> Permissions { get; private set; }

        public EmployeeDTO(string employeeID)
        {
            this.UserID = employeeID;
            this.StoreID = "";
            this.Role = employeeID;
            this.OwnerAssignner = "";
            this.ManagerAssigner = "";
            this.Permissions = new List<string>();

        }

        public EmployeeDTO(Employee employee)
        {
            this.UserID = employee.UserID;
            this.StoreID = employee.StoreID;
            this.Role = employee.Role.ToString();
            this.OwnerAssignner = employee.OwnerAssignner;
            this.ManagerAssigner = employee.ManagerAssigner;
            this.Permissions = employee.Permissions.Select(x => x.ToString()).ToList();
        }

        public void SetRole(string role)
        {
            this.Role = role;
        }

        public void SetStoreID(string storeID)
        {
            this.StoreID = storeID;
        }

        public void SetOwnerAssignner(string assigner)
        {
            this.OwnerAssignner = assigner;
        }

        public void SetManagerAssignerD(string assigner)
        {
            this.ManagerAssigner = assigner;
        }

        public void SetPermissions(List<string> permissions)
        {
            this.Permissions = permissions;
        }

        public void SetPermissions(List<Market_System.DomainLayer.StoreComponent.Permission> permissions)
        {
            this.Permissions = permissions.Select(x => x.ToString()).ToList();
        }


    }
}
