using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System.Web.WebSockets;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.UserComponent;
using Market_System.DAL;
using System.EnterpriseServices;

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
        public string founderID { get; private set; } //founder's userID
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
                    this.storePolicies.TryAdd(p.PolicyID, p);
            if (strategies != null)
                foreach (Purchase_Strategy p in strategies)
                    this.storeStrategies.TryAdd(p.StrategyID, p);

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

        internal string get_product_name_from_prodcut_id(string product_id)
        {
            return products[product_id].Name;
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
                    //the new owner is not already an owner, and isn't a manager - nor he is the founder of this store!
                    if ((this.employees.isFounder(userID, this.Store_ID) || this.employees.isOwner(userID, this.Store_ID)) &&
                        !(this.employees.isOwner(newOwnerID, this.Store_ID)) && !(this.employees.isManager(newOwnerID, this.Store_ID))
                        && !(this.employees.isFounder(newOwnerID, this.Store_ID)))
                    {
                        this.employees.AddNewOwnerEmpPermissions(userID, newOwnerID, this.Store_ID);
                    }

                    //the new owner is not already an owner, and is a manager -> we need to remove him as a manager first!
                    else if ((this.employees.isFounder(userID, this.Store_ID) || this.employees.isOwner(userID, this.Store_ID)) &&
                        !(this.employees.isOwner(newOwnerID, this.Store_ID)) && (this.employees.isManager(newOwnerID, this.Store_ID))
                        && !(this.employees.isFounder(newOwnerID, this.Store_ID)))
                    {
                        this.employees.removeEmployee(newOwnerID, this.Store_ID);
                        this.employees.AddNewOwnerEmpPermissions(userID, newOwnerID, this.Store_ID);
                    }

                    else
                        throw new Exception("Cannot assign new owner: you are not an owner in this store or employee is already an owner in this store.");
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public void Remove_Store_Owner(string userID, string other_Owner_ID)
        {
            lock (EmployementLock)
            {
                try
                {
                    Employee emp = null;
                    foreach (Employee tempEmp in employees.getStoreEmployees(this.Store_ID))
                    {
                        if (tempEmp.UserID.Equals(other_Owner_ID))
                        {
                            emp = tempEmp;
                        }
                    }

                    if ((this.employees.isFounder(userID, this.Store_ID) || this.employees.isOwner(userID, this.Store_ID)) && (emp != null) && (emp.OwnerAssignner != null) && (emp.OwnerAssignner.Equals(userID)))
                    {
                        this.employees.removeEmployee(other_Owner_ID, this.Store_ID);
                    }
                    else
                        throw new Exception("Cannot remove owner: you are not an owner/assignneer of other owner in this store or the employee isn't an owner in the store");
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public bool check_if_can_remove_or_add_permessions(string userID)
        {
            lock (EmployementLock)
            {
                try
                {
                    if ((this.employees.isFounder(userID, this.Store_ID)) || ((this.employees.isOwner(userID, this.Store_ID)) ))
                        return true;
                    else
                        return false;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public bool check_if_can_close_store(string userID)
        {
            lock (EmployementLock)
            {
                try
                {
                    if ((this.employees.isFounder(userID, this.Store_ID)))
                        return true;
                    else
                        return false;
                }
                catch (Exception ex) { throw ex; }
            }
        }


        public bool check_if_can_assign_manager_or_owner(string userid)
        {
            lock (EmployementLock)
            {
                try
                {
                    if ((this.employees.isFounder(userid, this.Store_ID) || this.employees.isOwner(userid, this.Store_ID)))
                    {
                        return true;
                    }
                    else
                        return false;
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
                    if ((this.employees.isFounder(userID, this.Store_ID) || this.employees.isOwner(userID, this.Store_ID)) &&
                        !this.employees.isManager(newManagerID, this.Store_ID) && !this.employees.isOwner(newManagerID, this.Store_ID)
                        && !(this.employees.isFounder(newManagerID, this.Store_ID)))
                    {
                        this.employees.AddNewManagerEmpPermissions(userID, newManagerID, Store_ID, new List<Permission>() { Permission.STOCK });
                    }
                    else
                        throw new Exception("Cannot assign new manager: you are not an owner or this employee is already a manager or owner in this store.");
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

        internal string GetUsersRole(string other_User_ID)
        {
            foreach (Employee emp in employees.getStoreEmployees(Store_ID))
            {
                if (emp.UserID.Equals(other_User_ID))
                {
                    return emp.Role.ToString();
                }
            }
            return null;
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
                if (this.founderID != userID || !this.is_closed_temporary()) // ADD - maket manager permission validation
                    throw new Exception("Only store founder or Market Manager can reopen a store OR store isn't closed.");
                this.storeRepo.re_open_closed_temporary_store(userID, this.Store_ID);
                this.employees.ReopenStore(this.Store_ID);
                this.temporaryClosed = false;
            }
            catch (Exception ex) { throw ex; }

        }

        private static object CalculatePriceLock = new object();
        public double CalculatePrice(string userID, List<ItemDTO> calculateUS)
        {
            lock (CalculatePriceLock)
            {
                try
                {
                    // bidding prices if approved
                    double bidsTotalPrice = 0.0;
                    List<BidDTO> storeBids = this.GetStoreBids(this.founderID);
                    List<ItemDTO> productsToCalculate = new List<ItemDTO>();
                    foreach (ItemDTO item in calculateUS)
                    {
                        BidDTO currBid;
                        if (( currBid = storeBids.SingleOrDefault(b => b.BidID == userID + "_" + item.GetID() + "_bid" && (b.ApprovedByStore || b.ApprovedByUser))) != null)
                            bidsTotalPrice += currBid.NewPrice * currBid.Quantity;
                        else
                            productsToCalculate.Add(item);
                    }


                    // no-bidded products
                    double productSalePrice = 0;
                    int quantity = 0;
                    List<ItemDTO> saledProducts = new List<ItemDTO>();
                    bool maxPolicy = false;
                    foreach(Purchase_Policy pp in this.storePolicies.Values)
                    {
                        if (pp is MaximumPolicy)
                        {
                            saledProducts = pp.ApplyPolicy(productsToCalculate);
                            maxPolicy = true;
                        }
                    }

                    if (!maxPolicy) // if Maximum Policy was applied other policies cannot be applied
                    {
                        // Apply each Product Sale:
                        foreach (ItemDTO item in productsToCalculate)
                        {
                            productSalePrice = AcquireProduct(item.GetID()).CalculatePrice(item);
                            item.SetPrice(productSalePrice / item.GetQuantity());
                            saledProducts.Add(item);
                            quantity += item.GetQuantity();
                            ReleaseProduct(item.GetID());
                        }

                        // Apply Store Policy:
                        foreach (Purchase_Policy p in this.storePolicies.Values)
                            saledProducts = p.ApplyPolicy(saledProducts);
                    }

                    return bidsTotalPrice + saledProducts.Aggregate(0.0, (acc, x) => acc += x.Price * x.GetQuantity());
                }
                catch (Exception ex) { throw ex; }
            }
        }





        private static object PurchaseLock = new object();
        public void Purchase(string userID, List<ItemDTO> productsToPurchaseFewInfo)
        {
            lock (PurchaseLock)
            {
                List<BidDTO> storeBids = this.GetStoreBids(this.founderID);
                List<ItemDTO> productsIncludingBids = productsToPurchaseFewInfo.Select(i => {
                                                                                            ItemDTO newItem = AcquireProduct(i.GetID()).GetProductDTO();
                                                                                            ReleaseProduct(i.GetID());
                                                                                            newItem.SetQuantity(i.GetQuantity());
                                                                                            return newItem;                                                                                          
                                                                                        }).ToList();

                List<ItemDTO> productsToPurchase = new List<ItemDTO>();                                            
                string initErrorMSG = "Cannot purchase: ";
                String cannotPurchase = initErrorMSG; // will look like "item#1ID_Name;item#2ID_Name;item#3IDName;..."

                // purchasing bid products
                foreach (ItemDTO item in productsIncludingBids)
                {
                    BidDTO currBid;
                    if ((currBid = storeBids.SingleOrDefault(b => b.BidID == userID + "_" + item.GetID() + "_bid" && (b.ApprovedByStore == true || b.ApprovedByUser == true))) != null)
                        this.BidPurchase(userID, currBid);
                    else
                        productsToPurchase.Add(item);
                }


                // purchasing no-bidded products
                try
                {
                    foreach (Purchase_Strategy ps in this.storeStrategies.Values)
                    {
                        if (!ps.Validate(productsToPurchase, userID))
                            throw new Exception("Restrictions violated: " + ps.Description);
                    }

                    foreach (ItemDTO item in productsToPurchase)
                        if (!AcquireProduct(item.GetID()).prePurchase(userID, item))
                        {
                            cannotPurchase.Concat(item.GetID().Concat(";"));
                            ReleaseProduct(item.GetID());
                        }

                    if (!cannotPurchase.Equals(initErrorMSG)) throw new Exception(cannotPurchase + " - not enough in stock.");
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

        public void AddStorePurchasePolicy(string userID, List<string> newPolicyProps) // string polID, string polName, double salePercentage, string description, string category, Statement formula
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    Purchase_Policy newPolicy = null;
                    switch (newPolicyProps[0])
                    {
                        case "Category":  // type, string polName, double salePercentage, string description, string category, Statement formula
                            newPolicy = new CategoryPolicy(this.Store_ID + "CategoryPolicyID" + newPolicyProps[1], newPolicyProps[1], Double.Parse(newPolicyProps[2]), newPolicyProps[3], newPolicyProps[4], newPolicyProps[5]);
                            break;
                        case "Product":  // type, string polName, double salePercentage, string description, Statement formula, string productID
                            newPolicy = new ProductPolicy(this.Store_ID + "ProductPolicyID" + newPolicyProps[1], newPolicyProps[1], Double.Parse(newPolicyProps[2]), newPolicyProps[3], newPolicyProps[4], newPolicyProps[5]);
                            break;
                        case "Store": // type, string polName, double salePercentage, string description, string storeID, String formula
                            newPolicy = new StorePolicy(this.Store_ID + "StorePolicyID" + newPolicyProps[1], newPolicyProps[1], Double.Parse(newPolicyProps[2]), newPolicyProps[3], newPolicyProps[4], newPolicyProps[5]);
                            break;
                    }

                    if (this.storePolicies.TryAdd(newPolicy.PolicyID, newPolicy))
                        Save();
                    else
                        throw new Exception("Policy already exists.");
                }
                else
                    throw new Exception("You don't have permissions to add new policy.");
            }
            catch (Exception e) { throw e; }
        }

        public void AddStorePurchasePolicy(string userID, Purchase_Policy newPolicy) // for tests only
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {

                    if (this.storePolicies.TryAdd(newPolicy.PolicyID, newPolicy))
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

        public void AddStorePurchaseStrategy(string userID, List<string> strategyPopsWithoutID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    Purchase_Strategy newStrategy = new Purchase_Strategy(this.Store_ID+"StoreStrategyID" + strategyPopsWithoutID[0], strategyPopsWithoutID[0], strategyPopsWithoutID[1], strategyPopsWithoutID[2]);
                    if (this.storeStrategies.TryAdd(newStrategy.StrategyID, newStrategy))
                        Save();
                    else throw new Exception("Strategy already exists.");
                }
                else throw new Exception("You have no permission to add strategy.");
            }
            catch (Exception e) { throw e; }
        }

        public void AddStorePurchaseStrategy(string userID, Purchase_Strategy newStrategy) // for tests only
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.Policy))
                {
                    if (this.storeStrategies.TryAdd(newStrategy.StrategyID, newStrategy))
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


        public double GetStoreProfitForDate(string userID, string date_as_dd_MM_yyyy)
        {
            try
            {
                if (this.employees.isOwner(userID, this.Store_ID) || EmployeeRepo.GetInstance().isMarketManager(userID))
                    return this.storeRepo.GetStoreProfitForDate(this.Store_ID, date_as_dd_MM_yyyy);
                else
                    throw new Exception("You don't have a permission to view profit.");
            }
            catch (Exception e) { throw e; }
        }







        // =========================================================================================
        // ========================================== BID ==========================================



        public BidDTO PlaceBid(string userID, string productID, double newPrice, int quantity)
        {
            try
            {
                BidDTO ret;
                ret = this.storeRepo.PlaceBid(this.Store_ID, userID, productID, newPrice, quantity);                
                AcquireProduct(productID).Reserve(quantity);                
                ReleaseProduct(productID);
                return ret;
            }
            catch (Exception e) { throw e; }
        }

        public bool ApproveBid(string userID, string bidID)
        {
            try
            {
                if (this.employees.isOwner(userID, this.Store_ID) || bidID.Contains(userID))
                    return this.storeRepo.ApproveBid(this.Store_ID, userID, bidID);
                throw new Exception("You cannot view this info.");
            }
            catch (Exception e) { throw e; }
        }


        public BidDTO GetBid(string userID, string bidID)
        {
            try
            {
                if (this.employees.isOwner(userID, this.Store_ID) || this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK) || bidID.Contains(userID))
                    return this.storeRepo.GetBid(bidID);
                throw new Exception("You cannot view this information.");
            }
            catch (Exception e) { throw e; }
        }



        public void CounterBid(string userID, string bidID, double counterPrice)
        {
            try
            {
                if (this.employees.isOwner(userID, this.Store_ID) || this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK) || bidID.Contains(userID))
                    this.storeRepo.CounterBid(bidID, counterPrice);
            }
            catch (Exception e) { throw e; }
        }


        public void RemoveBid(string userID, string bidID)
        {
            try
            {
                BidDTO bid = GetBid(userID, bidID);
                if (this.employees.isOwner(userID, this.Store_ID) || this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK) || bidID.Contains(userID))
                {
                    this.storeRepo.RemoveBid(bidID);
                    AcquireProduct(bid.ProductID).LetGoProduct(bid.Quantity); 
                    ReleaseProduct(bid.ProductID);
                }
            }
            catch (Exception e) { throw e; }
        }


        public void BidPurchase(string userID, BidDTO bid)
        {
            try
            {
                AcquireProduct(bid.ProductID).BidPurchase(userID, bid);
                ReleaseProduct(bid.ProductID);
            }
            catch (Exception e) { throw e; }
        }


        public List<BidDTO> GetStoreBids(string userID)
        {
            try
            {
                if (this.employees.isOwner(userID, this.Store_ID) || this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                    return this.storeRepo.GetStoreBids(this.Store_ID);
                throw new Exception("You cannot view this information.");
            }
            catch (Exception e) { throw e; }   
                          
        }





        // ==============================================================================================================================
        // ==============================================================================================================================








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

        public bool check_if_can_show_infos(string userID)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.INFO))
                {

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
        }
         

        private static object check_if_can_manage_stock_lock = new object();
        public bool check_if_can_manage_stock(string userID)
        {
            lock (check_if_can_manage_stock_lock)
            {
                try
                {
                    if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK))
                    {

                        return true;
                    }
                    else
                        return false;
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

        public List<string> get_all_comments_of_product(string productID)
        {
            try
            {
                List<string> list_of_comments = AcquireProduct(productID).get_all_comments_of_product();

                ReleaseProduct(productID);
                return list_of_comments;
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

        internal List<ItemDTO> GetItems_not_zero_quantity()
        {
            try
            {
                List<ItemDTO> productList = new List<ItemDTO>();
                ItemDTO current_item;
                foreach (String s in allProducts.Values)
                {
                    current_item = AcquireProduct(s).GetProductDTO();
                    if((current_item.GetQuantity() - current_item.GetReservedQuantity()) > 0)
                    {
                        productList.Add(current_item);
                        ReleaseProduct(s);
                    }
                }
                return productList;

            }
            catch (Exception ex) { throw ex; }
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

        public bool Check_If_Member_Only(string member_ID)
        {
            foreach(Employee emp in employees.getStoreEmployees(Store_ID))
            {
                if (emp.UserID.Equals(member_ID))
                {
                    return false;
                }
            }
            return true;
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


        public void AddProductPurchasePolicy(string userID, string productID, List<string> newPolicyProps)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! validate Policy perm
                {
                    AcquireProduct(productID).AddPurchasePolicy(newPolicyProps);
                    ReleaseProduct(productID);
                }
            }
            catch (Exception e) { throw e; }
        }


        public void AddProductPurchasePolicy(string userID, string productID, Purchase_Policy newPolicy) // for tests only
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

        public void AddProductPurchaseStrategy(string userID, string productID, List<string> newStrategyProperties)
        {
            try
            {
                if (this.employees.confirmPermission(userID, this.Store_ID, Permission.STOCK)) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! validate Policy perm
                {
                    AcquireProduct(productID).AddPurchaseStrategy(newStrategyProperties);
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


        public ConcurrentDictionary<string, Product> get_products()
        {
            return products;
        }



        // ===================== END of Product operations =========================
        // =========================================================================




        // ================================================================
        // ======================== TODO ==================================



        // ======================== END of TODO ==================================
        // =======================================================================


    }
}

