using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Market_System
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["totalApplications"] = 0;
            Application["totalUserSessions"] = 0;
            Application["totalApplications"] = (int)Application["totalApplications"] + 1;
        }

        void Session_Start(object sender, EventArgs e)
        {
            Application["TotalUserSessions"] = (int)Application["totalUserSessions"] + 1;

        }

        void Session_End(object sender, EventArgs e)
        {
            Application["TotalUserSessions"] = (int)Application["totalUserSessions"] - 1;

        }
    }
}