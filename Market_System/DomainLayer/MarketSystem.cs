using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.PaymentComponent;
using Market_System.DomainLayer.DeliveryComponent;
using Market_System.Domain_Layer.Communication_Component;

namespace Market_System.DomainLayer
{
    //TODO:: Implement this class as a Mediator.
    public sealed class MarketSystem
    {
        private static UserFacade userFacade;
        private static StoreFacade storeFacade;
        private static NotificationFacade notificationFacade;
        private static EmployeeRepo employeeRepo;
        private Random guest_id_generator;

        internal List<string> get_user_wokring_stores(string session_id)
        {
            string user_id = get_userid_from_session_id(session_id);
          return storeFacade.get_user_wokring_stores(user_id);
            
        }

        //This variable is going to store the Singleton Instance
        private static MarketSystem Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        internal void remove_guest_id_from_userRepo(string guest_id)
        {
            string guest_name = userFacade.get_username_from_user_id(guest_id);
            userFacade.remove_guest_from_users(guest_name);
            userFacade.remove_guest_id_from_userRepo(guest_id);
        }

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
                        
                        Instance.guest_id_generator = new Random();
                        employeeRepo = EmployeeRepo.GetInstance();
                        notificationFacade = NotificationFacade.GetInstance();
                        Instance.register("admin", "admin", "address"); //registering an admin 
                        StoreDTO first_store = Instance.initializing_store(new List<string> { "admin's_store" });
                        Instance.initializing_product(first_store.StoreID, new List<string> { "boots", "nice_boots", "100", "80", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "shoes" });
                        Instance.initializing_product(first_store.StoreID, new List<string> { "beer", "blue moon pub beer", "5", "800", "0", "5.0", "0", "2.0", "0.5_20.0_7.0", "attr", "drinks" });
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        private void initializing_product(string storeid, List<string> list)
        {
            Add_Product_To_Store(storeid, null, list);
        }

        private StoreDTO initializing_store(List<string> list)
        {
           return Add_New_Store(null, list);
        }

        internal Dictionary<string, string> extract_item_from_basket(string product_id, string session_id)
        {
            Dictionary<string, string> dict_to_return = new Dictionary<string, string>();
            dict_to_return.Add("name", storeFacade.get_product_name_from_prodcut_id(product_id));
            string user_id = get_userid_from_session_id(session_id);
            Cart cart = get_cart_of_userID(user_id);
            ItemDTO item=    cart.get_basket(storeFacade.GetStoreIdFromProductID(product_id)).extract_item(product_id);
            dict_to_return.Add("quantity", item.GetQuantity().ToString());
            double price = storeFacade.CalculatePrice(new List < ItemDTO > {item});
            dict_to_return.Add("price", price.ToString());
            return dict_to_return;

        }

        internal List<string> show_basket_in_cart(string selected_store_id,string session_id)
        {
            string user_id = get_userid_from_session_id(session_id);
            Cart cart = get_cart_of_userID(user_id);
            Bucket basket = cart.get_basket(selected_store_id);
            List<string> return_me = convert_basket_to_list_of_strings_to_show_user_in_GUI(basket);
            return return_me;
            
            
        }

        internal bool check_if_working_in_a_store(string sessionID)
        {
            string user_id = get_userid_from_session_id(sessionID);
            Dictionary<string, string> dict = storeFacade.GetStoresWorkingIn(user_id);
            return dict.Count > 0;
        }

        private List<string> convert_basket_to_list_of_strings_to_show_user_in_GUI(Bucket basket)
        {
           
           
                List<string> list_of_strings = new List<string>();
                string product_name = "";
    
                foreach (KeyValuePair<string, int> entry in basket.get_products()) // each entry is < product_id , quantity > 
                {
                product_name = storeFacade.get_product_name_from_prodcut_id(entry.Key);
                list_of_strings.Add("product ID: "+entry.Key+"  "+product_name + "   quantity : " + entry.Value.ToString());
                }
                return list_of_strings;
            
        }

        internal string link_guest_with_user_id(string guest_name)
        {
           return userFacade.link_guest_with_user_id(guest_name);
        }

        public Cart get_cart_of_userID(string user_id)
        {
            string usrename = userFacade.get_username_from_user_id(user_id);
            return userFacade.get_cart(usrename);
        }

