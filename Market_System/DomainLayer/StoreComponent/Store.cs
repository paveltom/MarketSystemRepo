using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System.Web.WebSockets;

namespace Market_System.DomainLayer.StoreComponent
{
    public class Store : Property
    {
        public enum MarketManagerPermission { MARKETMANAGER, NOTMARKETMANAGER }; // remove this permission later - until EmployeePermissions class is done
        public string Store_ID { get; private set; }
        public string Name { get; private set; }
        public ConcurrentDictionary<string, string> allProducts;
        private ConcurrentDictionary<string, Product> products;
        private ConcurrentDictionary<string, int> productUsage;
        private Employees employees;
        public String founderID { get; private set; } //founder's userID
        private StoreRepo storeRepo;
        public ConcurrentDictionary<string, Purchase_Policy> productDefaultPolicies; // passed to every new added product
        public ConcurrentDictionary<string, Purchase_Strategy> productDefaultStrategies; // passed to every new added product
        public ConcurrentDictionary<string, Purchase_Policy> storePolicies; // passed to every new added product
        public ConcurrentDictionary<string, Purchase_Strategy> storeStrategies; // passed to every new added product
        private bool temporaryClosed = false;

        // builder for a new store - initialize all fields later
        public Store(string founderID, string storeID, List<Purchase_Policy> policies, List<Purchase_Strategy> strategies, List<string> allProductsIDS, bool temporaryClosed)
        {
            this.Store_ID = storeID;
            this.founderID = founderID;
            this.storeRepo = StoreRepo.GetInstance();
            this.employees = new Employees();
            this.products = new ConcurrentDictionary<string, Product>();
            this.productUsage = new ConcurrentDictionary<string, int>();
            this.storePolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            this.storeStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            this.productDefaultPolicies = new ConcurrentDictionary<string, Purchase_Policy>();
            this.productDefaultStrategies = new ConcurrentDictionary<string, Purchase_Strategy>();
            this.temporaryClosed = temporaryClosed;

            if (policies != null)
                foreach (Purchase_Policy p in policies)
                    this.storePolicies.TryAdd(p.GetID(), p);
            if (strategies != null)
                foreach (Purchase_Strategy p in strategies)
                    this.storeStrategies.TryAdd(p.GetID(), p);

            this.allProducts = new ConcurrentDictionary<string, string>();
            if (allProductsIDS != null)                
                foreach(string s in allProductsIDS)
                    this.allProducts.TryAdd(s, s);
            this.employees.AddNewFounderEmpPermissions(this.founderID, this.Store_ID);
        }


        // ===================== Store operations =========================

        public void TransferFoundership(string userID, string newFounderID)
        {
            try
            {
                lock (this.founderID)
                {
                    if (this.founderID != userID)
                        throw new Exception("Only founder can transfer foundership.");
                    this.founderID = newFounderID;
                    Save();
                }

            }
            catch (Exception ex) { throw ex; }
        }


        public void ChangeName(string userID, string newName)
        {
            try
            {
                lock (this.founderID)
                {
                    if (userID != this.founderID)
                        throw new Exception("Only store founder can change its name.");
                    this.Name = newName;
                    Save();
                }
            }
            catch (Exception ex) { throw ex; }
        }



        private static object EmployementLock = new object();
        public void ManagePermissions(string userID, string employeeID, List<Permission> perms) // update only for store manager
        {
            lock (EmployementLock)
            {
                try
                {
                    if (this.employees.isFounder(userID, this.Store_ID) || (this.employees.isOwner(userID, this.Store_ID) && 
                                                                this.employees.isManagerSubject(employeeID, userID, this.Store_ID)))
                        this.employees.updateEmpPermissions(employeeID, this.Store_ID, perms);
                    else
                        throw new Exception("You can't manage this employee permissions.");
                }
                catch (Exception ex) { throw ex; }
            }
        }



        public void AssignNewOwner(string userID, string newOwnerID)
        {
            lock (EmployementLock)
            {
                try
                {
                    if ((this.employees.isFounder(userID, this.Store_ID) || this.employees.isOwner(userID, this.Store_ID)) && !(this.employees.isOwner(newOwnerID, this.Store_ID)))
                        this.employees.AddNewOwnerEmpPermissions(userID, newOwnerID, this.Store_ID);
                    else
                        throw new Exception("Cannot assign new owner: you are not an owner in this store or employee is already an owner in this store.");
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public void AssignNewManager(string userID, string newManagerID) // manager added with default Permission.Stock
        {
            lock (EmployementLock)
            {
                try
                {
                    if ((this.employees.isFounder(userID, this.Store_ID) || this.employees.isOwner(userID, this.Store_ID)) && !this.employees.isManager(newManagerID, this.Store_ID))
                        this.employees.AddNewManagerEmpPermissions(userID, newManagerID, Store_ID, new List<Permission>() { Permission.STOCK });
                    else
                        throw new Exception("Cannot assign new manager: you are not an owner or this employee is already a manager in this store.");
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public List<string> GetOwnersOfTheStore(string userID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO)) // ADD - or market manager
                    return this.employees.GetOwnersOfTheStore(this.Store_ID);
                else
                    throw new Exception("You don't have a permission to view Store owners.");
            }
            catch (Exception ex) { throw ex; }
        }


        public List<string> GetManagersOfTheStore(string userID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))  // ADD - or market manager
                    return this.employees.GetManagersOfTheStore(this.Store_ID);
                else
                    throw new Exception("You don't have a permission to view Store managers.");
            }
            catch (Exception ex) { throw ex; }
        }

