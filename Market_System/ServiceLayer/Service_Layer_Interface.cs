using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market_System.DomainLayer;
using Market_System.DomainLayer.StoreComponent;
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
        public Response<StoreDTO> GetStore(string store_id);//2.1 
        public Response<List<ItemDTO>> get_products_from_shop(string storeID);//2.1
        public Response<List<ItemDTO>> search_product_by_category(string category); //2.2
        public Response<List<ItemDTO>> search_product_by_keyword(string keyword); //2.2
        public Response<List<ItemDTO>> search_product_by_name(string name); //2.2
        public Response<string> add_product_to_basket(string product_id,string quantity);
        public Response<string> remove_product_from_basket(string product_id);
        public Response<StoreDTO> open_new_store(List<string> newStoreDetails); // 3.2
        public Response<string> comment_on_product(string productID, string comment, double rating); // 3.3
        public Response<ItemDTO> add_product_to_store(string storeID, string product_name,string description, string price,string quantity,string reserved_quantity,string rating,string sale,string wieght,string dimenstions,string attributes,string product_category); //4.1
        public Response<string> remove_product_from_store(string storeID, string productID); //4.1
      //  public void edit_product_details(); //4.1
        public Response<string> ChangeProductName(string productID, string name);
        public Response<string> ChangeProductDescription(string productID, string desc);
        public Response<string> ChangeProductPrice(string productID, double price);
        public Response<string> ChangeProductRating(string productID, double rating);
        public Response<string> ChangeProductQuantity(string productID, int quantity);
        public Response<string> ChangeProductWeight(string productID, double weight);
        public Response<string> ChangeProductSale(string productID, double sale);
        public Response<string> ChangeProductTimesBought(string productID, int times);
        public Response<string> ChangeProductCategory(string productID, string categoryID);
        public Response<string> ChangeProductDimenssions(string productID, double[] dims);
        public Response<string> AddProductPurchasePolicy(string productID, Purchase_Policy newPolicy, List<string> newPolicyProperties);
        public Response<string> RemoveProductPurchasePolicy(string productID, String policyID);
        public Response<string> AddProductPurchaseStrategy(string productID, Purchase_Strategy newStrategy, List<string> newStrategyProperties);
        public Response<string> RemoveProductPurchaseStrategy(string productID, String strategyID);
        public Response<string> assign_new_owner(string storeID, string new_owner_username); // 4.4
        public Response<string> assign_new_manager(string storeID, string new_manager_username); // 4.6
        // public void edit_manger_permissions(); //4.7
        public Response<string> ManageEmployeePermissions(string storeID, string employee_username, List<string> additionalPerms);
        public Response<string> AddEmployeePermission(string storeID, string employee_username, string newPerm);
        public Response<string> RemoveEmployeePermission(string storeID, string employee_username, string permToRemove);
        public Response<string> close_store_temporary(string storeID); //4.9
        public Response<List<string>> get_managers_of_store(string storeID); //4.11
        public Response<List<string>> get_owners_of_store( string storeID); //4.11
        public Response<string> get_purchase_history_from_store(string storeID); //4.13
        public Response<List<PurchaseHistoryObj>> get_purchase_history_of_a_member(); //6.4
        






    }
}
