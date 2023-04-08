using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.UserComponent
{
    //TODO:: Impelement this as a State.
    public class Owner : User_State
    {
        public string GetUserState()
        {
            return "Owner";
        }
    }
}