        public void AddEmployeePermission(string userID, string employeeID, Permission newP)
        {
            lock (EmployementLock)
            {
                try
                {
                    if ((this.employees.isFounder(userID, this.Store_ID)) || ((this.employees.isOwner(userID, this.Store_ID)) && 
                                                                            this.employees.isManagerSubject(employeeID, userID, this.Store_ID)))
                        this.employees.AddAnEmpPermission(employeeID, this.Store_ID, newP);
                    else
                        throw new Exception("You cannot add permissions for that employee.");
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public void RemoveEmployeePermission(string userID, string employeeID, Permission permToRemove)
        {
            lock (EmployementLock)
            {
                try
                {
                    if (userID == this.founderID || 
                        (this.employees.isOwner(userID, this.Store_ID) && this.employees.isManagerSubject(employeeID, userID, this.Store_ID)))
                        this.employees.removeAnEmpPermission(employeeID, this.Store_ID, permToRemove); // validate this method added to EmployeesPermission
                    else throw new Exception("You have no permission to remove this employee permisssion.");
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public string GetPurchaseHistoryOfTheStore(string userID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))  // ADD - or market manager
                    return this.storeRepo.getPurchaseHistoryOfTheStore(this.Store_ID) ;
                else
                    throw new Exception("You don't have a permissio to view Store purchase history.");
            }
            catch (Exception ex) { throw ex; }
        }


        public StoreDTO GetStoreDTO()
        {
            try
            {
                return new StoreDTO(this); // "copy constructor"
            }
            catch (Exception ex) { throw ex; }
        }


        public List<ItemDTO> GetItems()
        {
            try
            {
                List<ItemDTO> productList = new List<ItemDTO>();
                foreach (String s in allProducts.Values)
                {
                    productList.Add(AcquireProduct(s).GetProductDTO());
                    ReleaseProduct(s);
                }
                return productList;

            }
            catch (Exception ex) { throw ex; }
        }


        public void RemoveStore(string userID)
        {
            try
            {
                if (this.founderID != userID) // ADD - maket manager permission validation
                    throw new Exception("Only store founder or Market Manager can remove a store.");
                this.storeRepo.close_store_temporary(this.Store_ID);
                this.employees.removeStore(this.Store_ID);
                this.temporaryClosed = true;
            }
            catch (Exception ex) { throw ex; }

        }

        public void ReopenStore(string userID)
        {
            try
            {
                if (this.founderID != userID) // ADD - maket manager permission validation
                    throw new Exception("Only store founder or Market Manager can reopen a store.");
                this.storeRepo.re_open_closed_temporary_store(userID, this.Store_ID);
                //this.employeesReopenStore(this.Store_ID);                
                this.temporaryClosed = false;
            }
            catch (Exception ex) { throw ex; }

        }


        private static object CalculatePriceLock = new object();
        public double CalculatePrice(List<ItemDTO> productsToCalculate)
        {
            lock (CalculatePriceLock)
            {
                try
                {
                    double productSalePrice = 0;
                    double storeSalePrice = 0;
                    int quantity = 0;
                    foreach (ItemDTO item in productsToCalculate)
                    {
                        productSalePrice += AcquireProduct(item.GetID()).CalculatePrice(item.GetQuantity());
                        quantity += item.GetQuantity();
                        ReleaseProduct(item.GetID());
                    }
                    storeSalePrice = productSalePrice;
                    foreach(Purchase_Policy p in this.storePolicies.Values)
                        storeSalePrice -= Math.Max(0, p.ApplyPolicy(productSalePrice, quantity));
                    return storeSalePrice;
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
                    foreach (ItemDTO item in productsToPurchase)
                        if (!AcquireProduct(item.GetID()).prePurchase(item.GetQuantity()))
                        {
                            cannotPurchase.Concat(item.GetID().Concat(";"));
                            ReleaseProduct(item.GetID());
                        }

                    if (!cannotPurchase.Equals("")) throw new Exception(cannotPurchase);
                    else
                        foreach (ItemDTO item in productsToPurchase)
                        {
                            AcquireProduct(item.GetID()).Purchase(item.GetQuantity());
                            ReleaseProduct(item.GetID());
                            this.storeRepo.record_purchase(this, item); // for purchase history
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
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    if (this.storePolicies.TryAdd(newPolicy.GetID(), newPolicy))
                        Save();
                    else
                        throw new Exception("Policy already exists.");
                }
                else
                    throw new Exception("You don't have permissions to add new policy.");
            }
            catch (Exception e) { throw e; }
        }

        public void RemoveStorePurchasePolicy(string userID, String policyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    if (this.storePolicies.TryRemove(policyID, out _))
                        Save();
                    else throw new Exception("No such policy.");
                }
                else throw new Exception("You have no permission to remove store policy.");
            }
            catch (Exception e) { throw e; }
        }

        public void AddStorePurchaseStrategy(string userID, Purchase_Strategy newStrategy)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    if (this.storeStrategies.TryAdd(newStrategy.GetID(), newStrategy))
                        Save();
                    else throw new Exception("Strategy already exists.");
                }
                else throw new Exception("You have no permission to add strategy.");
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveStorePurchaseStrategy(string userID, String strategyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    if (this.storeStrategies.TryRemove(strategyID, out _))
                        Save();
                    else
                        throw new Exception("No such strategy in this store.");
                }
                else
                    throw new Exception("You don't have a permission to remove strategies.");
            }
            catch (Exception e) { throw e; }
        }


