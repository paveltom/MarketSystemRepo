using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class ContractModel
    {
        [Key]
        public string ContractID { get; set; } // newOwner_ID + "_" + storeID
        public string NewOwnerID { get; set; }
        public string StoreID { get; set; }
        public string SuggestorID { get; set; }
        public string HaveToAccept { get; set; } // owner1_owner2_... - all of the owners who still didn't accept

        

    }
}