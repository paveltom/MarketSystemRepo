﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Product : Property
    {
        // make the fields and/or its getter/setter threadsafe

        public String Product_ID { get; private set; } // composed of 2 parts: *storeId*_*inStoreProductId* - 1234_9876 for exmpl
        public String StoreID { get; private set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public int ReservedQuantity { get; set; }
        public double Rating { get; set; } // between 1-10
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public long timesBought;
        public Category ProductCategory { get; set; }   // (mabye will be implementing by composition design pattern to support a sub catagoring.)
        public Array<int> Dimenssions { get; set; } // array of 3
        public ConcurrentBag<string> PurchasePolicies { get; private set; } // make it threadsafe ChaiinOfResponsobolities 
        public ConcurrentBag<string> PurchaseStrategies { get; private set; } // make it threadsafe ChainOfResponsibilities
        public ConcurrentBag<string> Comments { get; private set; }
        private StoreRepo storeRepo;
        public ConcurrentDictionary<String, List<String>> PurchaseAttributes { get; private set; }

       
        // 2 Different Builder for a new product or for creating one from database:

        public Product(String product_ID, String name, String, String description, double Price, int initQuantity, double rating, double weight, Array<int> dimenssions, List<String> comments, List<Purchase_Policy> purchase_Policies,
                        List<Purchase_Strategy> purchase_Strategies, List<String>> product_Attributes) // add more
        {
            // init fields
            Save();

        }

        public Product(List<string> properties)// mostly strings of ids or string representation: product_ID, name, description, Price, initQuantity, weight, dimenssions, purchase_Policies, purchase_Strategies, product_Attributes
        {
            // init fields
            Save();
        }


        /*
        private Boolean Retrieve_Product()
        {
            Product p = this.storeRepo.GetProduct(this.Product_ID);
            this.StoreID = p.StoreID;
            this.Name = p.Name;
            this.Description = p.Description;
            this.Price = p.Price;
            this.ReservedQuantity = p.ReservedQuantity;
            this.Rating = p.Rating;
            this.Weight = p.Weight;
            this.ProductCategory = p.ProductCategory;
            this.PurchasePolicies = p.PurchasePolicies;
            this.PurchaseStrategies = p.PurchaseStrategies;
            this.Comments = p.Comments;
            this.Dimenssions = p.Dimenssions;
            this.PurchaseAttributes = p.PurchaseAttributes;
        }
        */


        public void AddPurchasePolicy(Purchase_Policy newPolicy)
        {
            try
            {
                if (this.PurchasePolicies.Contains(newPolicy.GetID()))
                    return false;
                this.storeRepo.AddProductPolicy(this.Product_ID, newPolicy);
                this.PurchasePolicies.Add(newPolicy.GetID());
                return true;
            } catch (Exception e) { throw e; }
        }

        public void RemovePurchasePolicy(String policyID)
        {
            try
            {
                if (this.PurchasePolicies.TryTake(policyID))
                {
                    this.storeRepo.RemoveProductPolicy(this.Product_ID, policyID);
                    return true;
                }
                return false;
            } catch (Exception e) { }
        }

        public void AddPurchaseStrategy(Purchase_Strategy newStrategy)
        {
            try
            {
                if (this.PurchaseStrategies.Contains(newPolicy.GetID()))
                    return false;
                this.storeRepo.AddProductStrategy(this.Product_ID, newStrategy);
                this.PurchaseStrategies.Add(newPolicy.GetID());
                return true;
            }
            catch (Exception e) { return false; }
        }

        public void RemovePurchasePolicy(String strategyID)
        {
            try
            {
                if (this.PurchaseStrategies.TryTake(policyID))
                {
                    this.storeRepo.RemoveProductPolicy(this.Product_ID, policyID);
                    return true;
                }
                return false;
            }
            catch (Exception e) { return false; }
        }


        public void AddAtribute(string attribute, List<string> options)
        {
            try
            {
                if (this.storeRepo.AddProductAttribute(Product_ID, attribute, options))
                    return this.PurchaseAttributes.TryAdd(attribute, options);
                else
                    return false;
            } catch (Exception e) { throw e; }
        }

        public void RemoveAttribute(string attribute, List<string> options)
        {
            try
            {
                if (this.PurchaseAttributes.TryRemove(attribute))
                    return this.storeRepo.RemoveProductAttribute(this.Product_ID, attribute);
                return false;
            }
            catch (Exception e) { throw e; }
        }


        public double ImplementSale(List<String> chosenAttributes)
        {
            throw new NotImplementedException();
            PurchasePolicies.accept(chosenAttributes); // chain of responsibility that returns the price with implemented sale
            // get items
            // concrete product varies by PurchaseAttributes, so the sale calculated considering chosenAttributes by
            // applying PurchasePolicies chain of responsibility
        }

        public string GetStoreID()
        {
            try
            {
                return Product_ID.Substring(0, Product_ID.IndexOf("_"));
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }

        public double CalculatePrice(int quantity) // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)
        {
            //change later to - return ImplementSale(attributes) * quantity;
            return this.Price * quantity;
        }

        
        private static object PurchaseLock = new object();
        public void Purchase() // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)
        {
            lock(PurchaseLock)
            {
                try
                {                  
                    Interlocked.Decrement(this.Quantity);
                    Interlocked.Decrement(this.ReservedQuantity);
                    // save()
                } catch (Exception e) { throw e;}
            }
        }

        public void Reserve(int quantity)
        {
            try
            {
                if (this.storeRepo.ReserveProduct(Product_ID, quantity))
                {
                    ReservedQuantity += quantity;
                    return true;
                }
                return false;
            } catch (Exception e) { throw e; }
        }


        public void LetGoProduct(int quantity)
        {
            try
            {
                if (this.storeRepo.ReleaseProduct(Product_ID, quantity))
                {
                    ReservedQuantity -= quantity;
                    return true;
                }
                return false;
            } catch (Exception e) { throw e; }
        }


        private static object UpdateRatingLock = new object();
        private void UpdateRating(double rating)
        {
            lock (UpdateRatingLock)
            {
                try
                {
                    if (timesBought > 0)
                    {
                        this.Rating = (this.Rating * timesBought + rating) / (timesBought + 1);
                        this.timesBought++;
                    }
                }
                catch (Exception e) { throw e; }
            }
        }


        public void AddComment(string userID, string comment, double rating)
        {
            // validate user have purchased the product - else throw exception
            try
            {
                Comments.Add(userID + ": " + comment + ".\n Rating: " + rating + ".");
                UpdateRating(rating);
            }
            catch (Exception e) { throw e; }
        }


        public Boolean prePurchase(int quantity)
        {
            try
            {
                return quantity > 0;
            } catch (Exception e) { throw e; }

        }

        public ItemDTO GetProductDTO()
        {
            try
            {
                return new ItemDTO(this);
            }catch (Exception e) { throw e; }
        }


        public void RemoveProduct()
        {
            try
            {
                this.storeRepo.RemoveProduct(this.Product_ID);
            } catch (Exception e) { throw e; } 
        }


        // call me every time data changes
        private void Save()
        {
            try
            {
                this.storeRepo.UpdateProduct(this);
            } catch (Exception e) { throw e; }
        }








        // ======================= Methods ToDo ================================:




    }

 
}