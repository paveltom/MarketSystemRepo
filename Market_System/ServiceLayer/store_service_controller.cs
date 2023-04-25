using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using System.Xml.Linq;

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

        public Response ChangeStoreName(string storeID, string newName)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeStoreName(SessionID, storeID, newName); // string storeID, string newName - add to MarketSystem !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Store name changed.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetStore(string storeID)
        {
            try
            {
                StoreDTO ret = this.Market.GetStore( storeID);
                return Response<StoreDTO>.FromValue(ret);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddNewStore(List<string> newStoreDetails)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                StoreDTO added = this.Market.Add_New_Store(this.SessionID, newStoreDetails); // change method in MarketSystem - cannot receive StoreID yet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<StoreDTO>.FromValue(added);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response close_store_temporary(string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.close_store_temporary(SessionID, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Store was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response<string> GetPurchaseHistoryOfTheStore(string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                string history = this.Market.GetStorePurchaseHistory(SessionID, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<string>.FromValue(history);
            }
            catch (Exception ex)
            {
                return Response<string>.FromError(ex.Message);
            }
        }

        public Response TransferFoundership(string storeID, string newFounderID) // change founder
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.TransferFoundership(SessionID, storeID, newFounderID); // Add method to MarketSystem
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
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddStorePurchasePolicy(SessionID, storeID, newPolicy, newPolicyProperties); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.RemoveStorePurchasePolicy(SessionID, storeID, policyID); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Policy was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddStorePurchaseStrategy(string storeID, Purchase_Strategy newStrategy, List<string> newStrategyProperties) // newPolicy = null OR newPolicyProperties = null)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddStorePurchaseStrategy(SessionID, storeID, newStrategy, newStrategyProperties);

                return new Response("Strategy was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveStorePurchaseStrategy(string storeID, String strategyID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.RemoveStorePurchaseStrategy(SessionID, storeID, strategyID); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
        public Response AddEmployeePermission(string storeID, string employeeID, string newPerm)
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

        internal void purchase(string session_id, List<ItemDTO> itemDTOs)
        {
            try
            {
                this.Market.purchase(session_id, itemDTOs);

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response RemoveEmployeePermission(string storeID, string employee_username, string permToRemove)
        {
            try
            {
                this.Market.RemoveEmployeePermission(this.SessionID, storeID, employee_username, (Permission)Enum.Parse(typeof(Permission), permToRemove)); // add to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Permission was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AssignNewOwner(string storeID, string newOwnerUsername)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.Assign_New_Owner(SessionID, newOwnerUsername, storeID);
                return new Response("New owner was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response<string> Remove_Store_Owner(string storeID, string other_Owner_Username)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.Remove_Store_Owner(SessionID, other_Owner_Username, storeID);
                return Response<string>.FromValue(("The owner:" + other_Owner_Username + " was removed successfully from store: " + storeID));
            }
            catch (Exception ex)
            {
                return Response<string>.FromError(("ERROR: " + ex.Message));
            }
        }

        public Response AssignNewManager(string storeID, string newOwnerUsername)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.Assign_New_Manager(SessionID, newOwnerUsername, storeID);
                return new Response("New manager was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetManagersOfTheStore(string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                List<string> managers = this.Market.GetStoreManagers(SessionID, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(managers);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response GetOwnersOfTheStore(string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                List<string> owners = this.Market.GetOwnersOfTheStore(SessionID, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(owners);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ManageEmployeePermissions(string storeID, string employee_username, List<string> additionalPerms) // update only for store manager - exchanges permissions
        {
            try
            {
                List<Permission> permList = additionalPerms.Select(x => (Permission)Enum.Parse(typeof(Permission), x)).ToList();
                this.Market.ManageEmployeePermissions(this.SessionID, storeID, employee_username, permList); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                List<ItemDTO> products = this.Market.GetProductsFromStore(SessionID, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<ItemDTO>>.FromValue(products);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductToStore(string storeID, List<String> productProperties)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                ItemDTO ret = this.Market.Add_Product_To_Store(storeID, SessionID, productProperties); // change method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return Response<ItemDTO>.FromValue(ret);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductFromStore(string storeID, string productID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.Remove_Product_From_Store(storeID, SessionID, productID); // change method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductComment(string productID, string comment, double rating)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddProductComment(SessionID, productID, comment, rating); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("New comment was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response LetGoProduct(string storeID, string productID, int quantity)
        {
            try
            {
                ItemDTO itemToLetGo = new ItemDTO(productID, quantity);
                this.Market.LetGoProduct(itemToLetGo); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product was removed from busket successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response SearchProductByKeyword(string keyword)
        {
            try
            {
                
                List<ItemDTO> keywordSearchResult = this.Market.SearchProductByKeyword( keyword); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<ItemDTO>>.FromValue(keywordSearchResult);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response SearchProductByName(string name)
        {
            try
            {

                List<ItemDTO> productNameSearchResult = this.Market.SearchProductByName(name); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<ItemDTO>>.FromValue(productNameSearchResult);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response SearchProductByCategory(string categoryID)
        {
            try
            {
                List<ItemDTO> categorySearchResult = this.Market.SearchProductByCategory( categoryID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<ItemDTO>>.FromValue(categorySearchResult);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductName(string productID, string name)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductName(SessionID, productID, name);
                return new Response("Product was renamed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductDescription(string productID, string description)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductDescription(SessionID, productID, description);
                return new Response("Product's description was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }
        //
        // // ==============================================================================================================
        // ==============================================================================================================
        // ==============================================================================================================

        public Response ChangeProductPrice(string productID, double price)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductPrice(SessionID, productID, price); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's price was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductRating(string productID, double rating)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductRating(SessionID, productID, rating); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's rating was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductQuantity(string productID, int quantity)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductQuantity(SessionID, productID, quantity); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's quantity was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductWeight(string productID, double weight)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductWeight(SessionID, productID, weight); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's weight was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductSale(string productID, double sale)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductSale(SessionID, productID, sale); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's sale was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductTimesBought(string productID, int times)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductTimesBought(SessionID, productID, times); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("The number of times the product were bought was updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductCategory(string productID, string categoryID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductCategory(SessionID, productID, categoryID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's category was changed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ChangeProductDimenssions(string productID, double[] dims)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.ChangeProductDimenssions(SessionID, productID, dims);
                return new Response("Product's dimenssions were updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductPurchasePolicy(string productID, Purchase_Policy newPolicy, List<string> newPolicyProperties) // newPolicy = null OR newPolicyProperties = null
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddProductPurchasePolicy(SessionID, productID, newPolicy, newPolicyProperties);
                return new Response("New product's purchase policy was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductPurchasePolicy(string productID, String policyID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.RemoveProductPurchasePolicy(SessionID, productID, policyID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's purchase policy was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AddProductPurchaseStrategy(string productID, Purchase_Strategy newStrategy, List<string> newStrategyProperties) // newStrategy = null OR newStrategyProperties = null
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddProductPurchaseStrategy(SessionID, productID, newStrategy, newStrategyProperties); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("New product's purchase strategy was added successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response RemoveProductPurchaseStrategy(string productID, String strategyID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.RemoveProductPurchaseStrategy(SessionID, productID, strategyID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Product's purchase strategy was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }




        // ======================================================
        // ======================== TODO ========================


        public Response apply_purchase_policy()
        { // I dont quite understand what its supposed to do....
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
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



