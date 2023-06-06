using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.ServiceLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
using System.IO;
using System.Web.Hosting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Market_System.ServiceLayer
{
    public class Service_Controller : Service_Layer_Interface
    {
        private User_Service_Controller usc;
        private Store_Service_Controller ssc;
        private Random session_id_generator;
        private string store_id_config;// this is used only for the config file

        internal Response<List<string>> get_stores_that_user_works_in()
        {
            try
            {
                Response<List<string>> okay = Response<List<string>>.FromValue(this.ssc.get_stores_that_user_works_in(session_id));
                return okay;
            }
            catch(Exception e)
            {
                return Response<List<string>>.FromError(e.Message);
            }
        }

        internal Response<bool> check_if_user_can_manage_stock(string store_id)
        {
            try
            {
                Response<bool> okay = Response<bool>.FromValue(this.ssc.check_if_user_can_manage_stock(session_id,store_id));
                return okay;
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }

        internal Response<List<string>> get_stores_id_that_user_works_in()
        {
            try
            {
                Response<List<string>> okay = Response<List<string>>.FromValue(this.ssc.get_stores_id_that_user_works_in(session_id));
                return okay;
            }
            catch (Exception e)
            {
                return Response<List<string>>.FromError(e.Message);
            }
        }

        private string session_id;





        public Service_Controller()
        {

            this.session_id_generator = new Random();
            this.session_id = session_id_generator.Next().ToString();
            this.usc = new User_Service_Controller();
            this.ssc = new Store_Service_Controller(session_id);
            new_guest_entered_the_website(session_id);
            if (first_time_running_project())
            {
                read_from_config_file();
                set_first_time_running_to_false();
            }
        }

        private void set_first_time_running_to_false()
        {
            this.usc.set_first_time_running_to_false();
        }

        private bool first_time_running_project()
        {
            return this.usc.first_time_running_project();
        }

        private void read_from_config_file()
        {
            string combine_me = "config_file.txt";
          
            string path;
            var temp_path = Directory.GetParent(Environment.CurrentDirectory).FullName;
            if (temp_path.Equals("C:\\Program Files (x86)") || temp_path.Equals("C:\\Program Files")) //Meaning that we're running the project.
            {

                string hosting_path = HostingEnvironment.ApplicationPhysicalPath;
                
             
                
               path = hosting_path + combine_me;
         
            }

            else 
            {
                if (temp_path.Equals("c:\\windows\\system32"))//Meaning that we're running a server
                {

                    string hosting_path = HostingEnvironment.ApplicationPhysicalPath;
                    path = hosting_path + combine_me;
                    path = hosting_path + combine_me;
                }
                else//Meaning that we're running the tests.
                {
                    string combine_me_2 = "\\Market_System\\";
                    int slice_me = temp_path.LastIndexOf('\\');
                    while (!temp_path.Substring(slice_me).Equals("\\MarketSystemRepo"))
                    {
                        temp_path = temp_path.Substring(0, slice_me);
                        slice_me = temp_path.LastIndexOf('\\');
                    }

                    path = temp_path + combine_me_2 + combine_me;
                }
              
            }

            StreamReader reader = new StreamReader(path);
            
            string current_command;
            try
            {
                while (!reader.EndOfStream)
                {
                    current_command = reader.ReadLine();
                    if (current_command[0] == '/')
                        continue;
                    execute_command(current_command);
                }

                reader.Close();
            }
            catch(Exception e)
            {
                //save somewhere the error
                Logger.get_instance().record_error("reading from configartion file failed due to: " + e.Message + " starting system without any configration");
                this.destroy();
                this.usc = new User_Service_Controller();
                this.ssc = new Store_Service_Controller(session_id);
                new_guest_entered_the_website(session_id);
            }
        }

        private void execute_command(string current_command)
        {
            Response res;
            string[] command = current_command.Split(' ');
            try
            {
                switch (command[0])
                {
                    case "register":
                        res= register(command[1], command[2], command[3]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "login":
                        res = login_member(command[1], command[2]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "checkout":
                        res=check_out(command[1], command[2], command[3], command[4], command[5],command[6]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "logout":
                       res= log_out();
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "add_product_to_basket":
                        res=add_product_to_basket(command[1], command[2]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "remove_product_from_basket":
                       res= remove_product_from_basket(command[1], command[2]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "open_new_store":
                        res = open_new_store(new List<string> { command[1] });
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        store_id_config = ((Response<StoreDTO>)res).Value.StoreID;
                        return;
                    case "comment_on_product":
                        res=comment_on_product(command[1], command[2], double.Parse(command[3]));
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "add_product_to_store":
                        res=add_product_to_store(store_id_config, command[1], command[2], command[3], command[4], command[5], command[6], command[7], command[8], command[9], command[10], command[11]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "remove_product_from_store":
                       res= remove_product_from_store(store_id_config, command[1]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "assign_new_owner":
                       res= assign_new_owner(store_id_config, command[1]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "remove_store_owner":
                      res=  Remove_Store_Owner(store_id_config, command[1]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "assign_new_manager":
                        res=assign_new_manager(store_id_config, command[1]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "addemployeepermission":
                       res= AddEmployeePermission(store_id_config, command[1], command[2]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "removeemployeepermission":
                       res= RemoveEmployeePermission(store_id_config, command[1], command[2]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                         return;
                    case "close_store_temporary":
                        res=close_store_temporary(store_id_config);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;
                    case "remove_a_member":
                        res=Remove_A_Member(command[1]);
                        if (res.ErrorOccured)
                            throw new Exception(res.ErrorMessage);
                        return;

                }
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        internal Response<bool> check_if_user_bought_item(string product_id)
        {
            try
            {
                Response<bool> result = Response<bool>.FromValue(this.usc.check_if_user_bought_item(product_id, this.session_id));
                return result;
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }

        internal Response< Dictionary<string, string>> extract_item_from_basket(string product_id)
        {
            try
            {
                Response<Dictionary<string,string>> result= Response < Dictionary<string, string> > .FromValue(this.usc.extract_item_from_basket(product_id, this.session_id));
                return result;
            }
            catch(Exception e)
            {
                return Response<Dictionary<string, string>>.FromError(e.Message);
            }
           
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

        internal Response<List<string>> show_basket_in_cart(string selected_store_id)
        {

            return this.usc.show_basket_in_cart(selected_store_id,this.session_id);

            
        }

        public Response<string> add_product_to_basket(string product_id, string quantity)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.add_product_to_basket(product_id, session_id, quantity));
                Logger.get_instance().record_event(this.usc.get_userID_from_session_id(session_id) + " added product with id: " + product_id + " to basket");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + " in add_product_to_basket");
                return Response<String>.FromError(e.Message);
            }


        }

        internal Response<bool> check_if_current_user_is_admin()
        {
            return Response<bool>.FromValue(this.usc.check_if_current_user_is_admin(session_id));
        }

        

        public Response<List<string>> get_store_ids_from_cart()
        {
            try
            {
                Response < List<string> > ok= Response < List<string> >.FromValue(this.usc.get_store_ids_from_cart(session_id));
               
                return ok;
            }
            catch(Exception e)
            {
                return Response<List<string>>.FromError(e.Message);
            }
        }



        public Response<bool> check_if_working_in_a_store()
        {
             Response<bool> result = Response<bool>.FromValue(this.ssc.check_if_working_in_a_store());
            return result;
        }

        public Response<ItemDTO> add_product_to_store(string storeID, string product_name, string description, string price, string quantity, string reserved_quantity, string rating, string sale, string wieght, string dimenstions, string attributes, string product_category)
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

            try
            {
                Response<ItemDTO> ok = (Response<ItemDTO>)this.ssc.AddProductToStore(storeID, ProductProperties);


                if (ok.ErrorOccured)
                {
                    Logger.get_instance().record_error("error!!: " + ok.ErrorMessage + "in add_product_to_store");

                }
                else
                {
                    Logger.get_instance().record_event("Added the product:" + ok.Value.GetID() + "to store: " + storeID + " has been done successfully");
                }
                return ok;
            }
            catch (Exception e)
            {
                return Response<ItemDTO>.FromError(e.Message);
            }
        } 

        public Response<string> assign_new_manager(string storeID, string newManagerUsername)
        {
            try
            {
                this.ssc.AssignNewManager( storeID, newManagerUsername);
                Response<string> ok = Response<string>.FromValue("done successfully");

                Logger.get_instance().record_event("assigning new manager with username : " + newManagerUsername + " to the store with id: " + storeID);

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in assign_new_manager");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> assign_new_owner(string storeID, string newOwnerUsername)
        {
            try
            {
                this.ssc.AssignNewOwner( storeID, newOwnerUsername);
                Response<string> ok = Response<string>.FromValue("done successfully");

                Logger.get_instance().record_event("assigning new owner with username : " + newOwnerUsername + " to the store with id: " + storeID);

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in assign_new_owner");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> check_delivery(string name, string address, string city, string country, string zip)
        {

            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Check_Delivery(name,address,city,country,zip));
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

        public Response<string> check_out(string card_number, string month, string year, string holder, string ccv, string id)
        {
            try
            {

                
                Response<string> ok = Response<string>.FromValue(this.usc.Check_Out(this.session_id, card_number,month,year,holder,ccv,id));
                //this.ssc.purchase(session_id, cart.convert_to_item_DTO());
                this.ssc.purchase(session_id);
                this.usc.save_purhcase_in_user(session_id);
               
                Logger.get_instance().record_event("checkout completed by : " + this.usc.getusername(session_id) );
                
                return ok;
            }
            catch (Exception e)
            {
                
                Logger.get_instance().record_error("error!!: " + e.Message+ "in check_out");
                return Response<String>.FromError(e.Message);
            }
            
           
        }

        internal Response<bool> check_if_can_assign_manager_or_owner(string storeID)
        {
            try
            {
                Response<bool> okay = Response<bool>.FromValue(this.ssc.check_if_can_assign_manager_or_owner(session_id, storeID));
                return okay;
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<string> close_store_temporary(string storeID)
        {
            try
            {
                this.ssc.close_store_temporary( storeID);
                Logger.get_instance().record_event(usc.get_userID_from_session_id(session_id)+" closed a store with the ID: "+storeID);
                Response<string> ok = Response<string>.FromValue("successfully closed store with ID: "+storeID);
                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + "in close_store_temporary");
                return Response<String>.FromError(e.Message);
            }
        }

        internal Response<bool> check_if_can_show_infos(string storeID)
        {
            try
            {
                Response<bool> okay = Response<bool>.FromValue(this.ssc.check_if_can_show_infos(session_id, storeID));
                return okay;
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }

        internal Response<bool> check_if_can_close_store(string storeID)
        {
            try
            {
                Response<bool> okay = Response<bool>.FromValue(this.ssc.check_if_can_close_store(session_id, storeID));
                return okay;
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }

        internal Response<bool> check_if_can_remove_or_add_permessions(string storeID)
        {
            try
            {
                Response<bool> okay = Response<bool>.FromValue(this.ssc.check_if_can_remove_or_add_permessions(session_id, storeID));
                return okay;
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<string> Reopen_Store(string storeID)
        {
            try
            {
                this.ssc.Reopen_Store(storeID);
                Logger.get_instance().record_event(usc.get_userID_from_session_id(session_id) + " reopend a store with the ID: " + storeID);
                Response<string> ok = Response<string>.FromValue("successfully reopend store with ID: " + storeID);
                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in Reopen_Store");
                return Response<String>.FromError(e.Message);
            }
        }






        public Response<string> AddStorePurchasePolicy(string storeID, List<string> newPolicyProperties) 
        {
            try
            {
                this.ssc.AddStorePurchasePolicy(storeID, newPolicyProperties);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("add a purchase policy for a store, for store id: " + storeID);
                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in AddStorePurchasePolicy");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> RemoveStorePurchasePolicy(string storeID, String policyID)
        {
            try
            {
                this.ssc.RemoveStorePurchasePolicy(storeID, policyID);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("removed a store purcahse policy, for product id: " + storeID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in RemoveStorePurchasePolicy");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> AddStorePurchaseStrategy(string storeID, List<string> newStrategyProperties) 
        {

            try
            {
                this.ssc.AddStorePurchaseStrategy(storeID, newStrategyProperties);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("add a purchase strategy for a store, for store id: " + storeID);
                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in AddStorePurchaseStrategy");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> RemoveStorePurchaseStrategy(string storeID, String strategyID)
        {

            try
            {
                this.ssc.RemoveStorePurchaseStrategy(storeID, strategyID);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("removed a store purcahse strategy, for product id: " + storeID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in RemoveStorePurchaseStrategy");
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

        public Response<string> ChangeProductDimenssions(string productID, double[] dims)
        {
            try
            {
                this.ssc.ChangeProductDimenssions(productID, dims);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("a product dimenssions change for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ChangeProductDimenssions");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> AddProductPurchasePolicy(string productID, List<string> newPolicyProperties)
        {

            try
            {
                this.ssc.AddProductPurchasePolicy(productID, newPolicyProperties);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("add a purchase policy for a product, for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in AddProductPurchasePolicy");
                return Response<String>.FromError(e.Message);
            }

        }

        public Response<string> RemoveProductPurchasePolicy(string productID, String policyID)
        {

            try
            {  
                this.ssc.RemoveProductPurchasePolicy(productID, policyID);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("removed a product purcahse policy, for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in RemoveProductPurchasePolicy");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> AddProductPurchaseStrategy(string productID, List<string> newStrategyProperties)
        {
            try
            {
                this.ssc.AddProductPurchaseStrategy(productID, newStrategyProperties);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("add a product purchase strategy, for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in AddProductPurhcaseStrategy");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response <string> RemoveProductPurchaseStrategy(string productID, String strategyID)
        {
            try
            {
                this.ssc.RemoveProductPurchaseStrategy(productID, strategyID);
                Response<string> ok = Response<string>.FromValue("done successfully");
                Logger.get_instance().record_event("remove a product purchase strategy, for product id: " + productID);
                return ok;
            }

            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in RemoveProductPurchaseStrategy");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<List<string>> get_managers_of_store(string storeID)
        {
            try
            {
                Response<List<string>> ok = (Response<List<string>>)this.ssc.GetManagersOfTheStore( this.session_id,storeID);
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
                Response<List<string>> ok = (Response<List<string>>)this.ssc.GetOwnersOfTheStore(this.session_id,storeID);
                Logger.get_instance().record_event("getting owners from store : " + storeID + " done successfully");

                return ok;
            }
            catch (Exception e)
            {

                Logger.get_instance().record_error("error!!: " + e.Message + "in get_owners_of_store");
                return Response<List<string>>.FromError(e.Message);
            }
        }


        public Response<ItemDTO> get_product_by_productID(string product_id)
        {
            try
            {
                Response<ItemDTO> ok = (Response<ItemDTO>)this.ssc.get_product_by_productID(product_id);
                

                return ok;
            }
            catch (Exception e)
            {


                return null;
            }
        }



        public Response<List<string>> get_all_comments_of_product(string product_id)
        {
            try
            {
                Response<List<string>> ok = Response<List<string>>.FromValue(this.ssc.get_all_comments_of_product(product_id));


                return ok;
            }
            catch (Exception e)
            {


                return Response<List<string>>.FromError(e.Message);
            }
        }

        public Response<List<ItemDTO>> get_not_zero_quantity_products_from_all_shop()
        {
            try
            {
                Response<List<ItemDTO>> ok = (Response<List<ItemDTO>>)this.ssc.GetProductsFromStores();


                return ok;
            }
            catch (Exception e)
            {


                return null;
            }
        }

        public Response<List<string>> get_products_from_shop_as_list_of_string(string storeID)
        {

            try
            {
                Response<List<string>> ok = (Response<List<string>>)this.ssc.GetProductsFromStore_as_string(this.session_id,storeID);
                

                return ok;
            }
            catch (Exception e)
            {

                
                return null;
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
                return null;
            }
        }

        public Response<string> get_purchase_history_from_store(string storeID)
        {
            Response<string> response = this.ssc.GetPurchaseHistoryOfTheStore(this.session_id, storeID);
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
                Logger.get_instance().record_event("getting purchase history of the user : " + this.usc.get_userID_from_session_id(session_id));
                
                return ok; 

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ "in get_purchase_history_of_a_member");
                return Response < List < PurchaseHistoryObj >>.FromError(e.Message);
            }
        }

        public Response<List<string>> get_all_categories()
        {
            try
            {
                Response<List<string>> ok = Response<List<string>>.FromValue(this.ssc.get_all_categories());

                return ok;
            }
            catch (Exception e)
            {
                return Response<List<string>>.FromError(e.Message);
            }
        }

        public Response<StoreDTO> GetStore(string store_id)
        {
            try
            {
                Response<StoreDTO> response = (Response<StoreDTO>)this.ssc.GetStore(store_id);
                if (response.ErrorOccured)
                {
                    Logger.get_instance().record_error("error!!: " + response.ErrorMessage + "in GetStore");

                }
                else
                {
                    Logger.get_instance().record_event("getting store with id: " + store_id + " was done successfully ");
                }

                return response;
            }

            catch(Exception e)
            {
                return null;
            }
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
              //  Logger.get_instance().record_event(username+"  has logged in!");
                
                return ok;
            }
            catch (Exception e)
            {
               // Logger.get_instance().record_error("error!!: " + e.Message+ " in login_member");
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
                this.ssc.set_new_session(session_id);
                new_guest_entered_the_website(session_id);// because now i am a guest
                return ok;
               
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message+ " in log_out");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<StoreDTO> open_new_store(List<string> newStoreDetails)
        {
            List<string> empty_list = new List<string>();
            try
            {
                Response<StoreDTO> ok = (Response<StoreDTO>)this.ssc.AddNewStore(this.session_id,newStoreDetails); //empty_list thye are doing nothing wiht it

                if (ok.ErrorOccured)
                {
                    Logger.get_instance().record_error("error!!: " + ok.ErrorMessage + "in open_new_store");

                }
                else
                {
                    Logger.get_instance().record_event("opening store with id: " + ok.Value.StoreID + " has failed");
                }

                return ok;
            }

            catch (Exception e)
            {
                return null;
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

        public Response<string> remove_product_from_basket(string product_id, string quantity)
        {
            try
            {
                Response<string> ok=Response<string>.FromValue(this.usc.remove_product_from_basket(product_id, session_id,quantity));
                Logger.get_instance().record_event(this.usc.get_userID_from_session_id(session_id)+" removed product with id: " +product_id+" from the basket");
             
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

        public Response<string> ManageEmployeePermissions(string storeID, string employee_username, List<string> additionalPerms)
        {
            try
            {
                Response<string> ok = (Response<string>)this.ssc.ManageEmployeePermissions(storeID,employee_username,additionalPerms);
                Logger.get_instance().record_event(" managed " + employee_username + " permissions was done successfully");

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in ManageEmployeePermissions");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> AddEmployeePermission(string storeID, string employee_username, string newPerm)
        {
            try
            {
                Response<string> ok = (Response<string>)this.ssc.AddEmployeePermission(storeID, employee_username, newPerm);
                Logger.get_instance().record_event(" added a new permission :  "+ newPerm + " to " + employee_username + "  was done successfully");

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in AddEmployeePermission");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> RemoveEmployeePermission(string storeID, string employee_username, string permToRemove)
        {
            try
            {
                Response<string> ok = (Response<string>)this.ssc.RemoveEmployeePermission(storeID, employee_username, permToRemove);
                Logger.get_instance().record_event(" removed  " + employee_username + "'s  permission to "+permToRemove+ " was done successfully");

                return ok;

            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in RemoveEmployeePermission");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> Read_System_Events()
        {
            try
            {
                this.usc.isLoggedInAdministrator(session_id); //Check if the user is logged-in as an administrator - hence, has a permission to perform this action.
                Response<string> system_Events = Response<string>.FromValue(Logger.get_instance().Read_Events_Record());
                Logger.get_instance().record_event("An admin has retrieved the System Events Logger file content successfuly");
                return system_Events; //TODO:: display the content of system_Events to the Admin!
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + "in Read_System_Events");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> Read_System_Errors()
        {
            try
            {
                this.usc.isLoggedInAdministrator(session_id); //Check if the user is logged-in as an administrator
                Response<string> system_Errors = Response<string>.FromValue(Logger.get_instance().Read_Errors_Record());
                Logger.get_instance().record_event("An admin has retrieved the System Erros Logger file content successfuly");
                return system_Errors; //TODO:: display the content of system_Events to the Admin!
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + "in Read_System_Errors");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> AddNewAdmin(string Other_username)
        {
            try
            {
                Response<string> response = Response<string>.FromValue(this.usc.AddNewAdmin(session_id, Other_username));
                Logger.get_instance().record_event("A new admin:" + Other_username + "has been added successfully");
                return response;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in AddNewAdmin");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> CheckIfAdmin(string Other_username)
        {
            try
            {
                Response<string> response;
                if (this.usc.CheckIfAdmin(session_id, Other_username))
                {
                    response = Response<string>.FromValue("The user is an admin");
                    Logger.get_instance().record_event("A check for this user:" + Other_username + "has been done successfully - he is an admin");
                }

                else
                {
                    response = Response<string>.FromValue("The user is NOT an admin");
                    Logger.get_instance().record_event("A check for this user:" + Other_username + "has been done successfully - he is NOT an admin");
                }

                return response;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in CheckIfAdmin");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> Remove_Store_Owner(string storeID, string other_Owner_Username)
        {
            try
            {
                Response<string> ok = this.ssc.Remove_Store_Owner(storeID, other_Owner_Username);
                Logger.get_instance().record_event("removed owner with username : " + other_Owner_Username + " from the store with id: " + storeID);

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in Remove_Store_Owner");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<string> Remove_A_Member(string member_Username)
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.Remove_A_Member(session_id, member_Username));
                Logger.get_instance().record_event("removed member with username : " + member_Username + " from market system");

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in Remove_A_Member");
                return Response<String>.FromError(e.Message);
            }
        }

        public Response<MemberDTO> GetMemberInfo(string member_Username)
        {
            try
            {
                Response<MemberDTO> ok = Response<MemberDTO>.FromValue(this.usc.Get_Member_Info(session_id, member_Username));
                Logger.get_instance().record_event("got member info with username : " + member_Username + " from market system");

                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in GetMemberInfo");
                return null;
            }
        }

        public bool HasNewMessages()
        {
            try
            {
                return this.usc.HasNewMessages(session_id);
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in HasNewMessages");
                return false;
            }
        }

        public Response<List<string>> GetMessages() //"Reads" the messages as well - The messages ARE NO LONGER NEW - we assume here that
            //the user has read them!!! 
            //NOTICE:: Use this function only when displaying messages to the user!!!!!!!!!!
        {
            try
            {
                Response<List<string>> ok = Response<List<string>>.FromValue(this.usc.GetMessages(session_id));
                Logger.get_instance().record_event("user with session ID : " + session_id + " got his Messages");
                return ok;
            }
            catch (Exception e)
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in GetMessages");
                return null;
            }
        }

        //yotam
        /// <summary>
        /// get user adress if 'username' is a logged in member
        /// </summary>
        /// <returns>'username' member adress or empty string</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal Response<string> getAdressOrEmpty()
        {
            try
            {
                Response<string> ok = Response<string>.FromValue(this.usc.getAdressOrEmpty(session_id));
                return ok;
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        internal Response<string> calculataePrice()
        {
            try
            {
               return Response<string>.FromValue(ssc.calculatePrice(session_id));
              
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> cancel_checkout(string transaction_id)
        {
            throw new NotImplementedException();
        }

        public Response<string> cancel_delivery(string transaction_id)
        {
            throw new NotImplementedException();
        }
        //yotam






        public Response<string> GetStoreProfitForDate(string storeID, DateTime dateTime)
        {
            try
            {
                Response<string> ret =  this.ssc.GetStoreProfitForDate(storeID, dateTime);
                Logger.get_instance().record_event("user with session ID : " + session_id + " requested profit for a store " + storeID + "on date " + dateTime);
                return ret;
            }
            catch (Exception e) 
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in GetStoreProfitForDate");
                return null;
            }
        }


        public Response<string> GetMarketProfitForDate(DateTime dateTime)
        {
            try
            {
                Response<string>  ret =  this.ssc.GetMarketProfitForDate(dateTime);
                Logger.get_instance().record_event("ADMIN with session ID : " + session_id + " requested profit for a market on date " + dateTime);
                return ret;

            }
            catch (Exception e) 
            {
                Logger.get_instance().record_error("error!!: " + e.Message + " in GetMarketProfitForDate");
                return null;
            }
        }


        // ======= Bid =========
        public Response<BidDTO> PlaceBid(string session, string productID, double newPrice, int quantity)
        {
            try
            {
                return ssc.PlaceBid(session, productID, newPrice, quantity);
            }
            catch (Exception e) { throw e; }
        }

        public Response ApproveBid(string session, string bidID)
        {
            try
            {
                return ssc.ApproveBid(session, bidID);
            }
            catch (Exception e) { throw e; }
        }


        public Response<BidDTO> GetBid(string session, string bidID)
        {
            try
            {
                return ssc.GetBid(session, bidID);
            }
            catch (Exception e) { throw e; }
        }


        public Response CounterBid(string session, string bidID, double counterPrice)
        {
            try
            {
                return ssc.CounterBid(session, bidID, counterPrice);
            }
            catch (Exception e) { throw e; }
        }


        public Response RemoveBid(string session, string bidID)
        {
            try
            {
                return ssc.RemoveBid(session, bidID);
            }
            catch (Exception e) { throw e; }
        }


        public Response<List<BidDTO>> GetStoreBids(string session, string storeID)
        {
            try
            {
                return ssc.GetStoreBids(session, storeID);
            }
            catch (Exception e) { throw e; }
        }


    }
}