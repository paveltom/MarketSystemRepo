using Market_System.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class BidModel
    {
        public string BidID { get; set; }
        public double NewPrice { get; set; }
        public bool ApprovedByStore { get; set; }
        public bool ApprovedByUser { get; set; }
        public bool CounterOffer { get; set; }
        public bool DeclinedByStore { get; set; }
        public bool DeclinedByUser { get; set; }
        public int NumOfApproves { get; set; }
        public string ProductID { get; set; }
        public string UserID { get; set; }
        public int Quantity { get; set; }
        public virtual StoreModel Store { get; set; }





        public BidDTO ModelToBid()
        {
            BidDTO ret = new BidDTO(this.UserID, this.ProductID, this.NewPrice, this.Quantity);
            ret.BidID = this.BidID;
            ret.ApprovedByUser = this.ApprovedByUser;
            ret.ApprovedByStore = this.ApprovedByStore;
            ret.DeclinedByStore = this.DeclinedByStore;
            ret.DeclinedByUser = this.DeclinedByUser;
            ret.CounterOffer = this.CounterOffer;
            ret.NumOfApproves = this.NumOfApproves;
            return ret;
        }
    }
}