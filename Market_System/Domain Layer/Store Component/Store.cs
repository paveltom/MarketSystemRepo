using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Store : Property
    {
        //Implement all of the Property Methods here
        public string Store_ID { get; private set; }
        public string Name { get; private set; }
        private ConcurrentBag<string> allProducts;
        private ConcurrentDictionary<string, Product> products; //<product_id, quantity>
        private ConcurrentDictionary<string, int> productUsage;
        private Employees employees;
        public String founderID { get; private set; } //founder's userID
        private StoreRepo storeRepo;
        private ConcurrentBag<string> defaultPolicies; // passed to every new added product
        private ConcurrentBag<string> defaultStrategies; // passed to every new added product

        public Store(string founderID, string storeID, List<string> newStoreDetails, List<string> allProductsIDS) // builder for a new store - initialize all fields later
        {
            this.Store_ID = storeID;
            this.founderID = founderID;
            this.storeRepo = StoreRepo.GetInstance();
            this.employees = Employees.GetInstance();
            if (allProductsIDS == null)
                this.allProducts = new ConcurrentBag<string>();
            else
                this.allProducts = allProductsIDS;
        }


        // ===================== Store operations =========================

        public string ChangeName(string userID, string newName)
        {
            try
            {
                // validate PERMISSION here
                this.Name = newName;
                storeRepo.UpdateStoreName(Store_ID, newName);
            }
            catch (Exception ex) { throw ex; }
        }

        public void AssignNewOwner(string userID, string newOwnerID)
        {
            try
            {
                // if userID has PERMISSION
                this.employees.AssignNewOwner(Store_ID, userID, newOwnerID);
            }
            catch (Exception ex) { throw ex; }
        }

        public void AssignNewManager(string userID, string newManagerID)
        {
            try
            {
                // validate PERMISSION
                this.employees.AssignNewManager(Store_ID, userID, newManagerID);
            }
            catch (Exception ex) { throw ex; }
        }


        public List<string> GetOwnersOfTheStore(string userID)
        {
            // validate PERMISSION
            this.employees.GetOwners(this.Store_ID);
        }


        public void GetManagersOfTheStore(string userID)
        {
            try
            {
                // validate PERMISSION here
                this.employees.GetManagers(this.Store_ID);
            }
            catch (Exception ex) { throw ex; }
        }


        public List<string> GetPurchaseHistoryOfTheStore(string userID)
        {
            try
            {
                // validate PERMSSION here
                this.storeRepo.GetPurchaseHistory(this.Store_ID);
            }
            catch (Exception ex) { throw ex; }
        }


        public StoreDTO GetStoreDTO()
        {
            try
            {
                return new StoreDTO(this); // "copy constructor"
            }catch (Exception ex) { throw ex; }
        }


        public List<ItemDTO> GetItems()
        {
            try
            {
                List<ItemDTO> productList = new List<ItemDTO>();
                foreach (String s in allProducts)
                {
                    productList.Add(AcquireProduct(s).GetProductDTO());
                    ReleaseProduct(s);
                }
                return productList;

            } catch (Exception ex) { throw ex; }    
        }


        public void RemoveStore(string userID)
        {
            try
            {
                // permission required
                this.storeRepo.RemoveStore(this.Store_ID);
                foreach (String s in allProducts)
                    AcquireProduct(s).RemoveProduct();
                this.employees.RemoveStore(this.Store_ID);
                // remove policies and strategies
                        
            } catch (Exception ex) { throw ex; }
           
        }


        private static object CalculatePriceLock = new object();
        public double CalculatePrice(List<ItemDTO> productsToCalculate)
        {
            lock (CalculatePriceLock)
            {
                try
                {
                    double price = 0;
                    foreach (ItemDTO item in productsToCalculate)
                    {
                        price += AcquireProduct(item.GetID()).CalculatePrice(item.GetQuantity());
                    }
                    return price;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        private static object PurchaseLock = new object();
        public void Purchase(string userID, List<ItemDTO> productsToPurchase)
        {
            lock (PurchaseLock)
            {
                String cannotPurchase = ""; // will look like "ite#1ID_Name;item#2ID_Name;item#3IDName;..."
                try
                {
                    foreach (ItemDTO item in productsToCalculate)
                        if (!AcquireProduct(item.GetID()).prePurchase(item.GetQuantity()))
                            cannotPurchase.Concat(item.GetID().Concat(";"));

                    if (!cannotPurchase.Equals("")) throw new Exception(cannotPurchase);
                    else
                        foreach (ItemDTO item in productsToCalculate)
                            AcquireProduct(item.GetID()).Purchase(userID, item.GetQuantity());
                }
                catch (Exception ex) { throw new Exception(cannotPurchase, ex); }
            }
        }


        private string GetStoreIdFromProductID(string productID)
        {
            try
            {
                return productID.Substring(0, productID.IndexOf("_"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        // ================================================================







        // ===================== Product operations =========================

        private Product AcquireProduct(string productID)
        {
            try
            {
                return ((Lazy<Product>)products.GetOrAdd(storeID, (k, val) => new Lazy<Product>(() =>
                {
                    productUsage.GetOrAdd(k, 1, (k, val) => val + 1);
                    return storeRepo.GetProduct(k);
                }))).Value; // valueFactory could be calle multiple timnes so Lazy instance may be created multiple times also, but only one will actually be used
            }
            catch (Exception e) { throw e; }
        }

        private static object ReleaseProductLock = new object();
        private void ReleaseProduct(string productID)
        {
            lock (ReleaseProductLock)
            {
                try
                {
                    if (productUsage.TryRemove(productID, 1))
                        products.TryRemove(productID, out _);
                    else
                        productUsage.TryUpdate(productID, (productUsage.TryGetValue(productID) - 1), _);
                }
                catch (Exception e) { throw e; }
            }
        }

        private static object AddProductLock = new object();
        public void AddProduct(string userID, List<string> productProperties)
        {
            lock (AddProductLock)
            {
                try
                {
                    // CHECK USER PERMISSION
                    Product newProduct = new Product(productProperties); // separate - retreive all the properties from the list and pass to builder
                    this.storeRepo.AddProduct(newProduct);
                }
                catch (Exception ex) { throw ex; }
            }

        }

        
        public void RemoveProduct(string userID, string product_id)
        {
            try
            {
                storeRepo.RemoveProduct(product_id);
                products.TryRemove(product_id, out _);
                productUsage.TryRemoveProduct(product_id, out _);

            }
            catch (Exception ex) { throw ex; }
        }


        public void ReserveProduct(ItemDTO reserveProduct)
        {
            try
            {
                return AcquireProduct(reserveProduct.GetID()).Reserve(reserveProduct.GetQuantity());
            }
            catch (Exception ex) { throw ex; }
        }

        public void LetGoProduct(ItemDTO reservedProduct)
        {
            try
            {
                return AcquireProduct(reservedProduct.GetID()).LetGoProduct(reservedProduct.GetQuantity());
            }
            catch (Exception ex) { throw ex; }
        }


        public void AddProductComment(string userID, string productID, string comment, double rating)
        {
            try
            {
                AcquireProduct(productID).AddComment(userID, comment, rating);
            } catch (Exception ex) { throw ex; }
        }


        // ================================================================







        // ========Methods ToDo==========:

        /*
         public void EditProduct(string userID, string productID, List<string> editedDetails)
        {
            // has to be separated into sub-editions: editWeight(), editQuantity(), etc on higher level
            throw new NotImplementedException();
        }
         */

        
        
        
        // ======more ideas=====:

        // passing a data for store representation (including what details?)
        // passing a data for store content representation- what products availble for a buyer
        // *maby should an additional store content view, a seperate view for a manager of the store
        // (search)-get products by name: getting all products with same name *or similar name
        // (search)-get products by catagory: getting all products from that catagory
        /*
         *  .חיפוש מוצרים ללא התמקדות בחנות ספציפית, לפי שם המוצר, קטגוריה או מילות מפתח.
         *  ניתן לסנן את התוצאות בהתאם למאפיינים כגון: טווח מחירים, דירוג המוצר, קטגוריה
         * דירוג החנות וכד'.
         */
    }
}