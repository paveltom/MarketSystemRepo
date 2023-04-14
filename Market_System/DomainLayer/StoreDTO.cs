using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.PaymentComponent;
using Market_System.DomainLayer.DeliveryComponent;
using System.Collections.Concurrent;

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
            this.managers = GetEmployeesDTO(storeToCopy.GetManagersOfTheStore(FounderID));
            this.owners = GetEmployeesDTO(storeToCopy.GetOwnersOfTheStore(FounderID));
            this.DefaultPolicies = GetPolicies(storeToCopy.defaultPolicies);
            this.DefaultStrategies = GetStrategies(storeToCopy.defaultStrategies);
        }


        private List<string> GetPolicies(ConcurrentDictionary<string, Purchase_Policy> policies)
        {
            List<string> ret = new List<string>();
            foreach (Purchase_Policy p in policies.Values)
                ret.Add(p.ToString());
            return ret;
        }

        private List<string> GetStrategies(ConcurrentDictionary<string, Purchase_Strategy> policies)
        {
            List<string> ret = new List<string>();
            foreach (Purchase_Strategy p in policies.Values)
                ret.Add(p.ToString());
            return ret;
        }

        private List<EmployeeDTO> GetEmployeesDTO(List<string> employees)
        {
            List<EmployeeDTO> ret = new List<EmployeeDTO>();    
            foreach(string s in employees)
                ret.Add(new EmployeeDTO(s));
            return ret;
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
