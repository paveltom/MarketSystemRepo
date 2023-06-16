using Market_System.DomainLayer.UserComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class BucketModel
    {
        [Key]
        public string BucketID { get; set; }
        public string StoreID { get; set; }
        public bool Purchased { get; set; }
        public string Products { get; set; } // <id1>+<quantity1>;<id2>+<quantity2>;...

        public virtual CartModel Cart { get; set; }
        public virtual UserPurchaseHistoryObjModel USerPurchase{ get; set; }



        public Bucket ModelToBucket()
        {
            Bucket buck = new Bucket(this.StoreID);
            buck.SetID(this.BucketID);
            string[] pairs = this.Products.Split(';');
            buck.SetProducts(pairs.Take(pairs.Length - 1).ToDictionary(p => p.Split('+')[0], p => int.Parse(p.Split('+')[1])));
            return buck;
        }
    }
}