using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Thuctapchuyenmon.Models;


namespace Thuctapchuyenmon.Controllers
{
    public class GioHangController : Controller
    {
        WebDBContext db = new WebDBContext();



        // GET: GioHang

        // them vao giỏ hàng
        public List<GioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang == null)
            {
                //Session["GioHang"] chưa tồn tại thì tạo ra list gio hang 
                listgiohang = new List<GioHang>();
                Session["GioHang"] = listgiohang;
                return listgiohang;

            }
            return listgiohang;
        }
        //public List<GioHang> LayHang(CTGH cTGH)
        //{
        //    List<GioHang> lisGiohang = Session["GioHang"] as List<GioHang>;
        //    if (Session["Taikhoan"] != null && lisGiohang.Count>0) {
        //        cTGH.Id_SP=lisGiohang(c=>c.M)


        //    }
        //}

        //Them giỏ hàng
        public ActionResult ThemvaoGioHang(int MaSp, string url)
        {
            //https://localhost:44391/Home 
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == MaSp);
            if (sp == null)
            {
                //đường dẫn bị lỗi
                Response.StatusCode = 404;
                return null;
            }
            //lây giỏ hàng xem có tồn tại k
            List<GioHang> listgiohang = LayGioHang();
            //1 sp đâ tồn tại
            GioHang sp1 = null;
            if (listgiohang.Count > 0)
            {
                sp1 = listgiohang.SingleOrDefault(c => c.MaSP == MaSp);

            }

            if (sp1 != null)
            {
                // co nghia no bao loi bi null so luong mà so luong cua m là kieu int okey chua
                if (sp.SoLuongTon < sp1.SoLuong)
                {
                    return View("ThongBao");
                }
                else
                {
                    sp1.SoLuong++;
                    sp1.ThanhTien = sp1.SoLuong * sp1.DonGia;
                    return View();
                }

            }

            GioHang gioHang = new GioHang(MaSp);
            listgiohang.Add(gioHang);
            return RedirectToAction("XemGioHang");

        }
        // tong sl
        public double TongSoLuong()
        {
            //Layay giỏ hàng
            List<GioHang> lisGiohang = Session["GioHang"] as List<GioHang>;
            if (lisGiohang == null)
            {
                return 0;
            }
            else
            {
                return lisGiohang.Sum(c => c.SoLuong);
            }
        }
        //tong tien
        public decimal tongtien()
        {
            List<GioHang> lisGiohang = Session["GioHang"] as List<GioHang>;
            if (lisGiohang == null)
            {
                return 0;
            }
            else
            {
                return lisGiohang.Sum(c => c.ThanhTien);
            }
        }
        public ActionResult XemGioHang()
        {
            if (TongSoLuong() == 0)
            {
                ViewBag.Tinhtongsl = 0;
                ViewBag.Tinhtonhtien = 0;
                return View();
            }
            else
            {
                ViewBag.Tinhtongsl = TongSoLuong();
                ViewBag.Tinhtonhtien = tongtien();

            }
            return View();

        }
        public ActionResult GioHang()
        {
            ViewBag.Tinhtongsl = TongSoLuong();
            ViewBag.Tinhtonhtien = tongtien();
            Admin ad = new Admin();
            try {
               ad = (Admin)Session["Taikhoan"];

            } catch {
                ad = new Admin();
            }
          
            
            //ViewBag.Ten = ad.UserName;
            List<GioHang> listgiohang = LayGioHang();
            ViewBag.listgiohang = listgiohang;

            return View(ad);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSp)
        {

            //    Admin ad = (Admin)Session["Taikhoan"];
            //    ViewBag.Ten = ad.UserName;
            //kt gio hang
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            //kt sp co ton tai k
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == MaSp);
            if (sp == null)
            {
                //đường dẫn bị lỗi
                Response.StatusCode = 404;
                return null;
            }
            //Lay list hang tư session
            List<GioHang> listgiohang = LayGioHang();
            //kt sp co trong gio hang hay chua
            GioHang sp2 = listgiohang.SingleOrDefault(c => c.MaSP == MaSp);
            if (sp2 == null)
            {
                return RedirectToAction("Trangchu", "Home");
            }
            //ViewBag.GioHang = listgiohang;S
            ViewBag.Giohang = listgiohang;
     
            return View(sp2);
        }
        //su li viec cap nhap
        [HttpPost]
        public ActionResult UpdateGioHang(GioHang gioHang)
        {
            SanPham sp3 = db.SanPhams.SingleOrDefault(c => c.ID == gioHang.MaSP);
            if (sp3.SoLuongTon < gioHang.SoLuong)
            {
                return View("ThongBao");
            }
            //cap nhap gio hang
            List<GioHang> listgiohang = LayGioHang();
            GioHang updateGiohang = listgiohang.Find(c => c.MaSP == gioHang.MaSP);
            updateGiohang.SoLuong = gioHang.SoLuong;
            updateGiohang.ThanhTien = updateGiohang.SoLuong * updateGiohang.DonGia;
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int MaSp)
        {
            //Admin ad = (Admin)Session["Taikhoan"];
            //ViewBag.Ten = ad.UserName;
            //kt gio hang
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            //kt sp co ton tai k
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == MaSp);
            if (sp == null)
            {
                //đường dẫn bị lỗi
                Response.StatusCode = 404;
                return null;
            }
            //Lay list hang tư session
            List<GioHang> listgiohang = LayGioHang();
            //kt sp co trong gio hang hay chua
            GioHang sp2 = listgiohang.SingleOrDefault(c => c.MaSP == MaSp);
            if (sp2 == null)
            {
                return RedirectToAction("Trangchu", "Home");
            }
            //xoa sp
            listgiohang.Remove(sp2);
            return RedirectToAction("GioHang");
        }
     
        public ActionResult Dathang(Admin mode)
        {

                Admin ad = mode;
                ViewBag.Ten = ad.UserName;
                // kt gio hang
                if (Session["GioHang"] == null)
                {
                    return RedirectToAction("TrangChu", "Home");
                }
                ////them thong tin ng sd vao bang admin
                Admin admin = new Admin();
                if (Session["Taikhoan"] == null) 

                { //khach hang chuwa co tk hoac chua dang nhap, chua co tk
                    admin = new Admin();
                    admin = ad;
                    db.Admins.Add(ad);
                    db.SaveChanges();
                }
                else
                {

                    admin = (Admin)Session["Taikhoan"];
                
                    db.SaveChanges();
                }
            List<GioHang> listgiohang = LayGioHang();
            //1 sp đâ tồn tại
           
            //Them dơn ĐH
            DonHang donHang = new DonHang();
                donHang.NgayDat = DateTime.Now;
                donHang.UserID = admin.ID;
               donHang.UserName = admin.UserName;
                donHang.UserAddress = admin.Address;
                donHang.UserPhone = admin.Phone;
                donHang.TinhTrangDonhang = false;
                donHang.TrinhTrangGiaohang = false;
                 donHang.Thanhtien = tongtien();


            db.DonHangs.Add(donHang);
                db.SaveChanges();
                //them ctdh
             
                foreach (var item in listgiohang)
                {
                    CTDonHang ctdh = new CTDonHang();
                    ctdh.ID_DH = donHang.ID;
                    ctdh.ID_SP = item.MaSP;
                    ctdh.TenSP = item.TenSP;
                    ctdh.SoLuong = item.SoLuong;
                    ctdh.DonGia = item.DonGia;
                    db.CTDonHangs.Add(ctdh);
                }
                db.SaveChanges();
                Session["GioHang"] = null;
                return RedirectToAction("GioHang");

            
         
        }
           
        }

    }
