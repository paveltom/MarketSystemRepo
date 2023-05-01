﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Product : Property
    {
        // make the fields and/or its getter/setter threadsafe

        public String Product_ID { get; private set; } // composed of 2 parts: *storeId*_*inStoreProductId* - 1234_9876 for exmpl
        public String StoreID { get; private set; }
        public String Name { get; private set; }
        public String Description { get; private set; }
        public Double Price { get; private set; }
        public int ReservedQuantity { get; private set; }
        public Double Rating { get; private set; } // between 1-10
        public int Quantity { get; private set; }
        public Double Weight { get; private set; }
        //public Double Sale { get; private set; } // 1-100 percentage  - temporary variant before PurchasePolicy implmnt
        public long timesBought { get; private set; }
        public long timesRated { get ; private set; }
        public Category ProductCategory { get; private set; }   // (mabye will be implementing by composition design pattern to support a sub catagoring.)
        public Double[] Dimenssions { get; private set; } // array of 3
        public ConcurrentDictionary<string, Purchase_Policy> PurchasePolicies { get; private set; } // make it threadsafe ChaiinOfResponsobolities 
        public ConcurrentDictionary<string, Purchase_Strategy> PurchaseStrategies { get; private set; } // make it threadsafe ChainOfResponsibilities
        public ConcurrentBag<string> Comments { get; private set; }
        private StoreRepo storeRepo;
        public ConcurrentDictionary<String, List<String>> PurchaseAttributes { get; private set; }
        private static Category defaultCategory = new Category("NoCategory");
        private static object QuantityLock = new object();
        private static object GeneralPropertiesLock = new object();

        // ==========================================================================================================


        public Product(String product_ID, String name, String description, double price, int initQuantity, int reservedQuantity, double rating, double sale, double weight,
                        double[] dimenssions, List<String> comments, ConcurrentDictionary<string, Purchase_Policy> purchase_Policies,
                        ConcurrentDictionary<string, Purchase_Strategy> purchase_Strategies, Dictionary<string, List<string>> product_Attributes, int boughtTimes, Category category)
        {
            this.Product_ID = product_ID;
            this.StoreID = this.Product_ID.Substring(0, this.Product_ID.IndexOf("_"));
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.ReservedQuantity = reservedQuantity;
            this.Rating = rating;
            this.Quantity = initQuantity;
            this.Weight = weight;
            this.Sale = sale;
            this.timesBought = boughtTimes;
            this.ProductCategory = category;
            this.Dimenssions = dimenssions;
            this.PurchasePolicies = purchase_Policies;
            this.PurchaseStrategies = purchase_Strategies;
            this.Comments = new ConcurrentBag<string>(comments);
            this.storeRepo = StoreRepo.GetInstance();
            this.PurchaseAttributes = new ConcurrentDictionary<string, List<string>>(product_Attributes);
        }


        public Product(List<String> productProperties, string storeID, ConcurrentDictionary<string, Purchase_Policy> defaultStorePolicies,
                        ConcurrentDictionary<string, Purchase_Strategy> defaultStoreStrategies)
        {
            // productProperties = {Name, Description, Price, Quantity, ReservedQuantity, Rating, Sale ,Weight, Dimenssions, PurchaseAttributes, ProductCategory}
            this.StoreID = storeID;
            this.storeRepo = StoreRepo.GetInstance();
            this.Product_ID = storeRepo.getNewProductID(storeID); //this.storeRepo.getNewProductID(storeID); Change when StoreRepo done !!!!!!!!!!!!!!
            this.PurchasePolicies = new ConcurrentDictionary<string, Purchase_Policy>(defaultStorePolicies);
            this.PurchaseStrategies = new ConcurrentDictionary<string, Purchase_Strategy>(defaultStoreStrategies);
            this.Comments = new ConcurrentBag<string>();
            this.timesBought = 0;

            String[] properties = productProperties.ToArray();

            this.Name = properties[0];
            this.Description = properties[1];
            this.Price = Double.Parse(properties[2]);
            this.Quantity = Int32.Parse(properties[3]);
            this.ReservedQuantity = Int32.Parse(properties[4]);
            this.Rating = Double.Parse(properties[5]);
            this.Sale = Double.Parse(properties[6]);
            this.Weight = Double.Parse(properties[7]);
            this.Dimenssions = properties[8].Split('_').Select(s => Double.Parse(s)).ToArray(); // dim1_dim2_dim3
            this.PurchaseAttributes = RetreiveAttributres(properties[9]); // atr1Name:atr1opt1_atr1opt2...atr1opti;atr2name:atr2opt1...
            this.ProductCategory = new Category(properties[10]);
        }


        private ConcurrentDictionary<string, List<string>> RetreiveAttributres(String atrs)
        {
            try
            {
                List<string> wholeAtrs = atrs.Split(';').ToList();
                if(wholeAtrs.Count > 0)
                    wholeAtrs.RemoveAt(wholeAtrs.Count - 1);
                ConcurrentDictionary<string, List<string>> ret = new ConcurrentDictionary<string, List<string>>();
                foreach (string s in wholeAtrs)
                {
                    string[] atrArray = s.Split(':').ToArray();
                    string atrName = atrArray[0];
                    List<string> atrOpts = atrArray[1].Split('_').ToList();
                    ret.TryAdd(atrName, atrOpts);
                }
                return ret;
            }
            catch (Exception e) { throw e; }
        }


        public void AddPurchasePolicy(Purchase_Policy newPolicy)
        {
            try
            {
                if (this.PurchasePolicies.TryAdd(newPolicy.GetID(), newPolicy))
                    Save();
                else
                    throw new Exception("Policy already exists.");
            }
            catch (Exception e) { throw e; }
        }

        public void RemovePurchasePolicy(String policyID)
        {
            try
            {
                if (this.PurchasePolicies.TryRemove(policyID, out _))
                    Save();
                else throw new Exception("No such policy.");
            }
            catch (Exception e) { throw e; }
        }

        public void AddPurchaseStrategy(Purchase_Strategy newStrategy)
        {
            try
            {
                if (this.PurchaseStrategies.TryAdd(newStrategy.GetID(), newStrategy))
                    Save();
                else throw new Exception("Strategy already exist.");
            }
            catch (Exception e) { throw e; }
        }


        public void RemovePurchaseStrategy(String strategyID)
        {
            try
            {
                if (this.PurchaseStrategies.TryRemove(strategyID, out _))
                    Save();
                else
                    throw new Exception("No such strategy.");
            }
            catch (Exception e) { throw e; }
        }


        public void AddAtribute(string attribute, List<string> options)
        {
            try
            {
                if (this.PurchaseAttributes.TryAdd(attribute, options))
                    Save();
                else throw new Exception("Attribute already exists.");
            }
            catch (Exception e) { throw e; }
        }

        public void RemoveAttribute(string attribute)
        {
            try
            {
                if (this.PurchaseAttributes.TryRemove(attribute, out _))
                    Save();
                else throw new Exception("No such attribute.");
            }
            catch (Exception e) { throw e; }
        }


        public double ImplementSale(List<String> chosenAttributes, int quantity)
        {
            //double saledPrice = this.Price - (this.Price / 100 * this.Sale);
            double saledPrice = this.Price;
            foreach(Purchase_Policy pp in this.PurchasePolicies.Values)
            {
                saledPrice -= Math.Max(0, pp.ApplyPolicy(this.Price, quantity, chosenAttributes));
            }
                
            return quantity * saledPrice;
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

        public double CalculatePrice(int quantity, List<string> attributes) // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)
        {
            //change later to - return ImplementSale(attributes) * quantity;
            try
            {
                if (quantity < 1)
                    throw new Exception("Bad quantity!");
                return ImplementSale(attributes, quantity); // add chosen attributes functionality
            }
            catch (Exception e) { throw e; }
        }


        private static object PurchaseLock = new object();
        public void Purchase(int quantity) // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)
        {
            lock (PurchaseLock)
            {
                try
                {
                    lock (QuantityLock)
                    {
                        if (this.Quantity < quantity)
                            throw new Exception("Not enough product in Store.");
                        this.Quantity -= quantity;
                        this.ReservedQuantity -= quantity;
                        this.timesBought += quantity;
                    }
                    Save();
                }
                catch (Exception e) { throw e; }
            }
        }

        public void Reserve(int quantity)
        {
            try
            {
                lock (QuantityLock)
                {
                    if ((this.ReservedQuantity + quantity) <= this.Quantity)
                    {
                        this.ReservedQuantity += quantity;
                        Save();
                    }
                    else
                        throw new Exception("Can't reserve: quantity too large.");
                }
            }
            catch (Exception e) { throw e; }
        }


        public void LetGoProduct(int quantity)
        {
            try
            {
                lock (QuantityLock)
                {
                    if ((this.ReservedQuantity - quantity) > 0)
                    {
                        this.ReservedQuantity -= quantity;
                        Save();
                    }
                    else
                        throw new Exception("Cannot release more tha reserved.");
                }
            }
            catch (Exception e) { throw e; }
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
                        this.Rating = (this.Rating * timesRated + rating) / (timesRated + 1);
                        this.timesRated++;
                        Save();
                    }
                }
                catch (Exception e) { throw e; }
            }
        }


        public void AddComment(string userID, string comment, double rating)
        {
            try
            {
                // validate user have purchased the product - else throw exception
                if (rating >= 1)
                {
                    Comments.Add(userID + ": " + comment + ".\n Rating: " + rating + ".");
                    UpdateRating(rating);
                }
                else
                {
                    if(comment.Trim() == "")
                        throw new Exception("Cannot add empty comment with no rating.");  
                    Comments.Add(userID + ": " + comment + ".\n Rating: _ .");
                }
                Save();
            }
            catch (Exception e) { throw e; }
        }


        public Boolean prePurchase(int quantity, List<string> chosenAttributes)
        {
            try
            {
                lock (QuantityLock)
                {
                    if (quantity < 1)
                        throw new Exception("Bad quantity.");
                    
                    foreach(Purchase_Strategy ps in this.PurchaseStrategies.Values)
                    {
                        if(!ps.validate(quantity, chosenAttributes))
                        {
                            throw new Exception(ps.Description);
                        }
                    }
                    return (this.Quantity - quantity) >= 0;
                }
            }
            catch (Exception e) { throw e; }

        }

        public ItemDTO GetProductDTO()
        {
            try
            {
                ItemDTO return_me= new ItemDTO(this.Product_ID, this.Quantity); // !!!! Change to send ItemDTO(this) after StoreRepo done!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return_me.SetReservedQuantity(this.ReservedQuantity);
                return return_me;
            }
            catch (Exception e) { throw e; }
        }


        /*
        public void RemoveProduct(string founderID)
        {
            try
            {
                this.storeRepo.RemoveProduct(this.StoreID, founderID, this);
            } catch (Exception e) { throw e; } 
        }
        */


        // call me every time data changes
        private void Save()
        {
            try
            {
                if (this.testRepo != null)
                    testRepo.Save(this);
                else
                    this.storeRepo.saveProduct(this);
            }
            catch (Exception e) { throw e; }
        }


        // ====================================================================================
        // =============================== set functions ======================================
        public void SetName(string name)
        {
            try
            {
                lock (this.Name)
                {
                    if (name.Trim() == "")
                        throw new Exception("Name cannot be empty.");
                    this.Name = name;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void SetDescription(string description)
        {
            try
            {
                lock (this.Description)
                {
                    if (description.Trim() == "")
                        throw new Exception("Name cannot be empty.");
                    this.Description = description;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void SetPrice(double price)
        {
            try
            {
                lock (GeneralPropertiesLock)
                {
                    if (price < 0)
                        throw new Exception("Price can't be negative.");
                    this.Price = price;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void SetRating(double rating)
        {
            try
            {
                lock (GeneralPropertiesLock)
                {
                    if (rating < 1 || rating > 10)
                        throw new Exception("Rating has to be between 1-10");
                    this.Rating = rating;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void SetQuantity(int quantity)
        {
            try
            {
                lock (QuantityLock)
                {
                    if (quantity < 0)
                        throw new Exception("Quantity cannot be negative.");
                    this.Quantity = quantity;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetWeight(double weight)
        {
            try
            {
                lock (GeneralPropertiesLock)
                {
                    if (weight < 0)
                        throw new Exception("Weight cannot be negative (unless you are selling helium balloons, than contact our support).");
                    this.Weight = weight;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetSale(double sale)
        {
            try
            {
                lock (GeneralPropertiesLock)
                {
                    if (sale < 0 || sale > 100)
                        throw new Exception("Sale is percents, so must be between 0-100.");
                    this.Sale = sale;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetTimesBought(long times)
        {
            try
            {
                lock (GeneralPropertiesLock)
                {
                    if (times < 0)
                        throw new Exception("Cannot be negative.");
                    this.timesBought = times;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetProductCategory(Category category)
        {
            try
            {
                lock (this.ProductCategory)
                {
                    if (category == null)
                        this.ProductCategory = Product.defaultCategory;
                    else
                        this.ProductCategory = category;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetDimenssions(double[] dims)
        {
            try
            {
                lock (this.Dimenssions)
                {
                    foreach(double d in dims)
                        if(d <= 0)
                            throw new Exception("Dimenssion cannot be 0 or negative.");  
                    this.Dimenssions = dims;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }








        // =============================================================
        // ======================= TODO ================================



        // ======================= END of TODO ================================
        // ====================================================================




    }


}