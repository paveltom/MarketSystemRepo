using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer;
using Market_System.ServiceLayer;

namespace Market_System.ServiceLayer
{
    public class Store_Service_Controller
    {
        private DomainLayer.MarketSystem Market;
        private Logger logger;

        public Store_Service_Controller()
        {
            this.Market = DomainLayer.MarketSystem.GetInstance();
        }

        // ====================================================================
        // ====================== General class methods ===============================

        public Response Purchase(string userID, List<string> productsIDS)
        {
            try
            {
                // LOG the action
                throw new NotImplementedException();
            } 
            catch (Exception ex) 
            { 
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }


        // ====================== END of General class methods ===============================
        // ===================================================================================



        // ====================================================================
        // ====================== Store methods ===============================

        public Response ChangeStoreName(string userID, string storeID, string newName)
        {
            try
            {                
                this.Market.ChangeStoreName(userID, storeID, newName); //string userID, string storeID, string newName
                // LOG the action
                return new Response("Store name changed.");
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetStore(string userID, string storeID)
        {
            try
            {
                ItemDTO ret = this.Market.GetStore(userID, storeID);
                // LOG the action
                return Response<ItemDTO>.FromValue(ret);
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddNewStore(string userID, string storeID, List<string> newStoreDetails)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStore(string userID, string storeID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetPurchaseHistoryOfTheStore(string userID, string storeID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response TransferFoundership(string userID, string storeID, string newFounderID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddStorePurchasePolicy(string userID, string storeID, Purchase_Policy newPolicy)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStorePurchasePolicy(string userID, string storeID, String policyID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddStorePurchaseStrategy(string userID, string storeID, Purchase_Strategy newStrategy)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStorePurchaseStrategy(string userID, string storeID, String strategyID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        // ====================== END of Store methods ===============================
        // ===========================================================================





        // =======================================================================
        // ====================== EMployee methods ===============================
        public Response AddEmployeePermission(string userID, string storeID, string employeeID, Permission newP)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }

        }

        public Response RemoveEmployeePermission(string userID, string storeID, string employeeID, Permission permToRemove)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AssignNewOwner(string userID, string storeID, string newOwnerID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AssignNewManager(string userID, string storeID, string newManagerID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetManagersOfTheStore(string userID, string storeID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetOwnersOfTheStore(string userID, string storeID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ManageEmployeePermissions(string userID, string storeID, string employeeID, List<Permission> perms)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        // ====================== END of Employee methods ===============================
        // ==============================================================================





        // ======================================================================
        // ====================== Product methods ===============================

        public Response GetProductsFromStore(string storeID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductToStore(string storeID, string usertID, List<String> productProperties)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductFromStore(string storeID, string userID, string productID, List<string> productProperties)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductComment(string userID, string productID, string comment, double rating)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ReserveProduct(ItemDTO reservedProduct)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response LetGoProduct(ItemDTO reservedProduct)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response SearchProductByKeyword(string keyword)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response SearchProductByName(string name)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response SearchProductByCategory(Category category)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductName(string userID, string productID, string name)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductDescription(string userID, string productID, string description)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductPrice(string userID, string productID, double price)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductRating(string userID, string productID, double rating)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductQuantity(string userID, string productID, int quantity)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductWeight(string userID, string productID, double weight)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductSale(string userID, string productID, double sale)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductTimesBought(string userID, string productID, int times)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductProductCategory(string userID, string productID, Category category)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductDimenssions(string userID, string productID, Array<double> dims)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductPurchasePolicy(string userID, string storeID, string productID, Purchase_Policy newPolicy)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductPurchasePolicy(string userID, string storeID, string productID, String policyID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductPurchaseStrategy(string userID, string storeID, string productID, Purchase_Strategy newStrategy)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductPurchaseStrategy(string userID, string storeID, string productID, String strategyID)
        {
            try
            {
                // LOG the action
            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        }




        // ======================================================
        // ======================== TODO ========================


        public Response apply_purchase_policy() { // I dont quite understand what its supposed to do....
            try
            {

            }
            catch (Exception ex)
            {
                // LOG the error
                return new Response("ERROR: " + ex.Message);
            }
        } 


        internal void destroy()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!TODO
        }

        // ======================== END of TODO ========================
        // =============================================================

    }
}



