using System;
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

        internal void ChangeStoreName(string sessionID, string storeID, string newName)
        {
            storeFacade.ChangeStoreName(sessionID, storeID, newName);
        }

        public string get_username_from_session_id(string session_id)
        {
            return userFacade.get_username_from_session(session_id);
        }

        public void Login(string username, string password)
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
                storeFacade.RemoveStore(sessionID,storeID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string Add_Product_To_basket(string product_id,string username,string quantity)
        {

            lock (this)
            {
                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@change after store facade updates or implement this function


                //storeFacade.Remove_Product_From_Store(product_id); remove from comment after store 
                  
                        userFacade.add_product_to_basket(product_id, username,int.Parse(quantity));
                        Market_System.DomainLayer.UserComponent.Cart cart = userFacade.get_cart(username);
                        double price=storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                        //  price  =  storefacade.calcualte_total_price(cart);
                        
                        userFacade.update_cart_total_price(username, price);
                        return "added product id : " + product_id + " to " + username + "'s cart";
                    
                   
                
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

        internal List<string> GetStorePurchaseHistory(string sessionID, string storeID)
        {
            try
            {
               return storeFacade.GetPurchaseHistoryOfTheStore(sessionID, storeID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal void ReserveProduct(ItemDTO itemDTO)
        {
            storeFacade.ReserveProduct(itemDTO);
        }

        public string remove_product_from_basket(string product_id, string username)
        {
            lock (this)
            {
              
                    if (userFacade.check_if_user_is_logged_in(username))// no need to check if he register , it is enought to check if he is logged in
                    {
                        //storeFacade.add_Product_to_Store(product_id);

                        userFacade.remove_product_from_basket(product_id, username);
                        Market_System.DomainLayer.UserComponent.Cart cart = userFacade.get_cart(username);
                        double  price  =  storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                      //  double price = 110;
                        userFacade.update_cart_total_price(username, price);
                        return "removed product id : " + product_id + " from " + username + "'s cart";
                    }
                    else
                    {
                        throw new Exception("user is not logged in");
                    }
                
                
            }

        }

        internal string change_password(string username, string new_password)
        {
            return userFacade.change_password(username, new_password);
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

        public void unlink_user_with_session(string session_id)
        {
            userFacade.unlink_user_with_session(session_id);
        }
        

        public void Logout(string username)
        {
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
                userFacade.register(username, password,address);
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

        public void Add_New_Store(string username, int storeID)
        {
            try
            {
                storeFacade.Add_New_Store(username, storeID);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void Add_Product_To_Store(string storeID, string session_id, List<String> productProperties)
        {
            try
            {
                storeFacade.AddProductToStore(storeID, session_id, productProperties);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Remove_Product_From_Store(int store_ID, string founder, Product product)
        {
            try
            {
                storeFacade.Remove_Product_From_Store(store_ID, founder, product);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Owner(string founder, string username, int store_ID)
        {
            try
            {
                storeFacade.Assign_New_Owner(founder, username, store_ID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Manager(string founder, string username, int store_ID)
        {
            try
            {
                storeFacade.Assign_New_Managaer(founder, username, store_ID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        

        public List<PurchaseHistoryObj> get_purchase_history_of_a_member(string username)
        {
            try
            {
                return userFacade.get_purchase_history_of_a_member(username);
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

        public string Check_Out(string username,string credit_card_details,Cart cart)
        {
            try
            {
                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@change after store facade updates or implement this function
              double price = storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                // price = 1000;
                PayCashService_Dummy.get_instance().pay(credit_card_details, price);
                userFacade.save_purhcase_in_user(username,cart);
                
                return "Payment was successfull";
            }

            catch(Exception e)
            {
                //TODO:: לבטל שריון של ההזמנה!!!!
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

        public void ChangeProductProductCategory(string SessionID, string productID, string categoryID)
        {
            try
            {
                //TODO:: create a caregory here...
                var index = 0;
                for(int i = 0; i < categoryID.Length; i++)
                {
                    if(i == '_')
                    {
                        index = i;
                        break;
                    }
                }
                
                Category category = new Category(categoryID.Substring(index + 1));
                storeFacade.ChangeProductProductCategory(SessionID, productID, category);
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
                //TODO:: WHO IS STORE ID AND WHY DO IT APPEAR ONLY HERE AND NOT IN STORE SERVICE CONTROLLER - WHERE DO I GET IT FROM
                //ALSO: WHY WE DO NOT USE NEWPOLICYPROPERTY ?
                storeFacade.AddProductPurchasePolicy(SessionID, "CHANGETHISTOSTOREID!!!!!!", productID, newPolicy);
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
                //TODO:: WHO IS STORE ID AND WHY DO IT APPEAR ONLY HERE AND NOT IN STORE SERVICE CONTROLLER - WHERE DO I GET IT FROM
                storeFacade.RemoveProductPurchasePolicy(SessionID, "DUNNO WHO IS STORE ID CHANGE THIS", productID, policyID);
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
                //TODO:: WHO IS STORE ID AND WHY DO IT APPEAR ONLY HERE AND NOT IN STORE SERVICE CONTROLLER - WHERE DO I GET IT FROM
                storeFacade.AddProductPurchaseStrategy(SessionID, "DUNNO WHO IS STORE ID CHANGE THIS", productID, newStrategy);
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
                //TODO:: WHO IS STORE ID AND WHY DO IT APPEAR ONLY HERE AND NOT IN STORE SERVICE CONTROLLER - WHERE DO I GET IT FROM
                storeFacade.RemoveProductPurchaseStrategy(SessionID, "DUNNO WHO IS STORE ID CHANGE THIS", productID, strategyID);
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
    }
}
