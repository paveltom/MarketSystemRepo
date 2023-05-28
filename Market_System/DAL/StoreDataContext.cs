using Market_System.DAL.DBModels;
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


        public StoreDataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")
        {

        }
        
    }
}