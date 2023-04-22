﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.PaymentComponent;
using Market_System.DomainLayer.DeliveryComponent;

namespace Market_System.DomainLayer
{
    //TODO:: Implement as a Mediator.
    public sealed class MarketSystem
    {
        private static UserFacade userFacade;
        private static StoreFacade storeFacade;
        private static EmployeeRepo employeeRepo;
        private Random guest_id_generator;

        //This variable is going to store the Singleton Instance
        private static MarketSystem Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static MarketSystem GetInstance()
        {
            //This is thread-Safe - Performing a double-lock check.
            if (Instance == null)
            {
                //As long as one thread locks the resource, no other thread can access the resource
                //As long as one thread enters into the Critical Section, 
                //no other threads are allowed to enter the critical section
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        userFacade = UserFacade.GetInstance();
                        storeFacade = StoreFacade.GetInstance();
                        Instance = new MarketSystem();
                        Instance.register("admin", "admin", "address"); //registering an admin 
                        Instance.guest_id_generator = new Random();
                        

                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        public Cart get_cart_of_userID(string user_id)
        {
            string usrename = userFacade.get_username_from_user_id(user_id);
            return userFacade.get_cart(usrename);
        }

        internal void link_guest_with_session(string guest_name, string session_id)
        {
            userFacade.link_guest_with_session(guest_name, session_id);
        }

      

        public StoreDTO GetStore( string storeID)
        {
            try
            {
                return storeFacade.GetStore(storeID);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        internal void ChangeStoreName(string sessionID, string storeID, string newName)
        {
            try
            {
                storeFacade.ChangeStoreName(sessionID, storeID, newName);
            }

            catch(Exception e)
            {
                throw e;
            }
        }

        public string get_userid_from_session_id(string session_id)
        {

            return userFacade.get_userID_from_session(session_id);

        }

        public void Login(string username, string password) //for a registered Member
        {
            try
            {
                userFacade.Login(username, password);      
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void close_store_temporary(string sessionID, string storeID)
        {
            try
            {
                storeFacade.close_store_temporary(sessionID,storeID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string Add_Product_To_basket(string product_id,string session_id,string quantity)
        {
            string user_id = userFacade.get_userID_from_session(session_id);
            string usename = userFacade.get_username_from_user_id(user_id);
            lock (this)
            {

                        userFacade.add_product_to_basket(product_id, usename, int.Parse(quantity));
                        Market_System.DomainLayer.UserComponent.Cart cart = userFacade.get_cart(usename);
                        double price=storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                        //  price  =  storefacade.calcualte_total_price(cart);
                        
                        userFacade.update_cart_total_price(usename, price);
                        return "added product id : " + product_id + " to " + usename + "'s cart";
                    
                   
                
            }

        }

        internal void TransferFoundership(string sessionID, string storeID, string newFounderID)
        {
            try
            {
                 storeFacade.TransferFoundership(sessionID, storeID,newFounderID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void AddStorePurchasePolicy(string sessionID, string storeID, Purchase_Policy newPolicy, List<string> newPolicyProperties)
        {
            try
            {
                storeFacade.AddStorePurchasePolicy(sessionID, storeID, newPolicy);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void RemoveStorePurchasePolicy(string sessionID, string storeID, string policyID)
        {
            try
            {
                storeFacade.RemoveStorePurchasePolicy(sessionID, storeID, policyID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetStorePurchaseHistory(string sessionID, string storeID)
        {
            string user_id = get_userid_from_session_id(sessionID);
            try
            {
               return storeFacade.GetPurchaseHistoryOfTheStore(user_id, storeID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void AddStorePurchaseStrategy(string sessionID, string storeID, Purchase_Strategy newStrategy, List<string> newStrategyProperties)
        {
            try
            {
                 storeFacade.AddStorePurchaseStrategy(sessionID, storeID,newStrategy);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void RemoveStorePurchaseStrategy(string sessionID, string storeID, string strategyID)
        {
            try
            {
                storeFacade.RemoveStorePurchaseStrategy(sessionID, storeID, strategyID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReserveProduct(ItemDTO itemDTO)
        {
            storeFacade.ReserveProduct(itemDTO);
        }

        public string remove_product_from_basket(string product_id, string session_ID,int quantity)
        {
            string user_id = get_userid_from_session_id(session_ID);
            string username = userFacade.get_username_from_user_id(user_id);
            lock (this)
            {
              
                    if (userFacade.check_if_user_is_logged_in(username))// no need to check if he register , it is enought to check if he is logged in
                    {
                        //storeFacade.add_Product_to_Store(product_id);

                        userFacade.remove_product_from_basket(product_id, username,quantity);
                        Market_System.DomainLayer.UserComponent.Cart cart = userFacade.get_cart(username);
                        double  price  =  storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                      //  double price = 110;
                        userFacade.update_cart_total_price(username, price);
                        return "removed "+quantity+" of product id : " + product_id + " from " + userFacade.get_username_from_user_id(user_id) + "'s cart";
                    }
                    else
                    {
                        throw new Exception("user is not logged in");
                    }
                
                
            }

        }

        public void purchase(string session_id,List<ItemDTO> itemDTOs)
        {
            string userid = get_userid_from_session_id(session_id);
            try
            {
                storeFacade.Purchase(userid, itemDTOs);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void RemoveEmployeePermission(string sessionID, string storeID, string employee_username, StoreComponent.Permission permission)
        {
            try
            {
                string userID_from_SessionID = get_userid_from_session_id(sessionID);
                storeFacade.RemoveEmployeePermission(userID_from_SessionID, storeID, employee_username, permission);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void AddEmployeePermission(string sessionID, string storeID, string employeeID, StoreComponent.Permission permission)
        {
            try
            {
                string userID_from_SessionID = get_userid_from_session_id(sessionID);
                storeFacade.AddEmployeePermission(userID_from_SessionID, storeID,employeeID,permission);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal string change_password(string session_id, string new_password)
        {
            string user_id = userFacade.get_userID_from_session(session_id);
            string usename = userFacade.get_username_from_user_id(user_id);

            try
            {
                return userFacade.change_password(usename, new_password);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public bool isLoggedInAdministrator(string session)
        {
            string user_id = userFacade.get_userID_from_session(session);
            string username = userFacade.get_username_from_user_id(user_id);
            try
            {
                return userFacade.isLoggedInAdministrator(user_id, username);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void link_user_with_session(string username, string session_id)
        {
            userFacade.link_user_with_session(username, session_id);
        }

        public List<string> GetStoreManagers(string session_id,string store_id)
        {
            try
            {
                return storeFacade.GetManagersOfTheStore(session_id, store_id);
            }

            catch (Exception e)
            {
                throw e;
            }
            
        }

        internal List<string> GetOwnersOfTheStore(string sessionID, string storeID)
        {
            try
            {
                return storeFacade.GetOwnersOfTheStore(sessionID, storeID);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void unlink_userID_with_session(string session_id)
        {
            userFacade.unlink_userID_with_session(session_id);
        }
        

        public void Logout(string useriD)
        {
            string username = userFacade.get_username_from_user_id(useriD);
            try
            {
                userFacade.Logout(username);
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        public void register(string username, string password,string address)
        {
            try
            {
                userFacade.register(username, password, address);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void ManageEmployeePermissions(string sessionID, string storeID, string employee_username, List<StoreComponent.Permission> permList)
        {
            try
            {
                string userid_from_SessionID = get_userid_from_session_id(sessionID);
                storeFacade.ManageEmployeePermissions(userid_from_SessionID, storeID, employee_username, permList);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string login_guest()
        {
            string guest_name = "guest" + this.guest_id_generator.Next();
            try
            {
                userFacade.Login_guset(guest_name);
                return guest_name;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public StoreDTO Add_New_Store(string session_id, List<string> newStoreDetails)
        {
            string user_id = get_userid_from_session_id(session_id);
            try
            {
                return storeFacade.AddNewStore(user_id, newStoreDetails);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public ItemDTO Add_Product_To_Store(string storeID, string session_id, List<String> productProperties)
        {
            string user_id = get_userid_from_session_id(session_id);
            try
            {
                return storeFacade.AddProductToStore(storeID, user_id, productProperties);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string get_session_id_from_username(string username)
        {
            return userFacade.get_session_id_from_username(username);
        }

        internal List<ItemDTO> GetProductsFromStore(string sessionID, string storeID)
        {
            try
            {
              return  storeFacade.GetProductsFromStore(storeID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Remove_Product_From_Store(string store_ID, string session_id, string product_id)
        {
            try
            {
                storeFacade.RemoveProductFromStore(store_ID, session_id, product_id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Owner(string founder, string newOwner_ID, string store_ID)
        {
            try
            {
                storeFacade.AssignNewOwner(founder, store_ID, newOwner_ID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Manager(string founder, string new_manager_ID, string store_ID)
        {
            try
            {
                storeFacade.AssignNewManager(founder, store_ID, new_manager_ID); 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        

        public List<PurchaseHistoryObj> get_purchase_history_of_a_member(string session_ID)
        {
            string user_ID = userFacade.get_userID_from_session(session_ID);
            string usename = userFacade.get_username_from_user_id(user_ID);

            try
            {
                return userFacade.get_purchase_history_of_a_member(usename);
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        internal void AddProductComment(string sessionID, string productID, string comment, double rating)
        {
            try
            {
                 storeFacade.AddProductComment(sessionID,productID,comment,rating);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Check_Delivery(string adderss)
        {
            try 
            {
                DeliveryProxy.get_instance().deliver(adderss, 99);
                
                   // userFacade.Check_Delivery(username);
                    return "Delivery is available";
                
            }

            catch(Exception e)
            {
                throw e;
            }
            
        }



        public void save_purhcase_in_user(string session_id,Cart cart)
        {
            string user_id = get_userid_from_session_id(session_id);
            try
            {

             
                userFacade.save_purhcase_in_user(user_id, cart);
                userFacade.reset_cart(session_id);

             
            }

            catch (Exception e)
            {
                //TODO:: לבטל שריון של ההזמנה!!!!
                throw e;

            }
        }

        public string Check_Out(string username,string credit_card_details,Cart cart)
        {
            try
            {
                
              double price = storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                // price = 1000;
                PayCashService_Dummy.get_instance().pay(credit_card_details, price);
               // userFacade.save_purhcase_in_user(username,cart);
                
                return "Payment was successfull";
            }

            catch(Exception e)
            {
                //TODO:: לבטל שריון של ההזמנה!!!!
                throw e;
                
            }
        }

        internal void LetGoProduct(ItemDTO itemToLetGo)
        {
            try
            {
                 storeFacade.LetGoProduct(itemToLetGo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemDTO> SearchProductByKeyword(string keyword)
        {
            try
            {
             return    storeFacade.SearchProductByKeyword(keyword);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductName(string SessionID, string productID, string name)
        {
            try
            {
                storeFacade.ChangeProductName(SessionID, productID, name);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public List<ItemDTO> SearchProductByName(string name)
        {
            try
            {
                return storeFacade.SearchProductByName(name);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductDescription(string SessionID, string productID, string description)
        {
            try
            {
                storeFacade.ChangeProductName(SessionID, productID, description);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductPrice(string SessionID, string productID, double price)
        {
            try
            {
                storeFacade.ChangeProductPrice(SessionID, productID, price);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemDTO> SearchProductByCategory( string category)
        {
            try
            {
                return storeFacade.SearchProductByCategory(new Category(category));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductRating(string SessionID, string productID, double rating)
        {
            try
            {
                storeFacade.ChangeProductRating(SessionID, productID, rating);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductQuantity(string SessionID, string productID, int quantity)
        {
            try
            {
                storeFacade.ChangeProductQuantity(SessionID, productID, quantity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductWeight(string SessionID, string productID, double weight)
        {
            try
            {
                storeFacade.ChangeProductWeight(SessionID, productID, weight);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductSale(string SessionID, string productID, double sale)
        {
            try
            {
                storeFacade.ChangeProductSale(SessionID, productID, sale);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductTimesBought(string SessionID, string productID, int times)
        {
            try
            {
                storeFacade.ChangeProductTimesBought(SessionID, productID, times);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductCategory(string SessionID, string productID, string categoryID)
        {
            try
            {
                Category category = new Category(GetStoreIdFromProductID(productID));
                storeFacade.ChangeProductCategory(SessionID, productID, category);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeProductDimenssions(string SessionID, string productID, double[] dims)
        {
            try
            {
                storeFacade.ChangeProductDimenssions(SessionID, productID, dims);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddProductPurchasePolicy(string SessionID, string productID, Purchase_Policy newPolicy, List<string> newPolicyProperties)
        {
            try
            {
                //TODO:: WHY WE DO NOT USE NEWPOLICYPROPERTY ?
                storeFacade.AddProductPurchasePolicy(SessionID, GetStoreIdFromProductID(productID), productID, newPolicy);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveProductPurchasePolicy(string SessionID, string productID, string policyID)
        {
            try
            {
                storeFacade.RemoveProductPurchasePolicy(SessionID, GetStoreIdFromProductID(productID), productID, policyID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddProductPurchaseStrategy(string SessionID, string productID, Purchase_Strategy newStrategy, List<string>  newStrategyProperties)
        {
            try
            {
                //TODO:: WHY WE DO NOT USE NEWPOLICYPROPERTY ?
                storeFacade.AddProductPurchaseStrategy(SessionID, GetStoreIdFromProductID(productID), productID, newStrategy);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveProductPurchaseStrategy(string SessionID, string productID, string strategyID)
        {
            try
            {
                storeFacade.RemoveProductPurchaseStrategy(SessionID, GetStoreIdFromProductID(productID), productID, strategyID);
            }
            catch (Exception e)
            {
                throw e;
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

        public void destroy_me()
        {
            Instance = null;
            userFacade.Destroy_me();
            storeFacade.Destroy_me();
            PurchaseRepo.GetInstance().destroy_me();
            UserRepo.GetInstance().destroy_me();

        }

        public void AddNewAdmin(string sessionID, string Other_username)
        {
            try
            {
                userFacade.AddNewAdmin(sessionID, Other_username);

                //Add to Employees as well:
                string user_ID = userFacade.get_userID_from_session(sessionID);
                employeeRepo.addNewAdmin(user_ID);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public bool CheckIfAdmin(string sessionID, string Other_username)
        {
            try
            {
                return userFacade.CheckIfAdmin(sessionID, Other_username);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
