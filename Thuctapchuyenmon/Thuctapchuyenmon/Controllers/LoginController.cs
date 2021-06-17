using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Thuctapchuyenmon.Controllers
{
    public class LoginController : Controller
    {

        // GET: 
        WebDBContext db = new WebDBContext();
        [HttpPost]
        public ActionResult Dangnhap(string username, string pass)
        {//lag qua
            //Kiemtra ten đăng nhap và mk
                string tenDn = username.ToString();
            string mk = GetMD5(pass.ToString());
            Admin ad = db.Admins.SingleOrDefault(n => n.UserName == tenDn && n.PassWord == mk);
            if (ad != null)
            {
                Session["Taikhoan"] = ad;

                if (ad.ID == 30)
                {
                    return RedirectToAction("Index", "Admin");


                }
                return RedirectToAction("Index", "Home");
            }
           else  if(ad==null)
            {
                ViewBag.Error = "Mật khẩu hoặc tên đăng nhập không đúng";
               
            }
            return View();
            }
           
    
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

            if (Session["Taikhoan"] == null)
            {
                ViewBag.ThongBao = "Bạn đã có tài khoản ";
            }
            else
            {
                //kt capcha hơp le
                if (this.IsCaptchaValid("Mã sẳn sàng"))
                {
                    //them vao csdl
                    if (ad.UserName.Count() > 0)
                    {
                        ViewBag.ten = "Tên đăng nhập này đã có trong hệ thống";
                        return View();
                    }
                    else
                    {
                        var encryptormk = GetMD5(ad.PassWord);
                        ad.PassWord = encryptormk;
                        db.Admins.Add(ad);
                        db.SaveChanges();
                        Session["Taikhoan"] = ad;
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {

                    ViewBag.Thongbao = "Câu trả lời không đúng mời nhập lại!!!!";
                }
            }
                return View();
            
        }
        [HttpGet]
        public ActionResult TaoTK1()
        {
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Trangchu", "Home");
        }

        public void SendEmail(string address, string subject, string message)
        {
            string email = "tmooquiz40@gmail.com";
            string password = "0353573467";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
        public string CreateLostPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijk0123456789mnopqrstuvwxyz";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public ActionResult Quenmk(string inputfirstname, string inputEmailAddress)
        {


            return View();

        }
        public JsonResult Guimail(string inputfirstname, string inputEmailAddress)
        {

            var a = (from t in db.Admins where t.UserName == inputfirstname && t.Email == inputEmailAddress select t).SingleOrDefault();

            if (a != null)
            {
                string ma = CreateLostPassword(4);
                SendEmail(inputEmailAddress, "Thay đối mật khẩu ", "Mật khẩu mới của bạn là: " + ma);
                Session["Ma"] = ma;
                return Json(new
                {
                    status = true

                }, JsonRequestBehavior.AllowGet); ;
            }

            return Json(new
            {
                status = false

            }, JsonRequestBehavior.AllowGet) ; ;

        }


        public JsonResult kiemtramk(string inputfirstname, string inputEmailAddress, string maxn)
        {
            var a = (from t in db.Admins where t.UserName == inputfirstname && t.Email == inputEmailAddress select t).SingleOrDefault();
            string ma = (string)Session["Ma"];
            if (ma.Equals(maxn))
            {
                if (ma != maxn)
                {
                    ViewBag.thongbao = "Mật khẩu sai mời nhập lại";
                }
                else
                {
                    Admin taiKhoan = db.Admins.Find(a.ID);
                    taiKhoan.PassWord = GetMD5(ma);
                    db.SaveChanges();

                    Session["Ma"] = "";

                    return Json(new
                    {
                        status = true,
                       
                    }, JsonRequestBehavior.AllowGet) ;
                }
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }

        }
    }