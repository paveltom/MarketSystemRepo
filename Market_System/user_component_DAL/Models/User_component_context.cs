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
        public DbSet<Bucket_model> bucker_models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True");

        }
        
        public User_component_context() : base(new DbContextOptionsBuilder<User_component_context>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")
                .Options)
        {


        }

        public bool IsDatabaseExists()
        {
            return Database.CanConnect();
        }

    }
    }