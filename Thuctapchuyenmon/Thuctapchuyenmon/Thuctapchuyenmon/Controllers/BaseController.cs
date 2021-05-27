using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;


namespace Thuctapchuyenmon.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (Admin )Session[Thuctapchuyenmon.ComMonStants.UserLogin];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(
               new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}