using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market_System.DomainLayer;
using Market_System.DomainLayer.UserComponent;
using Market_System.ServiceLayer;


namespace Market_System.ServiceLayer
{
    interface Service_Layer_Interface
    {
        public Response<string> check_out(string username,string credit_card_details,Cart cart); //I.3 מערכת
        public Response<string> check_delivery(string address); //I.4 מערכת
        public Response<string> login_member(string username,string pass); // 1.4
       // public Response login_guest(); //1.1
        public Response<string> log_out(); //3.1
        public Response<string> register(string username, string pass,string address); // 1.3
        public void GetStore();//2.1 
        public Response<List<ItemDTO>> get_products_from_shop(string storeID);//2.1
        public Response<List<ItemDTO>> search_product_by_category(string category); //2.2
        public Response<List<ItemDTO>> search_product_by_keyword(string keyword); //2.2
        public Response<List<ItemDTO>> search_product_by_name(string name); //2.2
        public void apply_purchase_policy(); //2.5
        public Response<string> add_product_to_basket(string product_id, string username);
        public Response<string> remove_product_from_basket(string product_id, string username);
        public void open_new_store(); // 3.2
        public Response<string> comment_on_product(string productID, string comment, double rating); // 3.3
        public void add_product_to_store(string storeID, List<string> ProductProperties); //4.1
        public void remove_product_from_store(); //4.1
        public void edit_product_details(); //4.1
        public Response<string> assign_new_owner(string storeID, string newOwnerID); // 4.4
        public Response<string> assign_new_manager(string storeID, string newManagerID); // 4.6
        public void edit_manger_permissions(); //4.7
        public void close_store_temporary(); //4.9
        public Response<List<string>> get_managers_of_store(string storeID); //4.11
        public Response<List<string>> get_owners_of_store( string storeID); //4.11
        public void get_purchase_history_from_store(); //4.13
        public Response<List<PurchaseHistoryObj>> get_purchase_history_of_a_member(string username); //6.4

  

       

    }
}
