﻿using System;
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
                this.Market.ChangeStoreName(this.SessionID, storeID, newName); // string storeID, string newName - add to MarketSystem !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.Add_New_Store(this.SessionID, newStoreDetails); // change method in MarketSystem - cannot receive StoreID yet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Store was added successfully.");
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
                this.Market.close_store_temporary(this.SessionID, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                string history = this.Market.GetStorePurchaseHistory(this.SessionID, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.TransferFoundership(this.SessionID, storeID, newFounderID); // Add method to MarketSystem
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
                this.Market.AddStorePurchasePolicy(this.SessionID, storeID, newPolicy, newPolicyProperties); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.RemoveStorePurchasePolicy(this.SessionID, storeID, policyID); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.AddStorePurchaseStrategy(this.SessionID, storeID, newStrategy, newStrategyProperties);

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
                this.Market.RemoveStorePurchaseStrategy(this.SessionID, storeID, strategyID); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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

        public Response RemoveEmployeePermission(string storeID, string employeeID, string permToRemove)
        {
            try
            {
                this.Market.RemoveEmployeePermission(this.SessionID, storeID, employeeID, (Permission)Enum.Parse(typeof(Permission), permToRemove)); // add to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Permission was removed successfully.");
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response AssignNewOwner(string storeID, string newOwnerID)
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

        public Response AssignNewManager(string storeID, string newManagerID)
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

        public Response GetManagersOfTheStore(string storeID)
        {
            try
            {
                List<string> managers = this.Market.GetStoreManagers(this.SessionID, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                List<string> owners = this.Market.GetOwnersOfTheStore(this.SessionID, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(owners);
            }
            catch (Exception ex)
            {
                return new Response("ERROR: " + ex.Message);
            }
        }

        public Response ManageEmployeePermissions(string storeID, string employeeID, List<string> additionalPerms) // update only for store manager - exchanges permissions
        {
            try
            {
                List<Permission> permList = additionalPerms.Select(x => (Permission)Enum.Parse(typeof(Permission), x)).ToList();
                this.Market.ManageEmployeePermissions(this.SessionID, storeID, employeeID, permList); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                List<ItemDTO> products = this.Market.GetProductsFromStore(this.SessionID, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.Add_Product_To_Store(storeID, this.SessionID, productProperties); // change method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("New product was added successfully.");
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
                this.Market.Remove_Product_From_Store(storeID, this.SessionID, productID); // change method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.AddProductComment(this.SessionID, productID, comment, rating); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                List<ItemDTO> keywordSearchResult = this.Market.SearchProductByKeyword(this.SessionID, keyword); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                List<ItemDTO> productNameSearchResult = this.Market.SearchProductByName(this.SessionID, name); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                List<ItemDTO> categorySearchResult = this.Market.SearchProductByCategory(this.SessionID, categoryID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductName(this.SessionID, productID, name);
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
                this.Market.ChangeProductDescription(this.SessionID, productID, description);
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
                this.Market.ChangeProductPrice(this.SessionID, productID, price); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductRating(this.SessionID, productID, rating); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductQuantity(this.SessionID, productID, quantity); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductWeight(this.SessionID, productID, weight); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductSale(this.SessionID, productID, sale); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductTimesBought(this.SessionID, productID, times); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductCategory(this.SessionID, productID, categoryID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.ChangeProductDimenssions(this.SessionID, productID, dims);
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
                this.Market.AddProductPurchasePolicy(this.SessionID, productID, newPolicy, newPolicyProperties);
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
                this.Market.RemoveProductPurchasePolicy(this.SessionID, productID, policyID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.AddProductPurchaseStrategy(this.SessionID, productID, newStrategy, newStrategyProperties); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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
                this.Market.RemoveProductPurchaseStrategy(this.SessionID, productID, strategyID); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
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



