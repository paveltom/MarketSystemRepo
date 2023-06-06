﻿using Market_System.DAL.DBModels;
using Market_System.DomainLayer.StoreComponent;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Market_System.DAL
{
    public class StoreDataContext : DbContext
    {
        public virtual DbSet<StoreModel> Stores { get; set; }
        public virtual DbSet<ProductModel> Products{ get; set; }
        public virtual DbSet<EmployeeModel> Employees { get; set; }
        public virtual DbSet<StorePurchaseHistoryObjModel> Purchases { get; set; }
        public virtual DbSet<PurchasePolicyModel> Policies{ get; set; }
        public virtual DbSet<PurchaseStrategyModel> Strategies{ get; set; }
        public virtual DbSet<BidModel> Bids { get; set; }



        //public StoreDataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename=C:\\MarketDB.dbo;Initial Catalog=MarketDB;Integrated Security=True")

        //public StoreDataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")

        // 192.168.1.65/24


        // Data Source=192.168.1.65/24, 1433;Initial Catalog=MarketDB; User Id=sa; Password=sadnaSQL123;Integrated Security=True";
        public StoreDataContext() : base("Data Source=192.168.1.65, 1433; Initial Catalog=MarketDB; User Id=sa; Password=sadnaSQL123;Integrated Security=False")

        {

        }
        
    }
}