using Market_System.DomainLayer.StoreComponent;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Ajax.Utilities;


namespace Market_System.DAL.DBModels
{
    public class ProductModel
    {
        [Key]
        public string ProductID { get; set; }
        public string StoreID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Auction { get; set; } // userID_price_transactionID
        public int ReservedQuantity { get; set; }
        public double Rating { get; set; } // between 1-10
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Sale { get; set; } // 1-100 percentage  - temporary variant before PurchasePolicy implmnt
        public long timesBought { get;  set; }
        public long timesRated { get;  set; }
        public string ProductCategory { get;  set; }   // (mabye will be implementing by composition design pattern to support a sub catagoring.)
        public string Dimenssions { get;  set; } // string format: x_y_z

        public virtual ICollection<CommentModel> Comments { get;  set; } // comments format: 
        public virtual ICollection<ProductAttributeModel> ProductPurchaseAttributes { get; set; }
        public virtual StoreModel Store { get; set; }
        public virtual ICollection<PurchaseStrategyModel> Strategies { get; set; }
        public virtual ICollection<PurchasePolicyModel> Policies { get; set; }
        public virtual ICollection<LotteryModel> Lottery { get; set; }


        public void UpdateWholeModel(Product updatedProduct) // called on Save()
        {
            this.Name = updatedProduct.Name;
            this.Description = updatedProduct.Description;
            this.Price = updatedProduct.Price;
            this.Auction = updatedProduct.Auction.Key + "_" + updatedProduct.Auction.Value[0] + "_" + updatedProduct.Auction.Value[1];
            this.ReservedQuantity = updatedProduct.ReservedQuantity;
            this.Rating = updatedProduct.Rating; // between 1-10
            this.Quantity = updatedProduct.Quantity;
            this.Weight = updatedProduct.Weight;
            this.Sale = updatedProduct.Sale;// 1-100 percentage  - temporary variant before PurchasePolicy implmnt
            this.timesBought = updatedProduct.timesBought;
            this.timesRated = updatedProduct.timesRated;
            this.ProductCategory = updatedProduct.ProductCategory.CategoryName;    // (mabye will be implementing by composition design pattern to support a sub catagoring.)
            this.Dimenssions = updatedProduct.Dimenssions.Aggregate("", (acc, d) => (acc += "_" + d), str => str.Substring(1));// string format: x_y_z

            // collection init
            this.ProductPurchaseAttributes = new HashSet<ProductAttributeModel>();
            this.Comments = new HashSet<CommentModel>();
            this.Strategies = new HashSet<PurchaseStrategyModel>();
            this.Policies = new HashSet<PurchasePolicyModel>();
            this.Lottery= new HashSet<LotteryModel>();


        updatedProduct.Lottery.ForEach(l => {
            if (this.Lottery.Any(lot => lot.LotteryID == updatedProduct.Product_ID + "_" + l.Key + "_lottery"))
                    return;
                LotteryModel model = new LotteryModel();
                model.LotteryID = updatedProduct.Product_ID + "_" + l.Key + "_lottery";
                model.UserID = l.Key;
                model.Percantage = int.Parse(l.Value[0]);
                model.TransactionID = l.Value[1];
                this.Lottery.Add(model);
            });

            updatedProduct.Comments.ForEach(c =>
            {
                string commentID = this.ProductID + c.Split(':')[0];
                if (this.Comments.Select(x => x.CommentID == commentID).Count() == 0)
                {
                    CommentModel cm = new CommentModel();
                    cm.Product = this;
                    cm.Comment = c;
                    cm.CommentID = commentID;
                    this.Comments.Add(cm); 
                }
            });

            List<CommentModel> remove = new List<CommentModel>();
            this.Comments.ForEach(c =>
            {
                               
                if (!updatedProduct.Comments.Any(x => this.ProductID + x.Split(':')[0] == c.CommentID))
                {
                    remove.Add(c);                   
                }
            });
            remove.ForEach(c => this.Comments.Remove(c));

            updatedProduct.PurchaseAttributes.ForEach(p =>
            {
                string attrID = this.ProductID + p.Key;
                if (this.ProductPurchaseAttributes.Select(x => x.AttributeID == attrID).Count() == 0)
                {
                    ProductAttributeModel pm = new ProductAttributeModel();
                    pm.Product = this;
                    pm.AttributeID = attrID;
                    pm.AttributeName = p.Key;
                    pm.AttributeOptions = p.Value.Aggregate("", (acc, s) => acc + "_" + s, str => str.Substring(1));
                    this.ProductPurchaseAttributes.Add(pm);
                }
            });

            List<ProductAttributeModel> removeAttr = new List<ProductAttributeModel>();
            this.ProductPurchaseAttributes.ForEach(c =>
            {

                if (!updatedProduct.PurchaseAttributes.Any(x => this.ProductID + x.Key == c.AttributeID))
                {
                    removeAttr.Add(c);
                }
            });
            removeAttr.ForEach(c => this.ProductPurchaseAttributes.Remove(c));



            updatedProduct.PurchaseStrategies.ForEach(p =>
            {
                if (this.Strategies.Select(x => x.StrategyID == p.Key).Count() == 0)
                {
                    PurchaseStrategyModel pm = new PurchaseStrategyModel();
                    pm.DefineNewMe(p.Value, false, null, this);
                    this.Strategies.Add(pm);
                }
            });

            List<PurchaseStrategyModel> removeStrat = new List<PurchaseStrategyModel>();
            this.Strategies.ForEach(c =>
            {

                if (!updatedProduct.PurchaseStrategies.Any(x => x.Key == c.StrategyID))
                {
                    removeStrat.Add(c);
                }
            });
            removeStrat.ForEach(c => this.Strategies.Remove(c));


            updatedProduct.PurchasePolicies.ForEach(p =>
            {
                if (this.Policies.Select(x => x.PolicyID == p.Key).Count() == 0)
                {
                    PurchasePolicyModel pm = new PurchasePolicyModel();
                    pm.DefineNewMe(p.Value, false, null, this);
                    this.Policies.Add(pm);
                }
            });

            List<PurchasePolicyModel> removePol = new List<PurchasePolicyModel>();
            this.Policies.ForEach(c =>
            {

                if (!updatedProduct.PurchasePolicies.Any(x => x.Key == c.PolicyID))
                {
                    removePol.Add(c);
                }
            });
            removePol.ForEach(c => this.Policies.Remove(c));

        }
    }
}