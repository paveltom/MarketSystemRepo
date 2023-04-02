using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Market_System.Domain_Layer.Store_Component
{
    //TODO:: Add an Enum class here called: <Permission> - all of the available permissions that can be assigned to managers (by store owners).
    //TODO:: need to enforce that only the assginging owner may change the manager's permissions - or fire him. 


    public enum Permission { Administrator, Founder, Owner, Manager, Member, Guest}

    public class Roles
    {
        private List<Role> roles;
        
        public List<Role> Roles
        {
            get { return this.roles; }
            set { this.roles = value; }
        }
        public Roles()
        {
            this.roles = new List<Role>();
        }
    }
}

     
