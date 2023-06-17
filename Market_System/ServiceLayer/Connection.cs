using Market_System.DAL;
using Market_System.Domain_Layer.Communication_Component;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.ServiceLayer
{
    public class Connection
    {
        private static Connection Instance = null;
        public string ConnectionString { get; private set; }

        public static string BasicConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True";
        public static string RemoteLinuxConnectionString = "Data Source=192.168.1.65, 1433; Initial Catalog=MarketDB; User Id=sa; Password=sadnaSQL123;Integrated Security=False";


        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        public static Connection GetInstance()
        {
            if (Instance == null)
            {
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        Instance = new Connection();
                    }
                } //Critical Section End

            }

            return Instance;
        }


        public void SetConnectionString(string connString)
        {
            this.ConnectionString = connString;
        }
    }
}