﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.User_Component
{
    //TODO:: Impelement this as a State.
    public class Member : User_State
    {
        public Member()
        {

        }

        public string GetUserState()
        {
            return "Member";
        }
    }
}
