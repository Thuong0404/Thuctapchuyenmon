using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Thuctapchuyenmon
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["Soluongtruycap"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["Soluongtruycap"] = (int)Application["Soluongtruycap"] + 1;
            Application.UnLock();
            Session["User_admin"] = "";
            Session["User_ID"] = "";
            Session["UserName"] = "";
            Session["PassWord"] = "";
        }
    }
}
