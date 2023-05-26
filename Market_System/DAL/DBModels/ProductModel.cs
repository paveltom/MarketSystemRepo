using Market_System.DomainLayer.StoreComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class ProductModel
    {
        [Key]
        public string ProductID { get; set; }
        public string StoreID { get; set; }
        public string Name { get; set; }

        public virtual StoreModel Store { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<PurchaseStrategyModel> Strategies { get; set; }
        public virtual ICollection<PurchasePolicyModel> Policies { get; set; }
        public virtual ICollection<StorePurchaseHistoryObjModel> Purchases { get; set; }





    }
}