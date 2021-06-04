using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;
using System.Net.Mail;
using System.Net;

namespace Thuctapchuyenmon.Controllers
{
    public class QuanLiDonHangController : Controller
    {
        // GET: QuanLiDonHang
        WebDBContext db = new WebDBContext();
        public ActionResult ChuaDuyetChuaGiao()
        {
            var list = db.DonHangs.Where(c => c.TinhTrangDonhang == false && c.TinhTrangDonhang == false).OrderBy(c => c.NgayDat);
            return View(list);
        }
        public ActionResult DaDuyetChuaGiao()
        {
            var list1 = db.DonHangs.Where(c => c.TrinhTrangGiaohang == false && c.TinhTrangDonhang == true).OrderBy(c => c.NgayDat);
            return View(list1);

        }
        public ActionResult DaDuyetdaGiao()
        {

            var list2 = db.DonHangs.Where(c => c.TrinhTrangGiaohang == true && c.TinhTrangDonhang == true).OrderBy(c => c.NgayDat);
            return View(list2);

        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            DonHang donHang = db.DonHangs.SingleOrDefault(c => c.ID == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            var listCTDH = db.CTDonHangs.Where(c => c.ID_DH == id);
            ViewBag.ListCTDH = listCTDH;
            return View(donHang);

        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonHang donHang)
        {
           // var list3 = db.Admins.Where(c => c.Email != null);
            DonHang dh = db.DonHangs.SingleOrDefault(c => c.ID == donHang.ID);
            dh.TinhTrangDonhang = donHang.TinhTrangDonhang;
            dh.TrinhTrangGiaohang = donHang.TrinhTrangGiaohang;
            db.SaveChanges();
            var listCTDH = db.CTDonHangs.Where(c => c.ID_DH == dh.ID);
            var a = (from t in db.DonHangs where t.ID == donHang.ID select t).SingleOrDefault();
            string email = a.Admin.Email;
            ViewBag.ListCTDH = listCTDH;
         
           
            SendEmail(email, "Xác nhận đơn hàng từ Big Shope ", "Đơn  hàng của bạn ở Big Shope đã được duyệt và sẽ được giao trong đến  trong 2- 3 ngày tới. Vui lòng giữ điện thoại để shipper có thể liên lạc được với bạn . Big shoppe xin cảm ơn");
            return View(dh);
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
        public ActionResult ThongKe()
        {
            return View();
        }
        }
}