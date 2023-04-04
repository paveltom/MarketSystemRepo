using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.Domain_Layer.User_Component;



namespace Market_System.Service_Layer
{
    public class Service_Controller : Service_Layer_Interface
    {
        private User_Service_Controller usc;
        private Store_Service_Controller ssc;
        
        public Service_Controller()
        {
            this.usc = new User_Service_Controller();
            this.ssc = new Store_Service_Controller();
        }
        public Response add_product_to_basket(string product_id,string username)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.add_product_to_basket(product_id, username));
                Logger.get_instance().record_event(username+" added product with id: "+product_id+" to basket");
                
                return ok;
            }
            catch(Exception e)
            {
                
                Logger.get_instance().record_error("error!!: " + e.Message+ " in add_product_to_basket");
                return Response<String>.FromError(e.Message);
            }

            
        }

        public void add_product_to_store()
        {
            throw new NotImplementedException();
        }

        public void apply_purchase_policy()
        {
            throw new NotImplementedException();
        }

        public void assign_new_manager()
        {
            throw new NotImplementedException();
        }

        public void assign_new_owner()
        {
            throw new NotImplementedException();
        }

        public Response check_delivery(string address)
        {
            
            try
            {
                Response<string> ok= Response<string>.FromValue(this.usc.Check_Delivery(address));
                Logger.get_instance().record_event("checking deilvery for address: "+ address+ " succefully done.");
                return ok;
                

                
            }
            catch (Exception e)
            {
                
                Logger.get_instance().record_error("error!!: " + e.Message+ " in check_delivery");
                return Response<String>.FromError(e.Message);

            }
            
         
        }

    

        public Response check_out(string username,string credit_card, Cart cart)
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

        public void comment_on_product()
        {
            throw new NotImplementedException();
        }

        public void edit_manger_permissions()
        {
            throw new NotImplementedException();
        }

        public void edit_product_details()
        {
            throw new NotImplementedException();
        }

        public void get_managers_of_store()
        {
            throw new NotImplementedException();
        }

        public void get_products_from_shop()
        {
            throw new NotImplementedException();
        }

        public void get_purchase_history_from_store()
        {
            throw new NotImplementedException();
        }

        public Response get_purchase_history_of_a_member(string username)
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
                return Response<String>.FromError(e.Message);
            }
        }


        public void get_shop()
        {
            throw new NotImplementedException();
        }

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

        public Response login_member(string username,string pass)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Login_Member(username, pass));
                Logger.get_instance().record_event(username+"  has logged in!");
                
                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in login_member");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response log_out()
        {
            try
            {
                Response<string> ok=Response<string>.FromValue(this.usc.Logout());
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

        public Response register(string username,string pass,string address)
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

        public Response remove_product_from_basket(string product_id,string username)
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

        public void search_product_by_category()
        {
            throw new NotImplementedException();
        }

        public void search_product_by_keyword()
        {
            throw new NotImplementedException();
        }

        public void search_product_by_name()
        {
            throw new NotImplementedException();
        }
    }
}