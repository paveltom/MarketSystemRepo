using Market_System.DomainLayer.UserComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer
{
    public class MemberDTO
    {
        string username;
        bool isAdmin;
        string address;
        List<PurchaseHistoryObj> purchase_History; // TODO:: change this object to a pruchaseHistoryDTO object (?)
        Dictionary<string, string> stores_Working_In; //<store_ID, Role> ---> role per store
        public MemberDTO(string username)
        {
            this.username = username;
            this.address = "";
            isAdmin = false;
            purchase_History = new List<PurchaseHistoryObj>();
            stores_Working_In = new Dictionary<string, string>();
        }

        public MemberDTO(User user, bool isAdmin, List<PurchaseHistoryObj> purchaseHistoryObjs, Dictionary<string, string> stores_Working_In)
        {
            this.username = user.GetUsername();
            this.address = user.get_Address();
            this.isAdmin = isAdmin;
            this.purchase_History = purchaseHistoryObjs;
            this.stores_Working_In = stores_Working_In;
        }

        public bool IsAdmin()
        {
            return isAdmin;
        }
    }
}