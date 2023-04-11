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
        private string SessionID;

        public Store_Service_Controller(string sessionID)
        {
            this.Market = DomainLayer.MarketSystem.GetInstance();
            this.SessionID = sessionID;
        }

        // ====================================================================
        // ====================== General class methods ===============================


        // ====================== END of General class methods ===============================
        // ===================================================================================



        // ====================================================================
        // ====================== Store methods ===============================

        public Response ChangeStoreName( string storeID, string newName)
        {
            try
            {                
                this.Market.ChangeStoreName(this.SessionID, storeID, newName); // string storeID, string newName - add to MarketSystem !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Store name changed.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetStore( string storeID)
        {
            try
            {
                ItemDTO ret = this.Market.GetStore(this.SessionID, storeID);
                return Response<ItemDTO>.FromValue(ret);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddNewStore( List<string> newStoreDetails)
        {
            try
            {
                this.Market.Add_New_Store(this.SessionID, this.SessionID); // change method in MarketSystem - cannot receive StoreID yet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Store was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStore( string storeID)
        {
            try
            {
                this.Market.RemoveStore(this.SessionID, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Store was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetPurchaseHistoryOfTheStore(string storeID)
        {
            try
            {
                List<string> history = this.Market.GetStorePurchaseHistory(this.SessionID, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(history);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response TransferFoundership( string storeID, string newFounderID) // change founder
        {
            try
            {
                this.Market.TransferFoundership(this.SessionID, storeID, newFounderID);
                return new Response("Founder was changed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddStorePurchasePolicy(string storeID, Purchase_Policy newPolicy, List<string> newPolicyProperties) // newPolicy = null OR newPolicyProperties = null
        {
            try
            {
                this.Market.AddStorePurchasePolicy(this.SessionID, storeID, newPolicy, newPolicyProperties);
                return new Response("Policy was added successfully.\"");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStorePurchasePolicy(string storeID, String policyID)
        {
            try
            {
                this.Market.RemoveStorePurchasePolicy(this.SessionID, storeID, policyID);
                return new Response("Policy was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddStorePurchaseStrategy( string storeID, Purchase_Strategy newStrategy, List<string> newStrategyProperties) // newPolicy = null OR newPolicyProperties = null)
        {
            try
            {
                this.Market.AddStorePurchaseStrategy(this.SessionID, storeID, newStrategy, newStrategyProperties);
                return new Response("Strategy was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStorePurchaseStrategy( string storeID, String strategyID)
        {
            try
            {
                this.Market.RemoveStorePurchaseStrategy(this.SessionID, storeID, strategyID);
                return new Response("Strategy was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        // ====================== END of Store methods ===============================
        // ===========================================================================





        // =======================================================================
        // ====================== EMployee methods ===============================
        public Response AddEmployeePermission( string storeID, string employeeID, string newPerm)
        {
            try
            {
                this.Market.AddEmployeePermission(this.SessionID, storeID, employeeID, (Permission)Enum.Parse(typeof(Permission), newPerm));
                return new Response("Permission was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }

        }

        public Response RemoveEmployeePermission( string storeID, string employeeID, string permToRemove)
        {
            try
            {
                this.Market.RemoveEmployeePermission(this.SessionID, storeID, employeeID, (Permission)Enum.Parse(typeof(Permission), permToRemove));
                return new Response("Permission was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AssignNewOwner( string storeID, string newOwnerID)
        {
            try
            {
                this.Market.Assign_New_Owner(this.SessionID, newOwnerID, storeID);
                return new Response("New owner was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AssignNewManager( string storeID, string newManagerID)
        {
            try
            {
                this.Market.Assign_New_Manager(this.SessionID, newManagerID, storeID);
                return new Response("New manager was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetManagersOfTheStore( string storeID)
        {
            try
            {
                List<string> managers = this.Market.GetStoreManagers(this.SessionID, storeID);
                return Response<List<string>>.FromValue(managers);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetOwnersOfTheStore( string storeID)
        {
            try
            {
                List<string> owners = this.Market.GetOwnersOfTheStore(this.SessionID, storeID);
                return Response<List<string>>.FromValue(owners);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ManageEmployeePermissions( string storeID, string employeeID, List<string> additionalPerms) // update only for store manager - exchanges permissions
        {
            try
            {                
                List<Permission> permList = additionalPerms.Select(x => (Permission)Enum.Parse(typeof(Permission), x)).ToList();
                this.Market.ManageEmployeePermissions(this.SessionID, storeID, employeeID, permList);
                return new Response("New manager's permissions were added successfully.");
            }
            catch (Exception ex)
            {
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
                List<ItemDTO> products = this.Market.GetProductsFromStore(this.SessionID, storeID);
                return Response<List<ItemDTO>>.FromValue(products);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductToStore(string storeID, string usertID, List<String> productProperties)
        {
            try
            {
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              |
                //                              V
                //                              STOPPED HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductFromStore(string storeID,  string productID, List<string> productProperties)
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

        public Response AddProductComment( string productID, string comment, double rating)
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

        public Response ChangeProductName( string productID, string name)
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

        public Response ChangeProductDescription( string productID, string description)
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

        public Response ChangeProductPrice( string productID, double price)
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

        public Response ChangeProductRating( string productID, double rating)
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

        public Response ChangeProductQuantity( string productID, int quantity)
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

        public Response ChangeProductWeight( string productID, double weight)
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

        public Response ChangeProductSale( string productID, double sale)
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

        public Response ChangeProductTimesBought( string productID, int times)
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

        public Response ChangeProductProductCategory( string productID, Category category)
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

        public Response ChangeProductDimenssions( string productID, Array<double> dims)
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

        public Response AddProductPurchasePolicy( string storeID, string productID, Purchase_Policy newPolicy)
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

        public Response RemoveProductPurchasePolicy( string storeID, string productID, String policyID)
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

        public Response AddProductPurchaseStrategy( string storeID, string productID, Purchase_Strategy newStrategy)
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

        public Response RemoveProductPurchaseStrategy( string storeID, string productID, String strategyID)
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



