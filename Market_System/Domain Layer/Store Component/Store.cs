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
        public ConcurrentBag<string> Products { get; private set; } //<product_id, quantity>
        private ConcurrentBag<string> defaultPolicies; // passed to every new added product
        private ConcurrentBag<string> defaultStrategies; // passed to every new added product
        private Employees employees;
        public String founderID { get; private set; } //founder's userID
        private StoreRepo storeRepo;

        public Store(string founderID, string storeID) // builder for a new store - initialize all fields with default values
        {
            this.Store_ID = storeID;
            this.founderID = founderID;
            this.storeRepo = StoreRepo.GetInstance();
            this.employees = Employees.GetInstance();
            Retreive_Products();
        }

        public string ChangeName(string userID, string newName)
        {
            try
            {
                // validate PERMISSION here
                this.Name = newName;
                storeRepo.UpdateStoreName(Store_ID, newName);
            } catch (Exception ex) { throw ex; }
        }

        private void Retreive_Products()
        {
            try
            {
                ConcurrentBag<string> products = new ConcurrentBag<string>();
                foreach(Product p in this.storeRepo.GetProducts(this.Store_ID))
                {
                    products.Add(p.Product_ID);
                }
                this.Products = products;

            } catch (Exception ex) { throw ex; }
        }

        public void Add_Product(string userID, List<string> productProperties)
        {
            try
            {
                // if userID has PERMISSION
                Product newProduct = new Product(productProperties); // separate - retreive all the properties from the list and pass to builder
                this.storeRepo.AddProduct(newProduct);
                Retreive_Products();
            } catch (Exception ex) { throw ex; }   
            
        }

        public void Remove_Product(string userID, string product_id)
        {
            try
            {
                // if userID has PERMISSION
                storeRepo.RemoveProduct(product_id);
                Retreive_Products();

            } catch (Exception ex) { throw ex; }    
        }

        public void Add_New_Owner(string userID, string newOwnerID)
        {
            try
            {
                // if userID has PERMISSION and newOwnerID
                this.employees.AddNewOwner(Store_ID, userID, newOwnerID);
            }
            catch (Exception ex) { throw ex; }
        }

        public void Add_New_Manager(string userID)
        {
            try
            {
                // if userID has PERMISSION
                employees.AddNewManager(Store_ID, userID);
            } catch(Exception ex) { throw ex; }
        }

        public double CalculatePrice(List<ItemDTO> productsToCalculate)
        {
            try
            {
                double price = 0;
                foreach(ItemDTO item in productsToCalculate) {
                    Product p = storeRepo.GetProduct(item.GetID());
                    price += p.CalculatePrice(item.GetQuantity());
                }
                return price;
            } catch (Exception ex) { throw ex; }    
        }

        public void Purchase(string userID, List<ItemDTO> productsToPurchase)
        {
            String cannotPurchase = ""; // will look like "ite#1ID;item#2ID;item#3ID;..."
            try
            {
                foreach (ItemDTO item in productsToCalculate)
                {
                    if (!storeRepo.GetProduct(item.GetID()).Purchase(userID))
                        cannotPurchase.Concat(item.GetID().Concat(";"));
                }
                if(!cannotPurchase.Equals("")) { throw new Exception(cannotPurchase)};
            }
            catch (Exception ex) { throw new Exception(cannotPurchase, ex); }
        }

        public Boolean ReserveProduct(ItemDTO reserveProduct)
        {
            try
            {
                return storeRepo.GetProduct(reserveProduct.GetID()).Reserve(reserveProduct.GetQuantity());
            } catch (Exception ex) { throw ex; }    
        }

        public Boolean ReleaseProduct(ItemDTO reservedProduct)
        {
            try
            {
                return storeRepo.GetProduct(reservedProduct.GetID()).Release(reservedProduct.GetQuantity());
            }
            catch (Exception ex) { throw ex; }
        }

        public void EditProduct(string userID, string productID, List<strin> editedDetails) 
        {
            // has to be separated into sub-editions: editWeight(), editQuantity(), etc on higher level
            throw new NotImplementedException();
        }

        public void GetManagersOfTheStore(string userID)
        {
            try
            {
                // validate PERMISSION here
                this.employees.GetManagers(this.Store_ID);
            } catch (Exception ex) { throw ex; }    
        }

        public List<string> GetPurchaseHistoryOfTheStore(string userID)
        {
            try
            {
                // validate PERMSSION here
                this.storeRepo.GetPurchaseHistory(this.Store_ID);
            } catch (Exception ex) { throw ex; }    
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



        // ========Methods ToDo==========:
        // passing a data for store representation (including what details?)
        // passing a data for store content representation- what products availble for a buyer
        // *maby should an additional store content view, a seperate view for a manager of the store
        // (search)-get products by name: getting all products with same name *or similar name
        // (search)-get products by catagory: getting all products from that catagory

        // ======more ideas=====
        /*
         *  .חיפוש מוצרים ללא התמקדות בחנות ספציפית, לפי שם המוצר, קטגוריה או מילות מפתח.
         *  ניתן לסנן את התוצאות בהתאם למאפיינים כגון: טווח מחירים, דירוג המוצר, קטגוריה
         * דירוג החנות וכד'.
         */
    }
}