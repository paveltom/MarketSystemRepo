using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    public class User_DAL_controller
    {

        private static User_DAL_controller instance;
        private static User_component_context ucc;

        private User_DAL_controller()
        {

        }
        public static User_DAL_controller GetInstance()
        {
            if(instance==null)
            {
                instance = new User_DAL_controller();
                ucc = new User_component_context(new DbContextOptionsBuilder<User_component_context>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")
                .Options);

            }
            return instance;
        }

        public void change_to_test_database()
        {
            ucc= new User_component_context(new DbContextOptionsBuilder<User_component_context>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB_test;Integrated Security=True")
                .Options);
            try { 
                ucc.Database.Migrate();
                ucc.Database.EnsureCreated();
            }
            catch(Exception e)
            {
                //shove it up your ass
            }
        }

        public void change_to_regular_database()
        {
            ucc = new User_component_context(new DbContextOptionsBuilder<User_component_context>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")
                .Options);
        }

        public User_component_context get_context()
        {
            return ucc;
        }
    }
}