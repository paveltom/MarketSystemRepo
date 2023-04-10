using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{

    public enum Method: int
    {
        #region Inventory Management
            AddNewProduct=0,
            RemoveProduct=1,
            EditProducr=2,
        #endregion


        #region Staff Management 
        AddStoreOwner = 3,
        AddStoreManager = 4,
        RemoveStoreManager = 5,
        SetPermissions = 6,
        #endregion

        #region Policies Management
        SetPurchasePolicyAtStore = 7,
        GetPurchasePolicyAtStore = 8,
        SetPurchaseStrategyAtStore=9,
        GetPurchaseStrategyAtStore=10,
        #endregion

        #region Information
        GetStorePurchaseHistory = 11
        #endregion
 }

    public class Permission
    {
        public Boolean[] Array_of_Permission { get; }


        public Permission()
            {
            Array_of_Permission = new Boolean[12];
            }
        public void SetPermission(int method, Boolean active)
        {
            Array_of_Permission[method] = active;
        }
        // the owner all his method is permitted  
        public void SetAllMethodesPermitted()
        {
         for(int i=0; i< Array_of_Permission.Length; i++)
            {
                Array_of_Permission[i] = true;
            }
         
        
        }
    }
}