        public void link_guest_with_session(string guest_id, string session_id)
        {
            userFacade.link_guest_with_session(guest_id, session_id);
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

        public List<string> get_store_ids_from_cart(string session_id)
        {
            
                string user_id = get_userid_from_session_id(session_id);
                Cart cart = get_cart_of_userID(user_id);
                if(cart.gett_all_baskets().Count==0)
            {
                throw new Exception("cart is empty!!");
            }
                List<string> return_me = new List<string>();
                foreach (Bucket basket in cart.gett_all_baskets())
                {
                    return_me.Add(basket.get_store_id());
                }

                return return_me;
            
            
        }

        public void ChangeStoreName(string sessionID, string storeID, string newName)
        {
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                storeFacade.ChangeStoreName(user_id, storeID, newName);
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

        public void close_store_temporary(string sessionID, string storeID)
        {
            try
            {
                var founderID = storeFacade.GetStore(storeID).FounderID;
                List<EmployeeDTO> employees = new List<EmployeeDTO>();
                employees.AddRange(storeFacade.GetStore(storeID).owners);
                employees.AddRange(storeFacade.GetStore(storeID).managers);

                string user_id = get_userid_from_session_id(sessionID);
                storeFacade.close_store_temporary(user_id,storeID);

                //Send Notification to store Employees (owners & managers)
                var message = "The store id: " + storeID + " has been temporarely closed.";
                sendMessageToStoreEmployees_ForCloseTempStore(employees, message, userFacade.get_username_from_user_id(founderID), storeID);
                sendMessageToUser(message, founderID, "System");
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void Reopen_Store(string sessionID, string storeID)
        {
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                storeFacade.RestoreStore(user_id, storeID);

                //Send Notification to store Employees (owners & managers)
                var message = "The store id: " + storeID + " has been Reopened.";
                var founderID = storeFacade.GetStore(storeID).FounderID;
                sendMessageToStoreEmployees(message, userFacade.get_username_from_user_id(founderID), storeID);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string Add_Product_To_basket(string product_id,string session_id,string quantity)
        {
            try
            {
                string user_id = userFacade.get_userID_from_session(session_id);
                string usename = userFacade.get_username_from_user_id(user_id);
                lock (this)
                {

                    userFacade.add_product_to_basket(product_id, usename, int.Parse(quantity));
                    Market_System.DomainLayer.UserComponent.Cart cart = userFacade.get_cart(usename);
                    double price = storeFacade.CalculatePrice(cart.convert_to_item_DTO());
                    //  price  =  storefacade.calcualte_total_price(cart);

                    userFacade.update_cart_total_price(usename, price);
                    return "added product id : " + product_id + " to " + usename + "'s cart";
                }
            }

            catch(Exception e)
            {
                throw e;
            }

        }

        public void TransferFoundership(string sessionID, string storeID, string newFounderID)
        {
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                storeFacade.TransferFoundership(user_id, storeID,newFounderID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        internal bool HasNewMessages(string sessionID)
        {
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                return notificationFacade.HasNewMessages(user_id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> GetMessages(string session_id)
        {
            try
            {
                string user_id = get_userid_from_session_id(session_id);
                return notificationFacade.GetMessages(user_id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddStorePurchasePolicy(string sessionID, string storeID, Purchase_Policy newPolicy, List<string> newPolicyProperties)
        {
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                storeFacade.AddStorePurchasePolicy(user_id, storeID, newPolicy);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveStorePurchasePolicy(string sessionID, string storeID, string policyID)
        {
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                storeFacade.RemoveStorePurchasePolicy(user_id, storeID, policyID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetStorePurchaseHistory(string sessionID, string storeID)
        {
            
            try
            {
                string user_id = get_userid_from_session_id(sessionID);
                return storeFacade.GetPurchaseHistoryOfTheStore(user_id, storeID);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddStorePurchaseStrategy(string sessionID, string storeID, Purchase_Strategy newStrategy, List<string> newStrategyProperties)
        {
            try
            {
                string userID = userFacade.get_userID_from_session(sessionID);
                storeFacade.AddStorePurchaseStrategy(userID, storeID,newStrategy);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveStorePurchaseStrategy(string sessionID, string storeID, string strategyID)
        {
            try
            {
                string userID = userFacade.get_userID_from_session(sessionID);
                storeFacade.RemoveStorePurchaseStrategy(userID, storeID, strategyID);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public MemberDTO Get_Member_Info(string session_id, string member_Username)
        {
            try
            {
                //check if current user is an admin:
                string user_ID = userFacade.get_userID_from_session(session_id);
                CheckIfAdmin(session_id, userFacade.get_username_from_user_id(user_ID));

                //check if he has any roles in the system: owner/manager/admin/etc...
                string other_User_ID = userFacade.get_user_id_from_username(member_Username);
                bool isAdmin = CheckIfAdmin(session_id, member_Username);
                User otherUser = userFacade.getUser(member_Username);
                List<PurchaseHistoryObj> pruchase_History = null;
                try
                {
                    pruchase_History = userFacade.get_purchase_history_of_other_member(session_id, member_Username);
                }

                catch(Exception e)
                {
                    //do nothing...
                }
                Dictionary<string, string> stores_Working_In = storeFacade.GetStoresWorkingIn(other_User_ID);
                return new MemberDTO(otherUser, isAdmin, pruchase_History, stores_Working_In);

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
            try
            {
                string userID = userFacade.get_userID_from_session(session_id);
                storeFacade.Purchase(userID, itemDTOs);
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
                string userID = userFacade.get_userID_from_session(session_id);
                return storeFacade.GetManagersOfTheStore(userID, store_id);
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
                string userID = userFacade.get_userID_from_session(sessionID);
                return storeFacade.GetOwnersOfTheStore(userID, storeID);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string unlink_userID_with_session(string session_id)
        {
            return userFacade.unlink_userID_with_session(session_id);
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
            try
            {
                string guest_name = "guest" + this.guest_id_generator.Next();
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
            try
            {
                if(session_id==null)//it means it is the inizilating of the system
                {
                    string admin_id = userFacade.get_user_id_from_username("admin");
                    return storeFacade.AddNewStore(admin_id, newStoreDetails);
                }
                string user_id = get_userid_from_session_id(session_id);
                return storeFacade.AddNewStore(user_id, newStoreDetails);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public ItemDTO Add_Product_To_Store(string storeID, string session_id, List<String> productProperties)
        {
            try
            {
                if (session_id == null)//it means it is the inizilating of the system
                {
                    string admin_id = userFacade.get_user_id_from_username("admin");
                    return storeFacade.AddProductToStore(storeID, admin_id, productProperties);
                }
                string user_id = get_userid_from_session_id(session_id);
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

        internal ItemDTO get_product_by_productID(string product_id)
        {
            string store_id = getStoreIDFromProductID(product_id);
            List<ItemDTO> items = GetProductsFromStore(null,store_id);
            foreach(ItemDTO item in items)
            {
                if (item.GetID().Equals(product_id))
                {
                    item.set_name(storeFacade.get_product_name_from_prodcut_id(product_id));
                    return item;
                }
            }
            throw new Exception("item is not found");
        }

        internal List<ItemDTO> GetProductsFromAllStores()
        {
            try
            {
                return storeFacade.GetProductsFromAllStores();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Owner(string owner_SessionID, string newOwnerUsername, string store_ID)
        {
            try
            {
                string userID = userFacade.get_userID_from_session(owner_SessionID);
                string newOwner_ID = userFacade.get_user_id_from_username(newOwnerUsername);
                storeFacade.AssignNewOwner(userID, store_ID, newOwner_ID);

                //Notify the new owner
                var message = "You've been promoted to a store owner in store id: " + store_ID;
                notificationFacade.AddNewMessage(newOwner_ID, userFacade.get_username_from_user_id(userID), message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal List<string> GetProductsFromStore_as_string(string sessionID, string storeID)
        {
            List<string> return_me = new List<string>();
            List<ItemDTO> items = this.GetProductsFromStore(sessionID, storeID);
            foreach(ItemDTO item in items)
            {
                return_me.Add("product name:  " + storeFacade.get_product_name_from_prodcut_id(item.GetID()) + "   ID:   " + item.GetID() + "   quantity:  " + item.GetQuantity());
            }
            return return_me;
        }

        public void Remove_Store_Owner(string sessionID, string other_Owner_Username, string storeID)
        {
            try
            {
                string userID = userFacade.get_userID_from_session(sessionID);
                string other_Owner_ID = userFacade.get_user_id_from_username(other_Owner_Username);
                storeFacade.Remove_Store_Owner(userID, storeID, other_Owner_ID);

                //Notify the removed owner
                var message = "You have been removed from store ownership in store id: " + storeID;
                notificationFacade.AddNewMessage(other_Owner_ID, userFacade.get_username_from_user_id(userID), message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Assign_New_Manager(string founder_SessionID, string newManagerUsername, string store_ID)
        {
            try
            {
                string userID = userFacade.get_userID_from_session(founder_SessionID);
                string new_manager_ID = userFacade.get_user_id_from_username(newManagerUsername);
                storeFacade.AssignNewManager(userID, store_ID, new_manager_ID);

                //Notify the new manager
                var message = "You've been promoted to a store manager in store id: " + store_ID;
                notificationFacade.AddNewMessage(new_manager_ID, userFacade.get_username_from_user_id(userID), message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal List<string> get_all_categories()
        {
            try
            {
                return storeFacade.get_all_categories();
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
                string userID = userFacade.get_userID_from_session(sessionID);
                storeFacade.AddProductComment(userID,productID,comment,rating);
                var username = userFacade.get_username_from_user_id(userID);

                //Send notification to store owners
                var message = "New product comment has been added by: " + username + ", on product id: " + productID;

                //get Store ID and store Owners:
                var storeID = getStoreIDFromProductID(productID);

                //Send Notification to the store Owners
                sendMessageToStoreOwners(message, "System", storeID);
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

                //Check if the product's quantity is 0                
                foreach(ItemDTO product in cart.convert_to_item_DTO())
                {
                    if(product.GetQuantity() <= 0) //Send Notification to the store Owners if so
                    {
                        var message = "The quantity of product id: " + product.GetID() + " has ended.";
                        var userID = userFacade.get_user_id_from_username(username);
                        //get Store ID and store Owners:
                        var storeID = getStoreIDFromProductID(product.GetID());

                        //Send Notification to the store Owners
                        sendMessageToStoreOwners(message, "System", storeID);
                    }
                }             
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductName(userID, productID, name);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductName(userID, productID, description);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductPrice(userID, productID, price);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductRating(userID, productID, rating);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductQuantity(userID, productID, quantity);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductWeight(userID, productID, weight);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductSale(userID, productID, sale);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductTimesBought(userID, productID, times);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                Category category = new Category(GetStoreIdFromProductID(productID));
                storeFacade.ChangeProductCategory(userID, productID, category);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.ChangeProductDimenssions(userID, productID, dims);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                //TODO:: WHY WE DO NOT USE NEWPOLICYPROPERTY ?
                storeFacade.AddProductPurchasePolicy(userID, GetStoreIdFromProductID(productID), productID, newPolicy);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.RemoveProductPurchasePolicy(userID, GetStoreIdFromProductID(productID), productID, policyID);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                //TODO:: WHY WE DO NOT USE NEWPOLICYPROPERTY ?
                storeFacade.AddProductPurchaseStrategy(userID, GetStoreIdFromProductID(productID), productID, newStrategy);
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
                string userID = userFacade.get_userID_from_session(SessionID);
                storeFacade.RemoveProductPurchaseStrategy(userID, GetStoreIdFromProductID(productID), productID, strategyID);
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

        public void Remove_A_Member(string sessionID, string member_Username)
        {
            try
            {
                //check if current user is an admin:
                string user_ID = userFacade.get_userID_from_session(sessionID);
                CheckIfAdmin(sessionID, userFacade.get_username_from_user_id(user_ID));

                //check if he has any roles in the system: owner/manager/admin/etc...
                string other_User_ID = userFacade.get_user_id_from_username(member_Username);
                if (!CheckIfAdmin(sessionID, member_Username) && storeFacade.Check_If_Member_Only(other_User_ID))
                {
                    userFacade.Remove_A_Member(member_Username);
                }
                else
                {
                    throw new Exception("A member with roles in the system cannot be removed! You'll need to remove his roles first.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string getStoreIDFromProductID(string productID)
        {
            char[] spearator = { '_' };
            return productID.Split(spearator)[0];
        }

        private void sendMessageToStoreOwners(string message, string senderUsername, string storeID)
        {
            //Send Notification to the store Owners
            foreach (EmployeeDTO emp in storeFacade.GetStore(storeID).owners)
            {
                notificationFacade.AddNewMessage(emp.UserID, senderUsername, message);
            }
            //Send to founder as well - he is sort of owner too...
            var founderID = storeFacade.GetStore(storeID).FounderID;
            notificationFacade.AddNewMessage(founderID, senderUsername, message);
        }

        private void sendMessageToStoreManagers(string message, string senderUsername, string storeID)
        {
            //Send Notification to the store Managers
            foreach (EmployeeDTO emp in storeFacade.GetStore(storeID).managers)
            {
                notificationFacade.AddNewMessage(emp.UserID, senderUsername, message);
            }
        }

        private void sendMessageToStoreEmployees(string message, string senderUsername, string storeID)
        {
            //Send Notification to the store Owners
            foreach (EmployeeDTO emp in storeFacade.GetStore(storeID).owners)
            {
                notificationFacade.AddNewMessage(emp.UserID, senderUsername, message);
            }

            //Send Notification to the store Managers
            foreach (EmployeeDTO emp in storeFacade.GetStore(storeID).managers)
            {
                notificationFacade.AddNewMessage(emp.UserID, senderUsername, message);
            }

            //Send Notification to the Founder as well
            var founderID = storeFacade.GetStore(storeID).FounderID;
            notificationFacade.AddNewMessage(founderID, senderUsername, message);
        }

        private void sendMessageToStoreEmployees_ForCloseTempStore(List<EmployeeDTO> employees, string message, string senderUsername, string storeID)
        {
            //Send Notification to the store Owners & Managers & Founder - employees of the store
            foreach (EmployeeDTO emp in employees)
            {
                notificationFacade.AddNewMessage(emp.UserID, senderUsername, message);
            }
        }

        private void sendMessageToUser(string message, string userID, string from)
        {
            notificationFacade.AddNewMessage(userID, from, message);
        }
    }
}
