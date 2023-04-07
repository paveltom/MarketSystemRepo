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
    //TODO:: need to enforce that only the assginging owner may change the manager's permissions - or fire him. --> not implemented for now. maybe should implemted in other class?

    /**
     * EmployeesPermissions class is responsible Managing the stores managers  permissions.
     */

    public enum Permission { INFO, STOCK, OWNERAPPOINT  };
    public class EmployeesPermissions
    {
        private List<EmployeePermissions> empPermissions; //all possible managing permissions in a store.
        private List<Permission> ownerPermissions;  //these permissions are typcly will given to an store owner.
        //todo feild: founderPermissions 

        public List<EmployeePermissions> EmpPermissions
        {
            get { return this.empPermissions; }
            set { this.empPermissions = value; }
        }

        public List<Permission> OwnerPermissions
        {
            get { return this.ownerPermissions; }
            set { this.ownerPermissions = value; }
        }

        public EmployeesPermissions()
        {
            EmpPermissions = new List<EmployeePermissions>();
            OwnerPermissions = new List<Permission>();
            OwnerPermissions.Add(Permission.INFO);
            OwnerPermissions.Add(Permission.STOCK);
            OwnerPermissions.Add(Permission.OWNERAPPOINT);
        }

        /**add new 'userID' employee with 'permissions' in a store
         */
        public void AddNewEmpPermissions(string userID, string storeID, List<Permission> permissions)
        {
            EmployeePermissions newEmp = new EmployeePermissions(userID, storeID);
            newEmp.Permissions = permissions;
            AddEmp(newEmp);
        }

        /**checks if a given employee of certain store can do actions that require the 'permissionRequiered' permission.
         */
        public Boolean confirmPermission(string userID, string storeID, Permission permissionRequiered)
        {
            if (getemployeePermissions(userID, storeID) == null)
            {
                return false; //employee not exist 
            }
            else
            {
                return getemployeePermissions(userID, storeID).Contains(permissionRequiered);
            }
        }

        public List<Permission> getemployeePermissions(string userID, string storeID)
        {
            EmployeePermissions emp = getemployee(userID, storeID);
            if (emp != null)
            {
                return emp.Permissions;
            }
            else return null;
        }

        /** add the'permission' to an store employee.
         */
        public void AddAnEmpPermission(string userID, string storeID, Permission permissions)
        {
            getemployee(userID, storeID).addPermission(permissions);
        }

        /** add a list of permissions to an store employee.
         */
        public void AddAnEmpPermission(string userID, string storeID, List<Permission> permissions)
        {
            foreach(Permission per in permissions)
            {
                AddAnEmpPermission(userID, storeID, per);
            }
        }

        /**add new  Owner employee  with 'permissions' of an Owner.
        */
        public void AddNewOwnerEmpPermissions(string userID, string storeID)
        {
            EmployeePermissions newEmp = new EmployeePermissions(userID, storeID);
            newEmp.Permissions = OwnerPermissions;
            AddEmp(newEmp);
        }

        private EmployeePermissions getemployee(string userID, string storeID)
        {
            foreach (EmployeePermissions emp in EmpPermissions)
            {
                if (emp.UserID == userID && emp.StoreID == storeID)
                {
                    return emp;
                }
            }
            return null;//not found
        }
        
        private void AddEmp(EmployeePermissions emp)
        {
            EmpPermissions.Add(emp);
        }

        private void RemoveEmp(EmployeePermissions emp)
        {
            EmpPermissions.Remove(emp);
        }

    }
}

     
