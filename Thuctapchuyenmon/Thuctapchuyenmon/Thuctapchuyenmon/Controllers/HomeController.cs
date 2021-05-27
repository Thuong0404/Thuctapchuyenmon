using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;

namespace Thuctapchuyenmon.Controllers
{
    public class HomeController : Controller
    {
  

        public ActionResult Trangchu()
        {
            

            return View();
        }
        public ActionResult Giohang()
        {
           
            return View();
        }
       

        public ActionResult Index()
        {
            

            return View();
        }
        public ActionResult GioiThieu()
        {
            Admin ad = (Admin)Session["Taikhoan"];
            ViewBag.Ten = ad.UserName;
            return View();
        }

        WebDBContext db = new WebDBContext();

        public ActionResult Menu()
        {

            var listSp = new WebDBContext().NhomSPs.ToList();
          
            return View(listSp);
        }
        public ActionResult SanPham(int id)
        {
            List<GioHang> lisGiohang = Session["GioHang"] as List<GioHang>;
            ViewBag.Tinhtongsl = lisGiohang.TongSoLuong(); 
           Admin ad = (Admin)Session["Taikhoan"];
            ViewBag.Ten = ad.UserName;
            var listSp = (from c in db.SanPhams where c.Id_NhomSp == id select c).ToList();

            return View(listSp);
        }
        //public ActionResult Admin( id)
        //{
        //}
    }
}