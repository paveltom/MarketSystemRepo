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
        public DbSet<Product_in_basket_model> products_in_baskets_models { get; set; }
        public DbSet<only_for_checking_if_first_time_running> first_time_flag { get; set; }
        public DbSet<Bucket_model_history> bucket_history_models { get; set; }
        public DbSet<Product_in_basket_history_model> Product_in_basket_history_models { get; set; }
        public DbSet<purchase_history_model> purchase_history_models { get; set; }




        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True");

        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder) // this is used to define multiple keys for Product_in_basket_model
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product_in_basket_model>()
                .HasKey(p => new { p.product_id, p.basket_id });
        }

        public User_component_context() : base(new DbContextOptionsBuilder<User_component_context>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")
                .Options)
        {


        }
        
        public User_component_context(DbContextOptions options) : base(options)
        {
        }

        public bool IsDatabaseExists()
        {
            
            return Database.CanConnect();
        }

        internal List<purchase_history_model> get_purhcase_histories_by_username(string username)
        {
            var histories = this.purchase_history_models.Where(e => e.username == username).ToList();
            
            return histories;
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

        internal Cart_model Getcart_model_by_id(int cart_id)
        {
            var cart = this.cart_models.FirstOrDefault(u => u.ID == cart_id);
            return cart;
        }

        internal Product_in_basket_model get_Product_in_basket_model_by_product_id_and_basket_id(string basket_id, string product_id)
        {
            var pibm = this.products_in_baskets_models.FirstOrDefault(u => u.basket_id == basket_id && u.product_id== product_id);
            return pibm;
        }

        internal Bucket_model get_basket_model_by_basket_id(string basket_id)
        {
            var basket = this.bucket_models.FirstOrDefault(u => u.basket_id == basket_id );
            return basket;
        }
    }
    }