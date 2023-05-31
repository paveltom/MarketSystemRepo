using Market_System.DomainLayer.UserComponent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    public class User_component_context : DbContext
    {
        public DbSet<user_model> user_models { get; set; }
        public DbSet<Cart_model> cart_models { get; set; }
        public DbSet<Bucket_model> bucket_models { get; set; }
        public DbSet<only_for_checking_if_first_time_running> first_time_flag { get; set; }

        private static User_component_context instance;
        private static string regularConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True";
        private static string testConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB_Test;Integrated Security=True";

        private User_component_context(DbContextOptions<User_component_context> options) : base(options)
        {
        }

        public static User_component_context GetInstance()
        {
            if (instance == null)
            {
                var options = new DbContextOptionsBuilder<User_component_context>()
                    .UseSqlServer(regularConnectionString)
                    .Options;

                instance = new User_component_context(options);
                instance.Database.Migrate();
            }

            return instance;
        }

        public void ChangeToTestDatabase()
        {
            
            
                var options = new DbContextOptionsBuilder<User_component_context>()
                    .UseSqlServer(testConnectionString)
                    .Options;

                instance = new User_component_context(options);
            instance.Database.Migrate();
        }

        public void RestoreToRegularDatabase()
        {
          
                var options = new DbContextOptionsBuilder<User_component_context>()
                    .UseSqlServer(regularConnectionString)
                    .Options;

                instance = new User_component_context(options);
            
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True");

        }
        */
        /*
        public User_component_context() : base(new DbContextOptionsBuilder<User_component_context>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")
                .Options)
        {


        }
        */
        private User_component_context(DbContextOptions options) : base(options)
        {
        }

        public bool IsDatabaseExists()
        {
            
            return Database.CanConnect();
        }

        public void ResetDatabase()
        {
            Database.EnsureDeleted(); // Drop the existing database
            Database.EnsureCreated(); // Recreate the database structure
            
        }

        public user_model GetUserByName(string name)
        {
            
                var user = this.user_models.FirstOrDefault(u => u.username == name);
                return user;
            
        }

        internal bool first_time_running_project_market()
        {
            return this.first_time_flag.Count() <1;
        }

        internal bool first_time_running_project()
        {
            return this.first_time_flag.Count() < 2;
        }

        internal void set_first_time_running()
        {
            only_for_checking_if_first_time_running addme = new only_for_checking_if_first_time_running();
            addme.firsttimerunning = 1;
            this.first_time_flag.Add(new only_for_checking_if_first_time_running());
            SaveChanges();
            
        }

        internal void set_second_time_running()
        {
            only_for_checking_if_first_time_running addme = new only_for_checking_if_first_time_running();
            addme.firsttimerunning = 2;
            this.first_time_flag.Add(new only_for_checking_if_first_time_running());
            SaveChanges();

        }

        internal user_model GetUserByUserID(string userID)
        {
            var user = this.user_models.FirstOrDefault(u => u.user_ID == userID);
            return user;
        }
    }
    }