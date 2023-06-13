using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Market_System.DomainLayer.UserComponent;
using Market_System.DAL;
using Market_System.DAL.DBModels;

namespace Market_System.DomainLayer.StoreComponent

{
    public class EmployeeRepo
    {
        private static EmployeeRepo Instance = null;

        private static readonly object Instancelock = new object();

        public static EmployeeRepo GetInstance()
        {
            if (Instance == null)
            {
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        Instance = new EmployeeRepo();
                    }
                } //Critical Section End
            }
            return Instance;
        }

        public void destroy_me()
        {
            Instance = null;

        }

        public void addNewAdmin(string userID)
        {
            string adminRole = Role.Admin.ToString();
            using (StoreDataContext context = new StoreDataContext())
            {
                if(context.Employees.Any(e => e.UserID == userID && e.Role == adminRole))
                    throw new Exception("This user is already an admin!");
                EmployeeModel emp = new EmployeeModel();
                emp.UserID = userID;
                emp.Role = Role.Admin.ToString();
                emp.EmployeeID = userID + "_ADMIN"; // key for TABLE
                emp.StoreID = "";     
                emp.StoreClosed = true;
                context.Employees.Add(emp);
                context.SaveChanges();
            }
        }

        public void Save_Employee(Employee emp)
        {
            lock (this)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    string eRole = emp.Role.ToString();
                    EmployeeModel toSave;
                    if ((toSave = context.Employees.SingleOrDefault(e => e.UserID == e.UserID && e.StoreID == emp.StoreID && e.Role == eRole)) == null)
                    {
                        toSave = new EmployeeModel();
                        toSave.UpdateWholeModel(emp);
                        context.Employees.Add(toSave);
                    }
                    else
                        toSave.UpdateWholeModel(emp);

                    // StoreModel sm = context.Stores.SingleOrDefault(s => s.StoreID == emp.StoreID);
                    context.SaveChanges();
                }
            }
        }


        public void AddEmployee(Employee emp)
        {
            lock (this)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    string eRole = emp.Role.ToString();
                    EmployeeModel toSave;
                    if ((toSave = context.Employees.SingleOrDefault(e => e.UserID == e.UserID && e.StoreID == emp.StoreID && e.Role == eRole)) == null)
                    {
                        toSave = new EmployeeModel();
                        toSave.UpdateWholeModel(emp);
                        context.Employees.Add(toSave);
                    }
                    else
                        throw new Exception("Employee already works in this store.");

                    // StoreModel sm = context.Stores.SingleOrDefault(s => s.StoreID == emp.StoreID);
                    context.SaveChanges();
                }
            }
        }
        

        public void Remove_Employee(Employee emp)
        {
            lock (this)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    string eRole = emp.Role.ToString();
                    EmployeeModel remove;
                    if ((remove = context.Employees.SingleOrDefault(e => e.UserID == e.UserID && e.StoreID == emp.StoreID && e.Role == eRole)) != null)
                    {
                        context.Employees.Remove(remove);
                        context.SaveChanges();
                    }
                }
            }
        }

        internal bool isMarketManager(string userID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                string eRole = Role.Admin.ToString();
                if (context.Employees.SingleOrDefault(e => e.UserID == userID && e.Role == eRole) != null)
                    return true;
            }
            return false;
        }

        internal void Remove_Store(Employee emp)
        {
            lock (this)
            {
                using (StoreDataContext context = new StoreDataContext())
                {
                    string eRole = emp.Role.ToString();
                    EmployeeModel model;
                    if((model = context.Employees.SingleOrDefault(e => e.UserID == e.UserID && e.StoreID == emp.StoreID && e.Role == eRole)) != null)
                    {
                        model.StoreClosed = true;
                        context.SaveChanges();
                    }
                    
                }

            }
        }

        internal List<Employee> getClosedStoreEmployees(string storeID)
        {
            List<Employee> emps = new List<Employee>();
            using (StoreDataContext context = new StoreDataContext())
            {
                emps = context.Employees.Select(e => e).ToList().Where(e => e.StoreID == storeID).Select(e => e.ModelToEmployee()).ToList();
            }
            return emps;
        }

        internal void ReopenStore(string store_ID)
        {
            using (StoreDataContext context = new StoreDataContext())
            {
                context.Employees.Select(e => e).ToList().Where(e => e.StoreID == store_ID).ForEach(e => e.StoreClosed = false);                
                context.SaveChanges();               
            }
        }
    }
}
