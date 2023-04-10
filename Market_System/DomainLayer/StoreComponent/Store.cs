using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Store : Property
    {
        public enum MarketManagerPermission { MARKETMANAGER, NOTMARKETMANAGER }; // remove this permission later - until EmployeePermissions class is done
        //Implement all of the Property Methods here
        public string Store_ID { get; private set; }
        public string Name { get; private set; }
        private ConcurrentBag<string> allProducts;
        private ConcurrentDictionary<string, Product> products;
        private ConcurrentDictionary<string, int> productUsage;
        private Employees employees;
        public String founderID { get; private set; } //founder's userID
        private StoreRepo storeRepo;
        private ConcurrentDictionary<string, Purchase_Policy> defaultPolicies; // passed to every new added product
        private ConcurrentDictionary<string, Purchase_Strategy> defaultStrategies; // passed to every new added product


        // builder for a new store - initialize all fields later
        public Store(string founderID, string storeID, List<Purchase_Policy> policies, List<Purchase_Strategy> strategies, ConcurrentBag<string> allProductsIDS)
        {
            this.Store_ID = storeID;
            this.founderID = founderID;
            this.storeRepo = StoreRepo.GetInstance();
            this.employees = new Employees();
            this.products = new ConcurrentDictionary<string, Product>();
            this.productUsage = new ConcurrentDictionary<string, int>();
            this.defaultPolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            this.defaultStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();

            foreach (Purchase_Policy p in policies) this.defaultPolicies.TryAdd(p.GetID(), p);
            foreach (Purchase_Strategy p in strategies) this.defaultStrategies.TryAdd(p.GetID(), p);

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


        public void ChangeName(string userID, string newName)
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



        public void ManagePermissions(string userID, string employeeID, List<Permission> perms)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.OWNERAPPOINT))
                    this.employees.AddNewEmpPermissions(employeeID, this.Store_ID, perms);
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
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.OWNERAPPOINT))
                    this.employees.AssignNewManager(Store_ID, userID, newManagerID);
            }
            catch (Exception ex) { throw ex; }
        }


        public List<string> GetOwnersOfTheStore(string userID)
        {
            if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO)) // ADD - or market manager
                this.employees.GetOwners(this.Store_ID);
        }


        public List<string> GetManagersOfTheStore(string userID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))  // ADD - or market manager
                    return this.employees.GetManagers(this.Store_ID);
            }
            catch (Exception ex) { throw ex; }
        }

        public void AddEmployeePermission(string userID, string employeeID, Permission newP)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.OWNERAPPOINT))
                    this.employees.AddAnEmpPermission(employeeID, this.Store_ID, newP);
            } catch (Exception ex) { throw ex; }
        }


        public void RemoveEmployeePermission(string userID, string employeeID, Permission permToRemove)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.OWNERAPPOINT))
                    this.employees.RemoveAnEmpPermission(employeeID, this.Store_ID, permToRemove); // validate this method added to EmployeesPermission
            }
            catch (Exception ex) { throw ex; }
        }


        public List<string> GetPurchaseHistoryOfTheStore(string userID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))  // ADD - or market manager
                    this.storeRepo.GetPurchaseHistory(this.Store_ID);
            }
            catch (Exception ex) { throw ex; }
        }


        public StoreDTO GetStoreDTO()
        {
            try
            {
                return new StoreDTO(this); // "copy constructor"
            } catch (Exception ex) { throw ex; }
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
                if (this.founderID != userID) // ADD - maket manager permission validation
                    throw new Exception("Only store founder or Market Manager can remove a store.");
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

        public void AddStorePurchasePolicy(string userID, Purchase_Policy newPolicy)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))
                {
                    if (this.defaultPolicies.TryAdd(newPolicy.GetID(), newPolicy))
                        Save();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void RemoveStorePurchasePolicy(string userID, String policyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))
                {
                    if (p.GetID() == policyID)
                        if (this.defaultPolicies.TryRemove(policyID, out _))
                            Save();

                }
            }
            catch (Exception e) { throw e; }
        }

        public void AddStorePurchaseStrategy(string userID, Purchase_Strategy newStrategy)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))
                {
                    if (this.defaultStrategies.TryAdd(newStrategy.GetID(), newStrategy))
                        Save();
                }
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveStorePurchaseStrategy(string userID, String strategyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))
                {

                    if (p.GetID() == strategyID)
                        if (this.defaultStrategies.TryRemove(strategyID, out _))
                            Save();
                }
            }
            catch (Exception e) { throw e; }
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


        // ===================== END of Store operations =========================
        // =======================================================================







        // ==================================================================
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
                    if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                        Product newProduct = new Product(productProperties, this.Store_ID, this.defaultPolicies, this.defaultStrategies);
                        this.storeRepo.AddProduct(newProduct);
                        this.allProducts.Add(newProduct.Product_ID);
                        Save();
                    }
                }
                catch (Exception ex) { throw ex; }
            }

        }


        public void RemoveProduct(string userID, string product_id)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // ADD - or market manager
                {
                    this.storeRepo.RemoveProduct(product_id);
                    this.products.TryRemove(product_id, out _);
                    this.productUsage.TryRemoveProduct(product_id, out _);
                    this.allProducts.TryTake(product_id);
                    Save();
                }
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
                // ADD - validate user bought the product by purchase history
                AcquireProduct(productID).AddComment(userID, comment, rating);
                ReleaseProduct(productID);
            } catch (Exception ex) { throw ex; }
        }

        public void ChangeProductName(string userID, string productID, string name)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetName(name);
                    ReleaseProduct(productID);
                }

            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductDescription(string userID, string productID, string description)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetDescription(description);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductPrice(string userID, string productID, double price)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetPrice(price);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductRating(string userID, string productID, double rating, MarketManagerPermission perm)
        {
            try
            {
                if (perm.Equals(MarketManagerPermission.MARKETMANAGER)) // // change later after market manager permission enum added
                {
                    AcquireProduct(productID).SetRating(rating);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }
        public void ChangeProductQuantity(string userID, string productID, int quantity)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetQuantity(quantity);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductWeight(string userID, string productID, double weight)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetWeight(weight);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductSale(string userID, string productID, double sale)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetSale(sale);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductTimesBought(string userID, string productID, int times, MarketManagerPermission perm) // only market manager can do
        {
            try
            {
                if (perm.Equals(MarketManagerPermission.MARKETMANAGER)) // change later after market manager permission enum added
                {
                    AcquireProduct(productID).SetTimesBought(times);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductProductCategory(string userID, string productID, Category category)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetProductCategory(category);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductDimenssions(string userID, string productID, Array<double> dims)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).SetDimenssions(dims);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void AddProductPurchasePolicy(string userID, string productID, Purchase_Policy newPolicy)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).AddPurchasePolicy(newPolicy);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void RemoveProductPurchasePolicy(string userID, string productID, String policyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).RemovePurchasePolicy(policyID);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void AddProductPurchaseStrategy(string userID, string productID, Purchase_Strategy newStrategy)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).AddPurchaseStrategy(newStrategy);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveProductPurchaseStrategy(string userID, string productID, String strategyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                {
                    AcquireProduct(productID).RemovePurchaseStrategy(strategyID);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }


        // ===================== END of Product operations =========================
        // =========================================================================




        // ================================================================
        // ======================== TODO ==================================



        // ======================== END of TODO ==================================
        // =======================================================================


    }

