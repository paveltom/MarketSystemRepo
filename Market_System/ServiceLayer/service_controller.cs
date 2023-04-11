﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;

namespace Market_System.ServiceLayer
{
    public class Service_Controller : Service_Layer_Interface
    {
        private User_Service_Controller usc;
        private Store_Service_Controller ssc;
        private Random session_id_generator;
        private string session_id;

        public Service_Controller()
        {
            this.usc = new User_Service_Controller();
            this.ssc = new Store_Service_Controller();
            this.session_id_generator = new Random();
            this.session_id = session_id_generator.Next().ToString();
            new_guest_entered_the_website(session_id);
            //add kater login guest from here

        }

        private void new_guest_entered_the_website(string session_id)
        {
            try
            {
                string guest_name = this.usc.login_guest(session_id);

                Logger.get_instance().record_event("guest : " + guest_name + " has logged in");



            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in login_guest");

            }
        }

        public Response<string> add_product_to_basket(string product_id, string username)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.add_product_to_basket(product_id, username));
                Logger.get_instance().record_event(username + " added product with id: " + product_id + " to basket");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + " in add_product_to_basket");
                return Response<String>.FromError(e.Message);
            }


        }

        public void add_product_to_store(string storeID, List<string> ProductProperties)
        {

            try
            {
                //this.ssc.AddProductToStore(storeID, session_id, ProductProperties);
                // Response<string> ok = Response<string>.FromValue("successfully added product to store");
                Logger.get_instance().record_event("successfully added product to store: " + storeID);
                // return ok;



            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + " in add_product_to_store");
                // return Response<String>.FromError(e.Message);

            }
        }

        public void apply_purchase_policy()
        {
            throw new NotImplementedException();
        }

        public Response<string> assign_new_manager(string storeID, string newManagerID)
        {
            try
            {
                this.ssc.AssignNewManager(this.session_id, storeID, newManagerID);
                Response<string> ok = Response<string>.FromValue("done successfully");

                Logger.get_instance().record_event("assigning new manager with id : " + newManagerID + " to the store with id: " + storeID);

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in assign_new_manager");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> assign_new_owner(string storeID, string newOwnerID)
        {
            try
            {
                this.ssc.AssignNewOwner(this.session_id, storeID, newOwnerID);
                Response<string> ok = Response<string>.FromValue("done successfully");

                Logger.get_instance().record_event("assigning new owner with id : " + newOwnerID + " to the store with id: " + storeID);

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in assign_new_owner");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> check_delivery(string address)
        {

            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Check_Delivery(address));
                Logger.get_instance().record_event("checking deilvery for address: " + address + " succefully done.");
                return ok;



            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + " in check_delivery");
                return Response<String>.FromError(e.Message);

            }


        }

        public Response<string> change_password(string new_password)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.change_password(new_password, session_id));
                Logger.get_instance().record_event(ok.Value); // ok.vvalue is : "username changed password successfully" 

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + "in change_password");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> check_out(string username,string credit_card, Cart cart)
        {
            try
            {

                
                Response<string> ok = Response<string>.FromValue(this.usc.Check_Out(username,credit_card, cart));
                Logger.get_instance().record_event("checkout completed by : " + username );
                
                return ok;
            }
            catch (Exception e)
            {
                
                Logger.get_instance().record_error("error!!: " + e.Message+ "in check_out");
                return Response<String>.FromError(e.Message);
            }
            
           
        }


        public void close_store()
        {
            throw new NotImplementedException();
        }

        public Response<string> comment_on_product(string productID, string comment, double rating)
        {
            try
            {
                this.ssc.AddProductComment(this.session_id,productID,comment,rating);
                Response<string> ok = Response<string>.FromValue("done successfully");
               
                Logger.get_instance().record_event("a new comment for product id: "+productID );

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in comment_on_product");
                return Response<String>.FromError(e.Message);
            }
        }

        public void edit_manger_permissions()
        {
            throw new NotImplementedException();
        }

        public void edit_product_details()
        {
            throw new NotImplementedException();
        }

        public Response<List<string>> get_managers_of_store(string storeID)
        {

            try
            {
                Response<List<string>> ok = Response<List<string>>.FromValue(this.ssc.GetManagersOfTheStore(session_id, storeID));
                Logger.get_instance().record_event("getting managers from store : " + storeID + " done successfully");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in get_managers_of_store");
                return Response<List<string>>.FromError(e.Message);
            }
        }

        public Response<List<string>> get_owners_of_store(string storeID)
        {
            try
            {
                Response<List<string>> ok = Response<List<string>>.FromValue(this.ssc.GetOwnersOfTheStore(session_id,storeID));
                Logger.get_instance().record_event("getting owners from store : " + storeID + " done successfully");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in get_owners_of_store");
                return Response<List<string>>.FromError(e.Message);
            }
        }

        public Response<List<ItemDTO>> get_products_from_shop(string storeID)
        {
            try
            {


                Response<List<ItemDTO>> ok = Response<List<ItemDTO>>.FromValue(this.ssc.GetProductsFromStore(storeID));
                Logger.get_instance().record_event("getting products from store : " + storeID+" done successfully");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in get_products_from_shop");
                return Response<List<ItemDTO>>.FromError(e.Message);
            }
        }

        public void get_purchase_history_from_store()
        {
            throw new NotImplementedException();
        }

        public Response<List<PurchaseHistoryObj>> get_purchase_history_of_a_member(string username)
        {
            try
            {
                Response<List<PurchaseHistoryObj>> ok= Response<List<PurchaseHistoryObj>>.FromValue(this.usc.get_purchase_history_of_a_member(username));
                Logger.get_instance().record_event("getting purchase history of the user : " + username);
                
                return ok; 

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ "in get_purchase_history_of_a_member");
                return Response < List < PurchaseHistoryObj >>.FromError(e.Message);
            }
        }


        public void GetStore()
        {
            throw new NotImplementedException();
        }
        /*
        public Response login_guest()
        {
            try
            {
                Response<string> ok=Response<string>.FromValue(this.usc.login_guest());
                Logger.get_instance().record_event("guest : " + ok.Value+" has logged in");
               
                return ok;
                
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in login_guest");
                return Response<String>.FromError(e.Message);
            }
        }
        */
        public Response<string> login_member(string username,string pass)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Login_Member(username, pass, session_id));
               // this.usc.link_user_with_session(username, session_id);
                Logger.get_instance().record_event(username+"  has logged in!");
                
                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in login_member");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> log_out()
        {
            try
            {
                Response<string> ok=Response<string>.FromValue(this.usc.Logout(session_id));
                Logger.get_instance().record_event(ok.Value);
                
                return ok;
               
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in log_out");
                return Response<String>.FromError(e.Message);
            }
        }

        public void open_new_store()
        {
            throw new NotImplementedException();
        }

        public Response<string> register(string username,string pass,string address)
        {
             try
            {
                Response<string>ok= Response<string>.FromValue(this.usc.register(username, pass, address));
                Logger.get_instance().record_event(username+" has registered!");
               
                return ok;
                 
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ "in register");
                return Response<String>.FromError(e.Message);
            }
}

        public Response<string> remove_product_from_basket(string product_id,string username)
        {
            try
            {
                Response<string> ok=Response<string>.FromValue(this.usc.remove_product_from_basket(product_id, username));
                Logger.get_instance().record_event(username+" removed product with id: "+product_id+" from the basket");
             
                return ok;
                 
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in remove_product_from_basket");
                return Response<String>.FromError(e.Message);
            }
        }

        public void remove_product_from_store()
        {
            throw new NotImplementedException();
        }

        public Response<List<ItemDTO>> search_product_by_category(string category)
        {
            try
            {
                Response<List<ItemDTO>> ok = Response<List<ItemDTO>>.FromValue(this.ssc.SearchProductByCategory(new DomainLayer.StoreComponent.Category(category)));
                Logger.get_instance().record_event(" search by category : " + category + " was done successfully");

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in search_product_by_category");
                return Response<List<ItemDTO>>.FromError(e.Message);
            }
        }

        public Response<List<ItemDTO>> search_product_by_keyword(string keyword)
        {
            try
            {
                Response<List<ItemDTO>> ok = Response<List<ItemDTO>>.FromValue(this.ssc.SearchProductByKeyword(keyword));
                Logger.get_instance().record_event(" search by keyword : " + keyword + " was done successfully");

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in search_product_by_keyword");
                return Response<List<ItemDTO>>.FromError(e.Message);
            }
        }

        public Response<List<ItemDTO>> search_product_by_name(string name)
        {
            try
            {
                Response<List<ItemDTO>> ok = Response<List<ItemDTO>>.FromValue(this.ssc.SearchProductByName(name));
                Logger.get_instance().record_event(" search by name : " + name + " was done successfully");

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in search_product_by_name");
                return Response<List<ItemDTO>>.FromError(e.Message);
            }
        }

        public void destroy()
        {
            usc.destroy();
            ssc.destroy();
        }
    }
}