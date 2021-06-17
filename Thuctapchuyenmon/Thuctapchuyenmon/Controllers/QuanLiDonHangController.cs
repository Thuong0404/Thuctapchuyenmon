using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace Thuctapchuyenmon.Controllers
{
    public class QuanLiDonHangController : Controller
    {
        // GET: QuanLiDonHang
        WebDBContext db = new WebDBContext();
        public ActionResult ChuaDuyetChuaGiao()
        {
            ViewBag.Soluongtruycap = HttpContext.Application["Soluongtruycap"].ToString();
            ViewBag.tongdoanhthu = TKDoanhthu();
            ViewBag.tongdonhang = donhang();
            var list = db.DonHangs.Where(c => c.TinhTrangDonhang == false && c.TinhTrangDonhang == false).OrderBy(c => c.NgayDat);
            return View(list);
        }
        public ActionResult DaDuyetChuaGiao()
        {
            ViewBag.Soluongtruycap = HttpContext.Application["Soluongtruycap"].ToString();
            ViewBag.tongdoanhthu = TKDoanhthu();
            ViewBag.tongdonhang = donhang();
            var list1 = db.DonHangs.Where(c => c.TrinhTrangGiaohang == false && c.TinhTrangDonhang == true).OrderBy(c => c.NgayDat);
            return View(list1);

        }
        public ActionResult DaDuyetdaGiao()
        {
            ViewBag.Soluongtruycap = HttpContext.Application["Soluongtruycap"].ToString();
            ViewBag.tongdoanhthu = TKDoanhthu();
            ViewBag.tongdonhang = donhang();

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
            ViewBag.Soluongtruycap = HttpContext.Application["Soluongtruycap"].ToString();
            ViewBag.tongdoanhthu = TKDoanhthu();
            ViewBag.tongdonhang = donhang();
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

        public ActionResult tongsp(SanPham sp)
        {
            var listCTDH = db.CTDonHangs.Where(c => c.ID_SP == sp.ID);

            var nhom = db.NhomSPs.Where(c => c.ID == sp.Id_NhomSp);
            ViewBag.listCTDH = nhom;
            return View();
        }
        public ActionResult ThongKe()
        {
            ViewBag.Soluongtruycap = HttpContext.Application["Soluongtruycap"].ToString();
            ViewBag.tongdoanhthu = TKDoanhthu();
            ViewBag.tongdonhang = donhang();
            ViewBag.listson = son();
            ViewBag.listcn = chongngang();
            ViewBag.listtt = tt();
            ViewBag.listgoi = goi();
            ViewBag.listmat = maumat();
            ViewBag.listna = na();

            //ViewBag.tongtien = TKDoanhthungay();
            //ViewBag.tongdonhangngay = donhangngay();

            ViewBag.sl = son();
            ViewBag.sl1 = chongngang();
            ViewBag.sl2 = tt();
            ViewBag.sl3 = goi();
            ViewBag.sl4 = maumat();
            ViewBag.sl5 = na();
            ViewBag.sl6 = nen();
            ViewBag.sl7 = hoa();
            ViewBag.sl2 = mat();

            return View();
        }
        public decimal TKDoanhthu()
        {
            // decimal tongdoanhthu = decimal.Parse(db.DonHangs.Where(n=>n.NgayDat.Value.Month==thang &&n.NgayDat.Value.Year==nam).Sum(n => n.Thanhtien).Value.ToString());

            decimal tongdoanhthu = db.CTDonHangs.Sum(n => n.DonGia * n.SoLuong).Value;
            return tongdoanhthu;
        }

        public double donhang()
        {

            double tongdonhang = db.DonHangs.Count();
            return tongdonhang;

        }
        public int son()
        {

            var san = (from s in db.SanPhams where s.NhomSP.ID == 1 select s).Sum(s => s.Amount);
            ViewBag.sl = san;
            var list = (from t in db.CTDonHangs where t.SanPham.ID == t.ID_SP && t.SanPham.Id_NhomSp == 1 select t).Sum(t => t.SoLuong);
            ViewBag.listson = list;

            return san.Value;

        }
        public int chongngang()
        {
            var san1 = (from s in db.SanPhams where s.NhomSP.ID == 2 select s).Sum(s => s.Amount);
            ViewBag.sl1 = san1;
            var list2 = (from t in db.CTDonHangs where t.SanPham.ID == t.ID_SP && t.SanPham.Id_NhomSp == 2 select t).Sum(t => t.SoLuong);
            ViewBag.listcn = list2;

            return san1.Value;

        }
        public int tt()
        {
            var san2 = (from s in db.SanPhams where s.NhomSP.ID == 3 select s).Sum(s => s.Amount);
            ViewBag.sl2 = san2;
            var list3 = (from t in db.CTDonHangs where t.SanPham.ID == t.ID_SP && t.SanPham.Id_NhomSp == 3 select t).Sum(t => t.SoLuong);
            ViewBag.listtt = list3;
            return 0;

        }
        public int goi()
        {
            var san3 = (from s in db.SanPhams where s.NhomSP.ID == 4 select s).Sum(s => s.Amount);
            ViewBag.sl3 = san3;
            var list4 = (from t in db.CTDonHangs where t.SanPham.ID == t.ID_SP && t.SanPham.Id_NhomSp == 4 select t).Sum(t => t.SoLuong);
            ViewBag.listgoi = list4;
            return san3.Value;

        }
        public int maumat()
        {
            var san4 = (from s in db.SanPhams where s.NhomSP.ID == 5 select s).Sum(s => s.Amount);
            ViewBag.sl4 = san4;
            var list5 = (from t in db.CTDonHangs where t.SanPham.ID == t.ID_SP && t.SanPham.Id_NhomSp == 5 select t).Sum(t => t.SoLuong);
            ViewBag.listmat = list5;
            return san4.Value;

        }
        public int na()
        {
            var san5 = (from s in db.SanPhams where s.NhomSP.ID == 6 select s).Sum(s => s.Amount);
            ViewBag.sl5 = san5;
            var list6 = (from t in db.CTDonHangs where t.SanPham.ID == t.ID_SP && t.SanPham.Id_NhomSp == 6 select t).Sum(t => t.SoLuong);
            ViewBag.listna = list6;
            return san5.Value;

        }
        public int nen()
        {
            var san6 = (from s in db.SanPhams where s.NhomSP.ID == 7 select s).Sum(s => s.Amount);
            ViewBag.sl6 = san6;

            return san6.Value;

        }
        public int hoa()
        {
            var san7 = (from s in db.SanPhams where s.NhomSP.ID == 8 select s).Sum(s => s.Amount);
            ViewBag.sl7 = san7;

            return san7.Value;

        }
        public int mat()
        {
            var san8 = (from s in db.SanPhams where s.NhomSP.ID == 9 select s).Sum(s => s.Amount);
            ViewBag.sl8 = san8;

            return san8.Value;

        }

        public ActionResult Exdonhang()
        {
            var model = db.DonHangs.OrderBy(p => p.Thanhtien).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Exdonhang(DonHang dh)
        {
            
            var gv = new GridView();
            gv.DataSource = db.DonHangs
                .Select(r => new {
                    ID = r.ID,
                    UserID = r.UserID,
                    UserName=r.UserName,
                    UserAddress=r.UserAddress,
                    UserPhone = r.UserPhone,
                   NgayDat = r.NgayDat,
                    TinhTrangDonhang = r.TinhTrangDonhang,
                    TrinhTrangGiaohang = r.TrinhTrangGiaohang,
                    Thanhtien = r.Thanhtien
                })
                .OrderByDescending(p => p.Thanhtien)
                .ToList();
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition",
            // "attachment;filename=GridViewExport.xls");
            Response.Charset = "utf-8";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Danh sách đơn hàng.xls");
            //Mã hóa chữa sang UTF8
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                
                gv.Rows[i].Attributes.Add("class", "textmode");
            }
         
            //Add màu nền cho header của file excel
            gv.HeaderRow.BackColor = System.Drawing.Color.DarkBlue;
            //Màu chữ cho header của file excel
            gv.HeaderStyle.ForeColor = System.Drawing.Color.White;

            
           
            
            gv.HeaderRow.Cells[0].Text = "Mã đơn hàng";
            gv.HeaderRow.Cells[1].Text = "Mã khách hàng";
            gv.HeaderRow.Cells[2].Text = "Tên khách hàng";
            gv.HeaderRow.Cells[3].Text = "Địa chỉ khách hàng";
            gv.HeaderRow.Cells[4].Text = "Số điện thoai";
            gv.HeaderRow.Cells[5].Text = "Ngày đặt";
            gv.HeaderRow.Cells[6].Text = "Tình trạng đơn hàng";
            gv.HeaderRow.Cells[7].Text = "Tình trang giao hàng";
            gv.HeaderRow.Cells[8].Text = "Thành tiền";
            
            gv.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            var model = db.DonHangs.OrderBy(p => p.Thanhtien).ToList();
            return View("View", model);
        }


        public ActionResult Exkhachhang()
        {
            var model = db.Admins.OrderBy(p => p.UserName).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Exkhachhang(Admin ad)
        {
            var gv = new GridView();
            gv.DataSource = db.Admins
                .Select(r => new {
                    ID = r.ID,
                    UserName = r.UserName,
                    PassWord=r.PassWord,
                   Phone = r.Phone,
                    Address = r.Address,
                    Email = r.Email
                    
                })
                .OrderByDescending(p => p.UserName)
                .ToList();
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition",
            // "attachment;filename=GridViewExport.xls");
            Response.Charset = "utf-8";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Danh sách khách hàng.xls");
            //Mã hóa chữa sang UTF8
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                //Apply text style to each Row
                gv.Rows[i].Attributes.Add("class", "textmode");
            }
            //Add màu nền cho header của file excel
            gv.HeaderRow.BackColor = System.Drawing.Color.DarkBlue;
            //Màu chữ cho header của file excel
            gv.HeaderStyle.ForeColor = System.Drawing.Color.White;
          
            gv.HeaderRow.Cells[0].Text = "Mã khách hàng";
            gv.HeaderRow.Cells[1].Text = "Tên khách hàng";
            gv.HeaderRow.Cells[2].Text = "Mật khẩu";
            gv.HeaderRow.Cells[3].Text = "Số điện thoai";
            gv.HeaderRow.Cells[4].Text = "Địa chỉ";
            gv.HeaderRow.Cells[5].Text = "Email";
          

            gv.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            var model = db.Admins.OrderBy(p => p.UserName).ToList();
            return View("View", model);
        }
    }
    }
