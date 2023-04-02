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
                return ok;
            }
            catch(Exception e)
            {
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

        public Response check_delivery(string username, Cart cart)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Check_Delivery(username, cart));
                return ok;
            }
            catch (Exception e)
            {
                return Response<String>.FromError(e.Message);
            }
        }

        public void check_delivery()
        {
            throw new NotImplementedException();
        }

        public Response check_out(string username, Cart cart)
        {
            try
            {
                usc.Check_Delivery(username, cart);
                Response<string> ok = Response<string>.FromValue(this.usc.Check_Out(username, cart));
                return ok;
            }
            catch (Exception e)
            {
                return Response<String>.FromError(e.Message);
            }
        }

        public void check_out()
        {
            throw new NotImplementedException();
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
                return Response<List<PurchaseHistoryObj>>.FromValue(this.usc.get_purchase_history_of_a_member(username));

            }
            catch (Exception e)
            {
                return Response<String>.FromError(e.Message);
            }
        }

        public void get_purchase_history_of_a_member()
        {
            throw new NotImplementedException();
        }

        public void get_shop()
        {
            throw new NotImplementedException();
        }

        public void login_guest()
        {
            throw new NotImplementedException();
        }

        public Response login_member(string username,string pass)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Login_Member(username, pass));
                return ok;
            }
            catch (Exception e)
            {
                return Response<String>.FromError(e.Message);
            }
        }

        public Response log_out()
        {
            try
            {
                return Response<string>.FromValue(this.usc.Logout());
               
            }
            catch (Exception e)
            {
                return Response<String>.FromError(e.Message);
            }
        }

        public void open_new_store()
        {
            throw new NotImplementedException();
        }

        public Response register(string username,string pass)
        {
             try
            {
                return Response<string>.FromValue(this.usc.register(username, pass));
                 
            }
            catch (Exception e)
            {
                return Response<String>.FromError(e.Message);
            }
}

        public void remove_product_from_basket()
        {
            throw new NotImplementedException();
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