﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using System.Xml.Linq;
using System.Timers;

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

        internal List<string> get_stores_that_user_works_in(string session_id)
        {
            try
            {

                return this.Market.get_user_wokring_stores(session_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool check_if_user_can_manage_stock(string session_id,string store_id)
        {
            try
            {
                
                return Market.check_if_user_can_manage_stock(session_id, store_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<string> get_stores_id_that_user_works_in(string session_id)
        {
            try
            {

                return this.Market.get_stores_id_that_user_works_in(session_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                throw ex;
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
                throw ex;
            }
        }

        public Response AddNewStore(string session_id,List<string> newStoreDetails)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                StoreDTO added = this.Market.Add_New_Store(session_id, newStoreDetails); // change method in MarketSystem - cannot receive StoreID yet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<StoreDTO>.FromValue(added);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public Response Reopen_Store(string storeID)
        {
            try
            {
                this.Market.Reopen_Store(SessionID, storeID);
                return new Response("Store was reopened successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<string> GetPurchaseHistoryOfTheStore(string session_id,string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                string history = this.Market.GetStorePurchaseHistory(session_id, storeID); // add method to MarketSystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<string>.FromValue(history);
            }
            catch (Exception ex)
            {
                return Response<string>.FromError(ex.Message);
            }
        }

        internal bool check_if_working_in_a_store()
        {
            return this.Market.check_if_working_in_a_store(this.SessionID);
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
                throw ex;
            }
        }

        public Response AddStorePurchasePolicy(string storeID, List<string> newPolicyProperties) // newPolicy = null OR newPolicyProperties = null
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddStorePurchasePolicy(SessionID, storeID, newPolicyProperties); // add method to market system!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("Policy was added successfully.\"");
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public Response AddStorePurchaseStrategy(string storeID, List<string> newStrategyProperties) // newPolicy = null OR newPolicyProperties = null)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddStorePurchaseStrategy(SessionID, storeID, newStrategyProperties);
                return new Response("Strategy was added successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }


        public Response<string> GetStoreProfitForDate(string storeID, DateTime dateTime)
        {
            try
            {
                string month = dateTime.Month.ToString();
                if (month.Length == 1)
                    month = "0" + month;
                string date_as_dd_MM_yyyy = dateTime.Day.ToString() + "_" + month + "_" + dateTime.Year.ToString();
                return Response<string>.FromValue(this.Market.GetStoreProfitForDate(this.SessionID, storeID, date_as_dd_MM_yyyy).ToString());
            }
            catch (Exception e) { throw e; }
        }


        public Response<string> GetMarketProfitForDate(DateTime dateTime)
        {
            try
            {
                string month = dateTime.Month.ToString();
                if (month.Length == 1)
                    month = "0" + month;
                string date_as_dd_MM_yyyy = dateTime.Day.ToString() + "_" + month + "_" + dateTime.Year.ToString();
                return Response<string>.FromValue(this.Market.GetMarketProfitForDate(this.SessionID, date_as_dd_MM_yyyy).ToString());
            }
            catch (Exception e) { throw e; }
        }



        // ======= Bid =========
        public Response<BidDTO> PlaceBid(string productID, double newPrice, int quantity, string card_number, string month, string year, string holder, string ccv, string id)
        {
            try
            {
                return Response<BidDTO>.FromValue(Market.PlaceBid(this.SessionID, productID, newPrice, quantity, card_number, month, year, holder, ccv, id));
            }
            catch (Exception e) { throw e; }
        }

        public string ApproveBid(string bidID)
        {
            try
            {
                Market.ApproveBid(this.SessionID, bidID);
                return "Your approvement was accepted.";
            }
            catch (Exception e) { throw e; }
        }


        public Response<BidDTO> GetBid(string bidID)
        {
            try
            {
                return Response<BidDTO>.FromValue(Market.GetBid(this.SessionID, bidID));
            }
            catch (Exception e) { throw e; }
        }


        public string CounterBid(string bidID, double counterPrice)
        {
            try
            {
                Market.CounterBid(this.SessionID, bidID, counterPrice);
                return "Counter offer was placed.";
            }
            catch (Exception e) { throw e; }
        }


        public string RemoveBid(string bidID)
        {
            try
            {
                Market.RemoveBid(this.SessionID, bidID);
                return "Bid was removed.";
            }
            catch (Exception e) { throw e; }
        }


        public Response<List<BidDTO>> GetStoreBids(string storeID)
        {
            try
            {
                return Response<List<BidDTO>>.FromValue( Market.GetStoreBids(this.SessionID, storeID));
            }
            catch (Exception e) { throw e; }
        }



        // ======= AUCTION =========

        public string SetAuction(string productID, double newPrice, long auctionMinutesDuration)
        {
            try
            {
                Market.SetAuction(this.SessionID, productID, newPrice, auctionMinutesDuration);
                return "Auction was created successfully.";
            }
            catch (Exception e) { throw e; }
        }

        public string UpdateAuction(string productID, double newPrice, string card_number, string month, string year, string holder, string ccv, string id)
        {
            try
            {
                Market.UpdateAuction(this.SessionID, productID, newPrice, card_number, month, year, holder, ccv, id);
                return "Auction product price was updated successfully.";
            }
            catch (Exception ex) { throw ex; }
        }

        public string RemoveAuction(string productID)
        {
            try
            {
                Market.RemoveAuction(this.SessionID, productID);
                return "Auction was removed successfully.";
            }
            catch (Exception e) { throw e; }
        }



        // ======= LOTTERY ========
        public string SetNewLottery(string productID, long durationInMinutes)
        {
            try
            {
                Market.SetNewLottery(this.SessionID, productID, durationInMinutes);
                return "Lottery was created successfully.";
            }
            catch (Exception e) { throw e; }
        }

        public string RemoveLottery(string productID)
        {
            try
            {
                Market.RemoveLottery(this.SessionID, productID);
                return "Lottery was removed successfully.";
            }
            catch (Exception e) { throw e; }
        }


        public string AddLotteryTicket(string productID, int percentage, string card_number, string month, string year, string holder, string ccv, string id)
        {

            try
            {
                Market.AddLotteryTicket(this.SessionID, productID, percentage, card_number, month, year, holder, ccv, id);
                return "Lottery ticket was added successfully.";
            }
            catch (Exception e) { throw e; }

        }

        public Response<Dictionary<string, int>> ReturnUsersLotteryTickets(string productID)
        {
            try
            {
                return Response<Dictionary<string, int>>.FromValue(Market.ReturnUsersLotteryTickets(this.SessionID, productID));
            }
            catch (Exception e) { throw e; }
        }


        public Response<int> RemainingLotteryPercantage(string productID)
        {
            try
            {
                return Response<int>.FromValue(Market.RemainingLotteryPercantage(this.SessionID, productID));
            }
            catch (Exception e) { throw e; }
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
                throw ex;
            }

        }

        internal void purchase(string transactionID)
        {
            try
            {
                this.Market.purchase(this.SessionID, transactionID);

                
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
                throw ex;
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
                throw ex;
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
                throw ex;
            }
        }

        internal bool check_if_can_assign_manager_or_owner(string session_id, string store_id)
        {
            try
            {

                return Market.check_if_can_assign_manager_or_owner(session_id, store_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response AssignNewManager(string storeID, string newManagerUsername)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.Assign_New_Manager(SessionID, newManagerUsername, storeID);
                return new Response("New manager was added successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response GetManagersOfTheStore(string session_id,string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                List<string> managers = this.Market.GetStoreManagers(session_id, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(managers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool check_if_can_show_infos(string session_id, string storeID)
        {
            try
            {

                return Market.check_if_can_show_infos(session_id, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool check_if_can_close_store(string session_id, string storeID)
        {
            try
            {

                return Market.check_if_can_close_store(session_id, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool check_if_can_remove_or_add_permessions(string session_id, string storeID)
        {
            try
            {

                return Market.check_if_can_remove_or_add_permessions(session_id, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response GetOwnersOfTheStore(string session_id,string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                List<string> owners = this.Market.GetOwnersOfTheStore(session_id, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(owners);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
            }
        }

        public void SuggestNewOwner(string storeID, string newOwnerUsername)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.SuggestNewOwner(SessionID, newOwnerUsername, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public void AcceptSuggestion(string session_id, string suggestionUsername, string storeID)
        {
            try
            {
                this.Market.AcceptSuggestion(session_id, suggestionUsername, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public void DeclineSuggestion(string session_id, string suggestionUsername, string storeID)
        {
            try
            {
                this.Market.DeclineSuggestion(session_id, suggestionUsername, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CheckAreThereSuggestions(string session_id, string storeID)
        {
            try
            {
                return this.Market.CheckAreThereSuggestions(session_id, storeID);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
            }
        }

        internal Response<List<string>> GetProductsFromStore_as_string(string session_id,string storeID)
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                List<string> products = this.Market.GetProductsFromStore_as_string(session_id, storeID); // add to marketsystem!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return Response<List<string>>.FromValue(products);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        internal Response<ItemDTO> get_product_by_productID(string product_id)
        {
            try
            {
                
                Response<ItemDTO> item = Response<ItemDTO>.FromValue(this.Market.get_product_by_productID(product_id));
                return item;
            }
            catch (Exception ex)
            {
                return Response<ItemDTO>.FromError(ex.Message);
            }
        }

        internal Response<List<ItemDTO>> GetProductsFromStores()
        {
            try
            {
                Response < List < ItemDTO >> okay= Response < List < ItemDTO >>.FromValue(this.Market.GetProductsFromAllStores());

                return okay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> get_all_comments_of_product(string product_id)
        {
            try
            {
                List<string> okay = this.Market.get_all_comments_of_product(product_id);

                return okay;
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public Response AddProductPurchasePolicy(string productID, List<string> newPolicyProperties) // newPolicy = null OR newPolicyProperties = null
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddProductPurchasePolicy(SessionID, productID, newPolicyProperties);
                return new Response("New product's purchase policy was added successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public Response AddProductPurchaseStrategy(string productID, List<string> newStrategyProperties) // newStrategy = null OR newStrategyProperties = null
        {
            try
            {
                //string user_ID = this.Market.get_userid_from_session_id(this.SessionID);
                this.Market.AddProductPurchaseStrategy(SessionID, productID, newStrategyProperties); // add method in MarketSystem!!!!!!!!!!!!!!!!!!!!!!!
                return new Response("New product's purchase strategy was added successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        internal List<string> get_all_categories()
        {
            try
            {
                return Market.get_all_categories();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        internal void set_new_session(string session_id)
        {
            this.SessionID = session_id;
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
                throw ex;
            }
        }


        internal void destroy()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!TODO
        }

        internal string calculatePrice(string session_id)
        {
            try
            {
                return Market.calculatePrice(session_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal Timer get_timer_of_auciton(string key)
        {
            try
            {
                return Market.get_timer_of_auciton(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // ======================== END of TODO ========================
        // =============================================================

    }
}



