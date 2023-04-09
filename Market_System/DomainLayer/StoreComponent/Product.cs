using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Product : Property
    {
        // make the fields and/or its getter/setter threadsafe

        public String Product_ID { get; private set; } // composed of 2 parts: *storeId*_*inStoreProductId* - 1234_9876 for exmpl
        public String StoreID { get; private set; }
        public String Name { get; private set; }
        public String Description { get; private set; }
        public double Price { get; private set; }
        public int ReservedQuantity { get; private set; }
        public double Rating { get; private set; } // between 1-10
        public int Quantity { get; private set; }
        public double Weight { get; private set; }
        public double Sale { get; private set; } // 1-100 percentage  - temporary variant before PurchasePolicy implmnt
        public long timesBought { get; private set; }
        public Category ProductCategory { get; private set; }   // (mabye will be implementing by composition design pattern to support a sub catagoring.)
        public Array<double> Dimenssions { get; private set; } // array of 3
        public ConcurrentDictionary<string, Purchase_Policy> PurchasePolicies { get; private set; } // make it threadsafe ChaiinOfResponsobolities 
        public ConcurrentDictionary<string, Purchase_Strategy> PurchaseStrategies { get; private set; } // make it threadsafe ChainOfResponsibilities
        public ConcurrentBag<string> Comments { get; private set; }
        private StoreRepo storeRepo;
        public ConcurrentDictionary<String, List<String>> PurchaseAttributes { get; private set; }
        private static Category defaultCategory = new Category("NoCategory");


        // ==========================================================================================================


        public Product(String product_ID, String name, String, String description, double price, int initQuantity, int reservedQuantity, double rating, double sale, double weight, Array<int> dimenssions, List<String> comments, List<Purchase_Policy> purchase_Policies,
                        List<Purchase_Strategy> purchase_Strategies, Dictionary(string, List<string>) product_Attributes, int boughtTimes, Category category) // add more
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
            this.PurchasePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            this.PurchaseStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            foreach (Purchase_Policy p in purchase_Policies) this.PurchasePolicies.TryAdd(p.GetID(), p);
            foreach (Purchase_Strategy p in purchase_Strategies) this.PurchaseStrategies.TryAdd(p.GetID(), p);
            this.Comments = comments;
            this.storeRepo = StoreRepo.GetInstance();
            this.PurchaseAttributes = new ConcurrentDictionary<string, List<string>>(product_Attributes);
        }


        public void AddPurchasePolicy(Purchase_Policy newPolicy)
        {
            try
            {
                if (this.PurchasePolicies.TryAdd(newPolicy.GetID(), newPolicy))
                    Save();
            } catch (Exception e) { throw e; }
        }

        public void RemovePurchasePolicy(String policyID) {
            try
            {
                if (this.PurchasePolicies.TryRemove(policyID, out _))
                    Save();
            } catch (Exception e) { }
        }

        public void AddPurchaseStrategy(Purchase_Strategy newStrategy)
        {
            try
            {
                if (this.PurchaseStrategies.TryAdd(newStrategy.GetID(), newStrategy)
                    Save();
            }
            catch (Exception e) { return false; }
        }


        public void RemovePurchaseStrategy(String strategyID)
        {
            try
            {
                if (this.PurchaseStrategies.TryRemove(strategyID, out _))
                    Save();
            }
            catch (Exception e) { return false; }
        }


        public void AddAtribute(string attribute, List<string> options)
        {
            try
            {
                if(this.PurchaseAttributes.TryAdd(attribute, options))
                    Save();
            } catch (Exception e) { throw e; }
        }

        public void RemoveAttribute(string attribute, List<string> options)
        {
            try
            {
                if (this.PurchaseAttributes.TryRemove(attribute))
                    Save();
                return false;
            }
            catch (Exception e) { throw e; }
        }


        public double ImplementSale(List<String> chosenAttributes)
        {
            return this.Price - (this.Price / 100 * this.Sale);
            // PurchasePolicies.accept(chosenAttributes); // chain of responsibility that returns the price with implemented sale
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

        public double CalculatePrice(int quantity, Boolean implementSale) // maybe can receive some properties to coordinate the calculation (for exmpl - summer sale in whole MarketSystem)
        {
            //change later to - return ImplementSale(attributes) * quantity;
            try
            {
                if (!implementSale)
                    return ImplementSale() * quantity;
                else
                    return this.Price * quantity;
            } catch (Exception e) { throw e; }
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
                    Save();
                } catch (Exception e) { throw e;}
            }
        }

        public void Reserve(int quantity)
        {
            try
            {
                lock (this.ReservedQuantity)
                {
                    if ((this.ReservedQuantity + quantity) <= this.Quantity)
                    {
                        this.ReservedQuantity += quantity;
                        Save();
                    }
                }
            }
            catch (Exception e) { throw e; }
        }


        public void LetGoProduct(int quantity)
        {
            try
            {
                lock (this.ReservedQuantity)
                {
                    if ((this.ReservedQuantity - quantity) > 0)
                    {
                        this.ReservedQuantity -= quantity;
                        Save();
                    }
                }
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
                Comments.Add(userID + ": " + comment + ".\n Rating: " + rating + ".");
                UpdateRating(rating);
                Save();
            }
            catch (Exception e) { throw e; }
        }


        public Boolean prePurchase(int quantity)
        {
            try
            {
                lock (this.Quantity)
                {
                    return (this.Quantity - quantity) >= 0;
                }
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
                this.storeRepo.SaveProduct(this);
            } catch (Exception e) { throw e; }
        }


        // ====================================================================================
        // =============================== set functions ======================================
        public void SetName(string name)
        {
            try
            {
                lock (this.Name)
                {
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
                lock (this.Price)
                {
                    this.Price = price;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void SetRating(double raring)
        {
            try
            {
                lock (this.Rating)
                {
                    this.Rating = raring;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void SetQuantity(int quantity)
        {
            try
            {
                lock (this.Quantity)
                {
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
                lock (this.Weight)
                {
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
                lock (this.Sale)
                {
                    this.Sale = sale;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetTimesBought(int times)
        {
            try
            {
                lock (this.timesBought)
                {
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
                        this.ProductCategory = this.defaultCategory;
                    else 
                        this.ProductCategory = category;
                    Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void SetDimenssions(Array<double> dims)
        {
            try
            {
                lock (this.Dimenssions)
                {
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