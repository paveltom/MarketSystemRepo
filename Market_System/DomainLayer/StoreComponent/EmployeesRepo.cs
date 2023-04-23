using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Market_System.DomainLayer.UserComponent;

namespace Market_System.DomainLayer.StoreComponent

{
    public class EmployeeRepo
    {
        private static List<Employee> employeesDatabase;
        private static List<Employee> closedStoresDatabase;


        private static EmployeeRepo Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static EmployeeRepo GetInstance()
        {
            //This is thread-Safe - Performing a double-lock check.
            if (Instance == null)
            {
                //As long as one thread locks the resource, no other thread can access the resource
                //As long as one thread enters into the Critical Section, 
                //no other threads are allowed to enter the critical section
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        employeesDatabase = new List<Employee>();
                        closedStoresDatabase = new List<Employee>();
                        Instance = new EmployeeRepo();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        public void destroy_me()
        {
            Instance = null;

        }

        public void addNewAdmin(string userID)
        {
            foreach(Employee emp in employeesDatabase)
            {
                if (emp.UserID.Equals(userID))
                {
                    throw new Exception("This user is already an admin!");
                }
            }
            employeesDatabase.Add(new Employee(userID, "", Role.Admin)); //No store specified because it is an admin...
        }

        public void Save_Employee(Employee emp)
        {
            lock (this)
            {
                if (!employeesDatabase.Contains(emp))
                {
                    employeesDatabase.Add(emp);
                }
            }
        }

        public void Remove_Employee(Employee emp)
        {
            lock (this)
            {
                if (!employeesDatabase.Contains(emp))
                {
                    employeesDatabase.Remove(emp);
                }
            }
        }

        internal bool isMarketManager(string userID)
        {
            foreach (Employee emp in employeesDatabase)
            {
                if (emp.UserID.Equals(userID) && emp.Role.Equals(Role.Admin))
                {
                    return true;
                }
            }

            return false;
        }

        internal void Remove_Store(Employee emp)
        {
            lock (this)
            {
                if (!closedStoresDatabase.Contains(emp))
                {
                    closedStoresDatabase.Add(emp);
                }
            }
        }

        internal List<Employee> getClosedStoreEmployees(object storeID)
        {
            List<Employee> emps = new List<Employee>();
            foreach(Employee emp in closedStoresDatabase)
            {
                if (emp.StoreID.Equals(storeID))
                {
                    emps.Add(emp);
                }
            }

            return emps;
        }

        internal void ReopenStore(string store_ID)
        {
            foreach (Employee emp in closedStoresDatabase)
            {
                if (emp.StoreID.Equals(store_ID))
                {
                    closedStoresDatabase.Remove(emp);
                    employeesDatabase.Add(emp);
                }
            }
        }
    }
}
