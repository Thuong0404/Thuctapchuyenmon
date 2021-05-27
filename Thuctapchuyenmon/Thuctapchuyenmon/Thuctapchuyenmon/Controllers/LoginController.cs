using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

namespace Thuctapchuyenmon.Controllers
{
    public class LoginController : Controller
    {
        // GET: 
        WebDBContext db = new WebDBContext();
        [HttpPost]
        public ActionResult Dangnhap(FormCollection f)
        {
            //Kiemtra ten đăng nhap và mk
            string tenDn = f["username"].ToString();
            string mk = f["pass"].ToString();
            Admin ad = db.Admins.SingleOrDefault(n => n.UserName == tenDn && n.PassWord == mk);
            if (ad != null)
            {
                Session["Taikhoan"] = ad;
               
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = "Login failed";

                return RedirectToAction("Dangnhap");
            }
           
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpGet]
        
        public ActionResult TaoTK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TaoTK(Admin ad)
        {
            //kt capcha hơp le
            if (this.IsCaptchaValid("Mã sẳn sàng"))
            {
                //them vao csdl
                db.Admins.Add(ad);
                db.SaveChanges();
                Session["Taikhoan"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewBag.Thongbao = "Câu trả lời không đúng mời nhập lại!!!!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult TaoTK1()
        {
            return View();
        }
    }
}