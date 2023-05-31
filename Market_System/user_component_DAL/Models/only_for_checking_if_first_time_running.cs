using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market_System.user_component_DAL.Models
{
    
    public class only_for_checking_if_first_time_running
    {
        [Key]
        public int firsttimerunning { get; set; }

        public only_for_checking_if_first_time_running()
        {

        }
    }
}