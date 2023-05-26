using Market_System.DomainLayer.StoreComponent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class StoreModel
    {
        // everything that is other objects are defined as VIRTUAL

        [Key]
        public string StoreID { get; set; } // key in DB

        public string Name { get; set; }
        
        public string founderID { get; set; } //founder's userID

        public bool temporaryClosed { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }



    // those fields will be separated models
    /*
    public ConcurrentDictionary<string, Purchase_Policy> productDefaultPolicies; // passed to every new added product
    public ConcurrentDictionary<string, Purchase_Strategy> productDefaultStrategies; // passed to every new added product
    public ConcurrentDictionary<string, Purchase_Policy> storePolicies;
    public ConcurrentDictionary<string, Purchase_Strategy> storeStrategies;
    private Employees employees;
    private List<string> products;
    */
}
}