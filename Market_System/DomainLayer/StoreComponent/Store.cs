using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Store : Property
    {
        //Implement all of the Property Methods here
        public string Store_ID { get; private set; }
        public string Name { get; private set; }
        private ConcurrentBag<string> allProducts;
        private ConcurrentDictionary<string, Product> products; 
        private ConcurrentDictionary<string, int> productUsage;
        private EmployeesPermissions employees; 
        public String founderID { get; private set; } //founder's userID
        private StoreRepo storeRepo;
        private ConcurrentBag<string> defaultPolicies; // passed to every new added product
        private ConcurrentBag<string> defaultStrategies; // passed to every new added product


        // builder for a new store - initialize all fields later
        public Store(string founderID, string storeID, List<string> newStoreDetails, List<string> allProductsIDS) 
        {
            this.Store_ID = storeID;
            this.founderID = founderID;
            this.storeRepo = StoreRepo.GetInstance();
            this.employees = new EmployeesPermissions();
            if (allProductsIDS == null)
                this.allProducts = new ConcurrentBag<string>();
            else
                this.allProducts = allProductsIDS;
            this.employees.AddNewOwnerEmpPermissions(this.founderID, this.Store_ID);
        }


        // ===================== Store operations =========================

        public void TransferFoundership(string userID, string newFounderID)
        {
            try
            {
                lock (this.Name)
                {
                    if (this.founderID != userID)
                        throw new Exception("Only founder can transfer foundership.");
                    this.founderID = newFounderID;
                    Save();
                }

            } catch (Exception ex) { throw ex; }
        }
        
        
        public string ChangeName(string userID, string newName)
        {
            try
            {
                if (userID != this.founderID)
                    throw new Exception("Only store founder can change its name.");
                this.Name = newName;
                Save();
            }
            catch (Exception ex) { throw ex; }
        }

        

        public void ManagePermissions(string userID, List<Permission> perms)
        {
            try
            {
                this.employees.AssignNewOwner(Store_ID, userID, newOwnerID);
            }
            catch (Exception ex) { throw ex; }
        }


        public void AssignNewOwner(string userID, string newOwnerID)
        {
            try
            {
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
                {
                    AcquireProduct(s).RemoveProduct();
                    ReleaseProduct(s);
                }
                this.employees.RemoveStore(this.Store_ID); // change to remove as Yotam explainbed
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
                        ReleaseProduct(item.GetID());
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
                String cannotPurchase = ""; // will look like "item#1ID_Name;item#2ID_Name;item#3IDName;..."
                try
                {
                    foreach (ItemDTO item in productsToCalculate)
                        if (!AcquireProduct(item.GetID()).prePurchase(item.GetQuantity()))
                        {
                            cannotPurchase.Concat(item.GetID().Concat(";"));
                            ReleaseProduct(item.GetID());
                        }

                    if (!cannotPurchase.Equals("")) throw new Exception(cannotPurchase);
                    else
                        foreach (ItemDTO item in productsToCalculate)
                        {
                            AcquireProduct(item.GetID()).Purchase(userID, item.GetQuantity());
                            ReleaseProduct(item.GetID());
                        }
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


        // call me every time data changes
        private void Save()
        {
            try
            {
                this.storeRepo.SaveStore(this);
            }
            catch (Exception e) { throw e; }
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
                    this.allProducts.Add(newProduct.Product_ID);
                    Save();
                }
                catch (Exception ex) { throw ex; }
            }

        }

        
        public void RemoveProduct(string userID, string product_id)
        {
            try
            {
                this.storeRepo.RemoveProduct(product_id);
                this.products.TryRemove(product_id, out _);
                this.productUsage.TryRemoveProduct(product_id, out _);
                this.allProducts.TryTake(product_id);
                Save();
            }
            catch (Exception ex) { throw ex; }
        }


        public void ReserveProduct(ItemDTO reserveProduct)
        {
            try
            {
                AcquireProduct(reserveProduct.GetID()).Reserve(reserveProduct.GetQuantity());
                ReleaseProduct(reserveProduct.GetID());
            }
            catch (Exception ex) { throw ex; }
        }

        public void LetGoProduct(ItemDTO reservedProduct)
        {
            try
            {
                AcquireProduct(reservedProduct.GetID()).LetGoProduct(reservedProduct.GetQuantity());
                ReleaseProduct(reservedProduct.GetID());
            }
            catch (Exception ex) { throw ex; }
        }


        public void AddProductComment(string userID, string productID, string comment, double rating)
        {
            try
            {
                AcquireProduct(productID).AddComment(userID, comment, rating);
                ReleaseProduct(productID);
            } catch (Exception ex) { throw ex; }
        }


        // ================================================================
        // ======================== TODO ==================================

        public void ChangeProductName(string name)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductDescription(string description)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductPrice(double price)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductRating(double raring)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductQuantity(int quantity)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductWeight(double weight)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductSale(double sale)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductTimesBought(int times)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductProductCategory(Category category)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductDimenssions(Array<double> dims)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }


         public void EditProduct(string userID, string productID, List<string> editedDetails)
        {
            // has to be separated into sub-editions: editWeight(), editQuantity(), etc on higher level
            throw new NotImplementedException();
        }



        public void AddStorePurchasePolicy(Purchase_Policy newPolicy)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void RemoveStorePurchasePolicy(String policyID)
        {
            try
            {

            }
            catch (Exception e) { }
        }

        public void AddStorePurchaseStrategy(Purchase_Strategy newStrategy)
        {
            try
            {

            }
            catch (Exception e) { return false; }
        }


        public void RemoveStorePurchaseStrategy(String strategyID)
        {
            try
            {

            }
            catch (Exception e) { return false; }
        }


        public void AddProductPurchasePolicy(Purchase_Policy newPolicy)
        {
            try
            {

            }
            catch (Exception e) { throw e; }
        }

        public void RemoveProductPurchasePolicy(String policyID)
        {
            try
            {

            }
            catch (Exception e) { }
        }

        public void AddProductPurchaseStrategy(Purchase_Strategy newStrategy)
        {
            try
            {

            }
            catch (Exception e) { return false; }
        }


        public void RemoveProductPurchaseStrategy(String strategyID)
        {
            try
            {

            }
            catch (Exception e) { return false; }
        }


