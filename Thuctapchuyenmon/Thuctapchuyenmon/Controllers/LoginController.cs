using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using Thuctapchuyenmon.Common;
using System.Security.Cryptography;
using System.Text;

namespace Thuctapchuyenmon.Controllers
{
    public class LoginController : Controller
    {
        // GET: 
        WebDBContext db = new WebDBContext();
        [HttpPost]
        public ActionResult Dangnhap(FormCollection f)
        {//lag qua
            //Kiemtra ten đăng nhap và mk
            string tenDn = f["username"].ToString();
            string mk = GetMD5(f["pass"].ToString());
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
           
        }//m lam entity dung ko cai dao dau t k tạo cái đó ok dợi tí
        public static string GetMD5(string text)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));

            }
            return sbHash.ToString();

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
                var encryptormk = GetMD5(ad.PassWord);
                ad.PassWord = encryptormk;
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