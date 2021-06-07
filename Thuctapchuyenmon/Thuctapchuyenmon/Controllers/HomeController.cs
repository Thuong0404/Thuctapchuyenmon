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
       

        public ActionResult Index()
        {
           
            Admin ad = (Admin)Session["Taikhoan"];
            ViewBag.Ten = ad.UserName;
            return View();
        }
        public ActionResult GioiThieu()
        {
            Admin ad = (Admin)Session["Taikhoan"];
            ViewBag.Ten = ad.UserName;
            //List<GioHang> lisGiohang = (List<GioHang>)Session["GioHang"];
            //ViewBag.Tinhtongsl = lisGiohang.Sum(c => c.SoLuong);
            //ViewBag.Tinhtonhtien = lisGiohang.Sum(c => c.ThanhTien);



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

            //Admin ad = (Admin)Session["Taikhoan"];
            //ViewBag.Ten = ad.UserName;

            //List<GioHang> lisGiohang = (List<GioHang>)Session["GioHang"];
            //ViewBag.Tinhtongsl = lisGiohang.Sum(c => c.SoLuong);
            //ViewBag.Tinhtonhtien = lisGiohang.Sum(c => c.ThanhTien);

            var listSp = (from c in db.SanPhams where c.Id_NhomSp == id select c).ToList();

            return View(listSp);
        }
        //public ActionResult Admin( id)
        //{
        //}

        public ActionResult TimKiem(string tukhoa)
        {
            var listsp = db.SanPhams.Where(c => c.Name.Contains(tukhoa));


            return View(listsp.OrderBy(c => c.Name));
        }
        public ActionResult layoutAdmin()
        {


            return View();
        }
       
        
    }
}