using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.PaymentComponent;
using Market_System.DomainLayer.DeliveryComponent;

namespace Market_System.DomainLayer
{
    //TODO:: Implement as a Mediator.
    public class StoreDTO
    {
        public string StoreID { get; private set; }
        public string Name { get; private set; }
        public List<ItemDTO> AllProducts { get; private set; }
        public List<EmployeeDTO> owners { get; private set; }
        public List<EmployeeDTO> managers { get; private set; }
        public string FounderID { get; private set; }
        public List<string> DefaultPolicies { get; private set; }
        public List<string> DefaultStrategies { get; private set; }

        public StoreDTO(Store storeToCopy)
        {
            this.StoreID = storeToCopy.Store_ID;
            this.Name = storeToCopy.Name;
            this.FounderID = storeToCopy.founderID;
            this.AllProducts = storeToCopy.GetItems();
            this.managers = storeToCopy.GetManagersOfTheStore(FounderID);

        }


        public List<string> MarketManagerView()
        {
            throw new NotImplementedException();
        }

        public List<string> StoreManagerView()
        {
            throw new NotImplementedException();
        }

        public List<string> StoreOwnerView()
        {
            throw new NotImplementedException();
        }

        public List<string> StoreFounderView()
        {
            throw new NotImplementedException();
        }

        public List<string> UserView()
        {
            throw new NotImplementedException();
        }

    }
}
