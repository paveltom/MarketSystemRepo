using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer
{
    public class BidDTO
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
        public string PayDetails { get; set; }


        public BidDTO(string userID, string productID, double newPrice, int quantity, string payDetails)
        {          
            this.UserID = userID;
            this.ProductID = productID;
            this.NewPrice = newPrice;
            this.Quantity = quantity;
            this.BidID = userID + "_" + productID + "_bid";
            this.ApprovedByStore = false;
            this.ApprovedByUser = false;
            this.CounterOffer = false;
            this.DeclinedByStore = false;
            this.DeclinedByUser = false;
            this.NumOfApproves = 0;
            this.PayDetails = payDetails;
        }
    }
}