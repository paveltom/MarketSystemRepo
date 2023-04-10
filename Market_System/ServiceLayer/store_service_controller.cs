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

        public void Purchase(string userID, List<ItemDTO> products)
        {

        }


        // ====================== END of General class methods ===============================
        // ===================================================================================



        // ====================================================================
        // ====================== Store methods ===============================

        public void ChangeStoreName(string userID, string storeID, string newName)
        {

        }

        public StoreDTO GetStore(string storeID)
        {

        }

        public void AddNewStore(string userID, string storeID, List<string> newStoreDetails)
        {

        }

        public void RemoveStore(string userID, string storeID)
        {

        }

        public List<string> GetPurchaseHistoryOfTheStore(string userID, string storeID)
        {

        }

        public void TransferFoundership(string userID, string storeID, string newFounderID)
        {

        }

        public void AddStorePurchasePolicy(string userID, string storeID, Purchase_Policy newPolicy)
        {

        }

        public void RemoveStorePurchasePolicy(string userID, string storeID, String policyID)
        {

        }

        public void AddStorePurchaseStrategy(string userID, string storeID, Purchase_Strategy newStrategy)
        {

        }

        public void RemoveStorePurchaseStrategy(string userID, string storeID, String strategyID)
        {

        }

        // ====================== END of Store methods ===============================
        // ===========================================================================





        // =======================================================================
        // ====================== EMployee methods ===============================
        public void AddEmployeePermission(string userID, string storeID, string employeeID, Permission newP)
        {

        }

        public void RemoveEmployeePermission(string userID, string storeID, string employeeID, Permission permToRemove)
        {

        }

        public void AssignNewOwner(string userID, string storeID, string newOwnerID)
        {

        }

        public void AssignNewManager(string userID, string storeID, string newManagerID)
        {

        }

        public List<string> GetManagersOfTheStore(string userID, string storeID)
        {

        }

        public List<string> GetOwnersOfTheStore(string userID, string storeID)
        {

        }

        public void ManageEmployeePermissions(string userID, string storeID, string employeeID, List<Permission> perms)
        {

        }

        // ====================== END of Employee methods ===============================
        // ==============================================================================





        // ======================================================================
        // ====================== Product methods ===============================

        public List<ItemDTO> GetProductsFromStore(string storeID)
        {

        }

        public void AddProductToStore(string storeID, string usertID, List<String> productProperties)
        {

        }

        public void RemoveProductFromStore(string storeID, string userID, string productID, List<string> productProperties)
        {

        }

        public void AddProductComment(string userID, string productID, string comment, double rating)
        {

        }

        public void ReserveProduct(ItemDTO reservedProduct)
        {

        }

        public Boolean LetGoProduct(ItemDTO reservedProduct)
        {

        }

        public List<ItemDTO> SearchProductByKeyword(string keyword)
        {

        }

        public List<ItemDTO> SearchProductByName(string name)
        {

        }

        public List<ItemDTO> SearchProductByCategory(Category category)
        {

        }

        public void ChangeProductName(string userID, string productID, string name)
        {

        }

        public void ChangeProductDescription(string userID, string productID, string description)
        {

        }

        public void ChangeProductPrice(string userID, string productID, double price)
        {

        }

        public void ChangeProductRating(string userID, string productID, double rating)
        {

        }

        public void ChangeProductQuantity(string userID, string productID, int quantity)
        {

        }

        public void ChangeProductWeight(string userID, string productID, double weight)
        {

        }

        public void ChangeProductSale(string userID, string productID, double sale)
        {

        }

        public void ChangeProductTimesBought(string userID, string productID, int times)
        {

        }

        public void ChangeProductProductCategory(string userID, string productID, Category category)
        {

        }

        public void ChangeProductDimenssions(string userID, string productID, Array<double> dims)
        {

        }

        public void AddProductPurchasePolicy(string userID, string storeID, string productID, Purchase_Policy newPolicy)
        {

        }

        public void RemoveProductPurchasePolicy(string userID, string storeID, string productID, String policyID)
        {

        }

        public void AddProductPurchaseStrategy(string userID, string storeID, string productID, Purchase_Strategy newStrategy)
        {

        }

        public void RemoveProductPurchaseStrategy(string userID, string storeID, string productID, String strategyID)
        {

        }




        // ======================================================
        // ======================== TODO ========================

        public void apply_purchase_policy() { }

        internal void destroy()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!TODO
        }

        // ======================== END of TODO ========================
        // =============================================================

    }
}



