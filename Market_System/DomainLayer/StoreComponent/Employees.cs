﻿using Microsoft.Ajax.Utilities;
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
    //TODO:: need to enforce that only the assginging owner may change the manager's permissions - or fire him. --> not implemented for now. maybe should implemted in other class?

    /**
     * EmployeesPermissions class is responsible Managing the stores managers  permissions.
     */

    public enum Permission { INFO, STOCK, Policy, OwnerOnly, FounderOnly, Admin };

    /**
     *  Permission:
     *  מתוך מסמך דרישות כללי- II משתמשים
     *  INFO - 4.12,4.13 קבלת מידע ומתן תגובה, קבלת מידע על היסטוריית רכישות בחנות
     *  STOCK - 4.1 ניהול מלאי
     *  Policy - 4.2 שינוי סוגי וכללי (מדיניות) קניה והנחה של חנות
     *  OwnerOnly - 4.4, 4.5, 4.6, 4.7, 4.8 4.11 מינוי והסרת בעל חנות/מנהל חנות ושינויי הרשאות מנהל, בקשה למידע על תפקידים בחנות  
     *  FounderOnly - 4.9, 4.10, 4.3 קביעת אילוצי עקיבות עבור חנות, סגירת/פתיחת חנות
     *  Admin - 6.1, 6.2, 6.3, 6.4, 6.5 הרשאות מנהל מערכת=מנהל שוק
     */
    public class Employees
    {
        private List<Employee> empPermissions; 
        private List<Permission> ownerPermissions;  //these permissions are given to store owner.
        private List<Permission> founderPermissions;  //these permissions are given to store founder.

        //move database actions(and fields) to the repo class
        public List<Employee> EmpPermissions
        {
            get { return this.empPermissions; }
            set { this.empPermissions = value; }
        }

        public List<Permission> OwnerPermissions
        {
            get { return this.ownerPermissions; }
            set { this.ownerPermissions = value; }
        }
        public List<Permission> FounderPermissions
        {
            get { return this.ownerPermissions; }
            set { this.ownerPermissions = value; }
        }
  

        public Employees()
        {
            EmpPermissions = new List<Employee>();
            OwnerPermissions = new List<Permission>();
            FounderPermissions = new List<Permission>();
            OwnerPermissions.Add(Permission.INFO); OwnerPermissions.Add(Permission.STOCK);
            OwnerPermissions.Add(Permission.Policy); OwnerPermissions.Add(Permission.OwnerOnly);
            foreach (Permission permissions in OwnerPermissions)
            {
                FounderPermissions.Add(permissions);
            }
            FounderPermissions.Add(Permission.FounderOnly);
        }

        public List<Employee> getStoreEmployees(string storeID)
        {
            List<Employee> emps = new List<Employee>();
            foreach (Employee emp in empPermissions)
            {
                if (emp.StoreID == storeID)
                {
                    emps.Add(emp);
                }
            }
            return emps;
        }

        public List<string> GetOwnersOfTheStore(string storeID)
        {
            List<string> ownersIDs = new List<string>();
            List<Employee> employees = getStoreEmployees(storeID);
            foreach (Employee emp in employees)
            {
                if (emp.isOwner())
                {
                    ownersIDs.Add(emp.UserID);
                }
            }
            return ownersIDs;
        }

        public List<string> GetManagersOfTheStore(string storeID)
        {
            List<string> managersIDs = new List<string>();
            List<Employee> employees = getStoreEmployees(storeID);
            foreach (Employee emp in employees)
            {
                if (emp.isManager())
                {
                    managersIDs.Add(emp.UserID);
                }
            }
            return managersIDs;
        }

        public Boolean isOwnerSubject(string subjectUserID, string sellerID, string storeID)
        {
            return getemployee(subjectUserID, storeID).isMyOwnerAssignner(storeID);
        }

        public Boolean isManagerSubject(string subjectUserID, string sellerID, string storeID)
        {
            return getemployee(subjectUserID, storeID).isMyManagerAssignner(sellerID);
        }

        public Boolean isOwner(string employeeID, string storeID)
        {
            return getemployee(employeeID, storeID).isOwner();
        }

        public Boolean isManager(string employeeID, string storeID)
        {
            return getemployee(employeeID, storeID).isManager();
        }

        /**add new 'userID' employee with 'permissions' in a store
         */
        public void AddNewEmpPermissions(string userID, string storeID, List<Permission> permissions, Role role)
        {
            Employee newEmp = new Employee(userID, storeID, role);
            newEmp.Permissions = permissions;
            AddEmp(newEmp);
        }

        /**remove 'userID' employee
        */
        public void removeEmployee(string userID, string storeID)
        { 
            removeEmp(getemployee(userID, storeID));
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
            Employee emp = getemployee(userID, storeID);
            if (emp != null)
            {
                return emp.Permissions;
            }
            else return null;
        }

        /** add the'permission' to a store manager.
         */
        public void AddAnEmpPermission(string userID, string storeID, Permission permission)
        {
            getemployee(userID, storeID).addPermission(permission);
        }

        /** remove the'permission' from a store employee.
       */
        public void removeAnEmpPermission(string userID, string storeID, Permission permission)
        {
            getemployee(userID, storeID).removePermission(permission);
        }

        /** add a list of permissions to a store employee.
         */
        public void AddAnEmpPermission(string userID, string storeID, List<Permission> permissions)
        {
            foreach(Permission per in permissions)
            {
                AddAnEmpPermission(userID, storeID, per);
            }
        }

        public void updateEmpPermissions(string userID, string storeID, List<Permission> permissions)
        {
            this.getemployee(userID,storeID).Permissions = permissions;
        }

        /** remove a list of permissions from a store employee.
      */
        public void removeAnEmpPermission(string userID, string storeID, List<Permission> permissions)
        {
            foreach (Permission per in permissions)
            {
                removeAnEmpPermission(userID, storeID, per);
            }
        }

        /**add new  Owner employee  with 'permissions' of an Owner.
        */
        public void AddNewOwnerEmpPermissions(string assignnerID, string newOwnerID, string storeID)
        {
            Employee newEmp = new Employee(newOwnerID, storeID, Role.Owner);
            newEmp.OwnerAssignner = assignnerID;
            newEmp.Permissions = OwnerPermissions;
            AddEmp(newEmp);
        }

        /**add new  founder employee  with 'permissions' of an founder.
  */
        public void AddNewFounderEmpPermissions(string userID, string storeID)
        {
            Employee newEmp = new Employee(userID, storeID, Role.Founder);
            newEmp.Permissions = FounderPermissions;
            AddEmp(newEmp);
        }

        public void AddNewManagerEmpPermissions(string assignnerID, string newManagerID, string storeID, List<Permission> managingPermissions)
        {
            Employee newEmp = new Employee(newManagerID, storeID, Role.Manager);
            newEmp.ManagerAssigner = assignnerID;
            AddNewEmpPermissions(newManagerID, storeID, managingPermissions, Role.Manager);
        }

        public void removeStore(string storeID)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!111
            //save a backup
            List<Employee> emps = getStoreEmployees(storeID);
            foreach (Employee emp in emps)
            {
                this.removeEmployee(emp.UserID, storeID);
            }
        }

        private Employee getemployee(string userID, string storeID)
        {
            foreach (Employee emp in EmpPermissions)
            {
                if (emp.UserID == userID && emp.StoreID == storeID)
                {
                    return emp;
                }
            }
            return null;//not found
        }
        
        private void AddEmp(Employee emp)
        {
            EmpPermissions.Add(emp);
        }

        private void removeEmp(Employee emp)
        {
            EmpPermissions.Remove(emp);
        }

        //**add exeption catching.
    }
}

     