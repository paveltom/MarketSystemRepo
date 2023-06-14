using Market_System.DAL.DBModels;
using Market_System.Presentaion_Layer;
using System.Data.Entity;


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
        public virtual DbSet<TimerModel> TimersDB { get; set; }
        public virtual DbSet<TransactionModel> Transactions { get; set; }

        public virtual DbSet<CommentModel> Comments { get; set; } 
        public virtual DbSet<ProductAttributeModel> ProductPurchaseAttributes { get; set; }
        public virtual DbSet<LotteryModel> Lottery { get; set; }


        // Notification
        public virtual DbSet<MessageModel> Messages { get; set; }



        // User
        public virtual DbSet<UserModel> Users { get; set; }







        //public StoreDataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename=C:\\MarketDB.dbo;Initial Catalog=MarketDB;Integrated Security=True")   ==== localDB connection string with file in C:\ directory - dont have permissions

        //public StoreDataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")  ==== connection string to localDB

        // 192.168.1.65/24 ==== linux sql server ip       

        // public StoreDataContext() : base("Data Source=192.168.1.65, 1433; Initial Catalog=MarketDB; User Id=sa; Password=sadnaSQL123;Integrated Security=False")   ======= connection strng to linux sql server


        public StoreDataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True")

        {

        }
        
    }
}