        // call me every time data changes
        private void Save()
        {
            try
            {
                this.storeRepo.saveStore(this);
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
                return products.GetOrAdd(productID, (k) => new Lazy<Product>(() =>
                {
                    if (!this.allProducts.Keys.Contains(productID)) throw new Exception("No such product in this store.");
                    productUsage.AddOrUpdate(k, 1, (k, val) => val + 1);
                    return storeRepo.getProduct(k);
                }).Value); // valueFactory could be calle multiple timnes so Lazy instance may be created multiple times also, but only one will actually be used
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
                    if (((ICollection<KeyValuePair<string, int>>)productUsage).Remove(new KeyValuePair<string, int>(productID, 1)))
                        products.TryRemove(productID, out _);
                    else
                        productUsage.TryUpdate(productID, (productUsage[productID] - 1), productUsage[productID]);
                }
                catch (Exception e) { throw e; }
            }
        }

        private static object AddProductLock = new object();
        public ItemDTO AddProduct(string userID, List<string> productProperties)
        {
            lock (AddProductLock)
            {
                try
                {
                    if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                    {
                        Product newProduct = new Product(productProperties, this.Store_ID, this.productDefaultPolicies, this.productDefaultStrategies);
                        this.storeRepo.AddProduct(this.Store_ID, this.founderID, newProduct, 0);
                        this.allProducts.TryAdd(newProduct.Product_ID, newProduct.Product_ID);
                        Save();
                        return newProduct.GetProductDTO();
                    }
                    else
                        throw new Exception("You dont have a permission to manage store stock.");
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
                    this.storeRepo.RemoveProduct(this.Store_ID, this.founderID, AcquireProduct(product_id));
                    ReleaseProduct(product_id);
                    this.products.TryRemove(product_id, out _);
                    this.productUsage.TryRemove(product_id, out _);
                    this.allProducts.TryRemove(product_id, out _);
                    Save();
                }
                else throw new Exception("You don't have a permission to manage store stock.");
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
            }
            catch (Exception ex) { throw ex; }
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
        public void ChangeProductRating(string userID, string productID, double rating)
        {
            try
            {
                if (this.employees.isMarketManager(userID))
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

        public void ChangeProductTimesBought(string userID, string productID, int times) // only market manager can do
        {
            try
            {
                if (this.employees.isMarketManager(userID)) // change later after market manager permission enum added
                {
                    AcquireProduct(productID).SetTimesBought(times);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public void ChangeProductCategory(string userID, string productID, Category category)
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

        public void ChangeProductDimenssions(string userID, string productID, double[] dims)
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
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! validate Policy perm
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
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! validate Policy perm
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
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! validate Policy perm
                {
                    AcquireProduct(productID).AddPurchaseStrategy(newStrategy);
                    ReleaseProduct(productID);
                }
                else
                    throw new Exception("You don't have a permission to add strategy.");
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveProductPurchaseStrategy(string userID, string productID, String strategyID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! validate Policy perm
                {
                    AcquireProduct(productID).RemovePurchaseStrategy(strategyID);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }

        public Boolean is_closed_temporary()
        {
            return this.temporaryClosed;
        }


        public void SetTestEmployees(Employees emp)
        {
            this.employees = emp;
        }


        // ===================== END of Product operations =========================
        // =========================================================================




        // ================================================================
        // ======================== TODO ==================================



        // ======================== END of TODO ==================================
        // =======================================================================


    }
}

