using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.DAL.DBModels
{
    public class TimerModel
    {
        [Key]
        public string TimerID { get; set; } // productID_<ocassionType>_timer   =>  <ocassionType> = lottery / auction .....
        public string MinutesToCount { get; set; }
        public string CreationTimeStamp { get; set; }
        public string FounderID { get; set; }
        public string ProductID { get; set; }


    }
}