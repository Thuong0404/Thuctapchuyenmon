using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;
namespace Thuctapchuyenmon.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Admin ad = (Admin)Session["Taikhoan"];
            if (Session["Taikhoan"] != null)
            {
                ViewBag.Ten = ad.UserName;
            }
            return View();
        }
    }
}