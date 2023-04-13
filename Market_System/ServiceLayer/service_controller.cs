using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;

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
         
            this.session_id_generator = new Random();
            this.session_id = session_id_generator.Next().ToString();
            this.usc = new User_Service_Controller();
            this.ssc = new Store_Service_Controller(session_id);
            new_guest_entered_the_website(session_id);
            

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

        public Response<string> add_product_to_basket(string product_id,string quantity)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.add_product_to_basket(product_id,session_id,quantity));
                Logger.get_instance().record_event(this.usc.get_username_from_session_id(session_id) + " added product with id: " + product_id + " to basket");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + " in add_product_to_basket");
                return Response<String>.FromError(e.Message);
            }


        }

        public Response<string> add_product_to_store(string storeID, string product_name, string description, string price, string quantity, string reserved_quantity, string rating, string sale, string wieght, string dimenstions, string attributes, string product_category)
        {

            try
            {
                List<string> ProductProperties = new List<string>();
                ProductProperties.Add(product_name);
                ProductProperties.Add(description);
                ProductProperties.Add(price);
                ProductProperties.Add(quantity);
                ProductProperties.Add(reserved_quantity);
                ProductProperties.Add(rating);
                ProductProperties.Add(sale);
                ProductProperties.Add(wieght);
                ProductProperties.Add(dimenstions);
                ProductProperties.Add(attributes);
                ProductProperties.Add(product_category);

                this.ssc.AddProductToStore(storeID,  ProductProperties);
                 Response<string> ok = Response<string>.FromValue("successfully added product to store");
                Logger.get_instance().record_event("successfully added product to store: " + storeID);
                 return ok;



            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + " in add_product_to_store");
                return Response<String>.FromError(e.Message);

            }
        }

  

        public Response<string> assign_new_manager(string storeID, string newManagerID)
        {
            try
            {
                this.ssc.AssignNewManager( storeID, newManagerID);
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
                this.ssc.AssignNewOwner( storeID, newOwnerID);
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


        public Response<string> close_store_temporary(string storeID)
        {
            try
            {


                this.ssc.close_store_temporary( storeID);
                
                Logger.get_instance().record_event(usc.get_username_from_session_id(session_id)+" closed a store with the ID: "+storeID);
                Response<string> ok = Response<string>.FromValue("successfully closed store with ID: "+storeID);
                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in close_store_temporary");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> comment_on_product(string productID, string comment, double rating)
        {
            try
            {
                this.ssc.AddProductComment(productID,comment,rating);
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

        public Response<string> ChangeProductName(string productID, string name)
        {
            try
            {
                this.ssc.ChangeProductName(productID, name);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product name change for product id: " + productID);
                return ok;
            }

            catch(Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductName");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductDescription(string productID, string desc)
        {
            try
            {
                this.ssc.ChangeProductDescription(productID, desc);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product description change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductDescription");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductPrice(string productID, double price)
        {
            
            try
            {
                this.ssc.ChangeProductPrice(productID, price);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product price change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductPrice");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductRating(string productID, double rating)
        {
            
            try
            {
                this.ssc.ChangeProductRating(productID, rating);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product rating change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductRating");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductQuantity(string productID, int quantity)
        {
            
            try
            {
                this.ssc.ChangeProductQuantity(productID, quantity);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product quantity change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductQuantity");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductWeight(string productID, double weight)
        {
            
            try
            {
                this.ssc.ChangeProductWeight(productID, weight);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product weight change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductWeight");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductSale(string productID, double sale)
        {
            
            try
            {
                this.ssc.ChangeProductSale(productID, sale);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product sale change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductSale");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductTimesBought(string productID, int times)
        {
            
            try
            {
                this.ssc.ChangeProductTimesBought(productID, times);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product time bought change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductTimeBought");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> ChangeProductCategory(string productID, string categoryID)
        {
            try
            {
                this.ssc.ChangeProductCategory(productID, categoryID);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product category change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductCategory");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response ChangeProductDimenssions(string productID, double[] dims)
        {
            return this.ssc.ChangeProductDimenssions(productID, dims);
        }

        public Response AddProductPurchasePolicy(string productID, Purchase_Policy newPolicy, List<string> newPolicyProperties)
        {
            return this.ssc.AddProductPurchasePolicy(productID, newPolicy, newPolicyProperties);
        }

        public Response RemoveProductPurchasePolicy(string productID, String policyID)
        {
            return this.ssc.RemoveProductPurchasePolicy(productID, policyID);
        }

        public Response AddProductPurchaseStrategy(string productID, Purchase_Strategy newStrategy, List<string> newStrategyProperties)
        {
            return this.ssc.AddProductPurchaseStrategy(productID, newStrategy, newStrategyProperties);
        }

        public Response RemoveProductPurchaseStrategy(string productID, String strategyID)
        {
            return this.ssc.RemoveProductPurchaseStrategy(productID, strategyID);
        }

        public Response<List<string>> get_managers_of_store(string storeID)
        {
            try
            {
                Response<List<string>> ok = (Response<List<string>>)this.ssc.GetManagersOfTheStore( storeID);
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
                Response<List<string>> ok = (Response<List<string>>)this.ssc.GetOwnersOfTheStore(storeID);
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


                Response<List<ItemDTO>> ok = (Response<List<ItemDTO>>)this.ssc.GetProductsFromStore(storeID);
                Logger.get_instance().record_event("getting products from store : " + storeID+" done successfully");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in get_products_from_shop");
                return Response<List<ItemDTO>>.FromError(e.Message);
            }
        }

        public Response<List<string>> get_purchase_history_from_store(string storeID)
        {
            Response<List<string>> response = this.ssc.GetPurchaseHistoryOfTheStore(storeID);
            if (response.ErrorOccured)
            {
                Logger.get_instance().record_error("error!!: " + response.ErrorMessage + "in get_purchase_history_from_store");

            }
            else
            {
                Logger.get_instance().record_event("getting purchase history from a store with the ID: " + storeID + " was done successfully");
            }

            return response;
        }

        public Response<List<PurchaseHistoryObj>> get_purchase_history_of_a_member()
        {
            try
            {
                Response<List<PurchaseHistoryObj>> ok= Response<List<PurchaseHistoryObj>>.FromValue(this.usc.get_purchase_history_of_a_member(session_id));
                Logger.get_instance().record_event("getting purchase history of the user : " + this.usc.get_username_from_session_id(session_id));
                
                return ok; 

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ "in get_purchase_history_of_a_member");
                return Response < List < PurchaseHistoryObj >>.FromError(e.Message);
            }
        }


        public Response<ItemDTO> GetStore(string store_id)
        {
            Response < ItemDTO > response= (Response<ItemDTO>)this.ssc.GetStore(store_id);
            if(response.ErrorOccured)
            {
                Logger.get_instance().record_error("error!!: " + response.ErrorMessage + "in GetStore");
              
            }
            else
            {
                Logger.get_instance().record_event("getting store with id: " + store_id+" was done successfully");
            }

            return response;
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
                this.session_id = this.session_id_generator.Next().ToString(); //generate nwe session id
                new_guest_entered_the_website(session_id);// because now i am a guest
                return ok;
               
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in log_out");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> open_new_store()
        {
            try
            {
                List<string> empty_list = new List<string>();
                Response<string> ok = (Response<string>)this.ssc.AddNewStore(empty_list); //empty_list thye are doing nothing wiht it
                Logger.get_instance().record_event(ok.Value);
                
              
                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in open_new_store");
                return Response<String>.FromError(e.Message);
            }
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

        public Response<string> remove_product_from_basket(string product_id)
        {
            try
            {
                Response<string> ok=Response<string>.FromValue(this.usc.remove_product_from_basket(product_id, session_id));
                Logger.get_instance().record_event(this.usc.get_username_from_session_id(session_id)+" removed product with id: " +product_id+" from the basket");
             
                return ok;
                 
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in remove_product_from_basket");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> remove_product_from_store(string storeID, string productID)
        {
            try
            {
                
                
                this.ssc.RemoveProductFromStore(storeID, productID);
                Response<string> ok = Response<string>.FromValue("successfully removed a product with the ID: "+productID+" from a store with ID: "+storeID);
                Logger.get_instance().record_event("successfully removed product: "+productID+" from store: " + storeID);
                return ok;



            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in remove_product_from_store");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<List<ItemDTO>> search_product_by_category(string category)
        {
            try
            {
                Response<List<ItemDTO>> ok = (Response<List<ItemDTO>>)this.ssc.SearchProductByCategory(category);
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
                Response<List<ItemDTO>> ok = (Response<List<ItemDTO>>)this.ssc.SearchProductByKeyword(keyword);
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
                Response<List<ItemDTO>> ok = (Response<List<ItemDTO>>)this.ssc.SearchProductByName(name);
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