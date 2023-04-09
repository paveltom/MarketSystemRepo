using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer;
using Market_System.ServiceLayer;

namespace Market_System.ServiceLayer
{
    public class Store_Service_Controller
    {
        private MarketSystem Market;

        public Store_Service_Controller()
        {
            this.Market = MarketSystem.GetInstance();
        }

        // ====================================================================
        // ====================== General class methods ===============================

        public Response Purchase(string userID, List<string> productsIDS)
        {

        }


        // ====================== END of General class methods ===============================
        // ===================================================================================



        // ====================================================================
        // ====================== Store methods ===============================

        public Response ChangeStoreName(string userID, string storeID, string newName)
        {

        }

        public Response GetStore(string storeID)
        {

        }

        public Response AddNewStore(string userID, string storeID, List<string> newStoreDetails)
        {

        }

        public Response RemoveStore(string userID, string storeID)
        {

        }

        public Response GetPurchaseHistoryOfTheStore(string userID, string storeID)
        {

        }

        public Response TransferFoundership(string userID, string storeID, string newFounderID)
        {

        }

        public Response AddStorePurchasePolicy(string userID, string storeID, Purchase_Policy newPolicy)
        {

        }

        public Response RemoveStorePurchasePolicy(string userID, string storeID, String policyID)
        {

        }

        public Response AddStorePurchaseStrategy(string userID, string storeID, Purchase_Strategy newStrategy)
        {

        }

        public Response RemoveStorePurchaseStrategy(string userID, string storeID, String strategyID)
        {

        }

        // ====================== END of Store methods ===============================
        // ===========================================================================





        // =======================================================================
        // ====================== EMployee methods ===============================
        public Response AddEmployeePermission(string userID, string storeID, string employeeID, Permission newP)
        {

        }

        public Response RemoveEmployeePermission(string userID, string storeID, string employeeID, Permission permToRemove)
        {

        }

        public Response AssignNewOwner(string userID, string storeID, string newOwnerID)
        {

        }

        public Response AssignNewManager(string userID, string storeID, string newManagerID)
        {

        }

        public Response GetManagersOfTheStore(string userID, string storeID)
        {

        }

        public Response GetOwnersOfTheStore(string userID, string storeID)
        {

        }

        public Response ManageEmployeePermissions(string userID, string storeID, string employeeID, List<Permission> perms)
        {

        }

        // ====================== END of Employee methods ===============================
        // ==============================================================================





        // ======================================================================
        // ====================== Product methods ===============================

        public Response GetProductsFromStore(string storeID)
        {

        }

        public Response AddProductToStore(string storeID, string usertID, List<String> productProperties)
        {

        }

        public Response RemoveProductFromStore(string storeID, string userID, string productID, List<string> productProperties)
        {

        }

        public Response AddProductComment(string userID, string productID, string comment, double rating)
        {

        }

        public Response ReserveProduct(ItemDTO reservedProduct)
        {

        }

        public Response LetGoProduct(ItemDTO reservedProduct)
        {

        }

        public Response SearchProductByKeyword(string keyword)
        {

        }

        public Response SearchProductByName(string name)
        {

        }

        public Response SearchProductByCategory(Category category)
        {

        }

        public Response ChangeProductName(string userID, string productID, string name)
        {

        }

        public Response ChangeProductDescription(string userID, string productID, string description)
        {

        }

        public Response ChangeProductPrice(string userID, string productID, double price)
        {

        }

        public Response ChangeProductRating(string userID, string productID, double rating)
        {

        }

        public Response ChangeProductQuantity(string userID, string productID, int quantity)
        {

        }

        public Response ChangeProductWeight(string userID, string productID, double weight)
        {

        }

        public Response ChangeProductSale(string userID, string productID, double sale)
        {

        }

        public Response ChangeProductTimesBought(string userID, string productID, int times)
        {

        }

        public Response ChangeProductProductCategory(string userID, string productID, Category category)
        {

        }

        public Response ChangeProductDimenssions(string userID, string productID, Array<double> dims)
        {

        }

        public Response AddProductPurchasePolicy(string userID, string storeID, string productID, Purchase_Policy newPolicy)
        {

        }

        public Response RemoveProductPurchasePolicy(string userID, string storeID, string productID, String policyID)
        {

        }

        public Response AddProductPurchaseStrategy(string userID, string storeID, string productID, Purchase_Strategy newStrategy)
        {

        }

        public Response RemoveProductPurchaseStrategy(string userID, string storeID, string productID, String strategyID)
        {

        }




        // ======================================================
        // ======================== TODO ========================

        public Response apply_purchase_policy() { } // I dont quite understand what its supposed to do....

        // ======================== END of TODO ========================
        // =============================================================

    }
}



