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
       
        //Them giỏ hàng
        public ActionResult ThemvaoGioHang(int MaSp )
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
            Admin ad = (Admin)Session["Taikhoan"];
            List<GioHang> listgiohang = LayGioHang();
            //1 sp đâ tồn tại
            GioHang sp1 = null;
            if (Session["Taikhoan"]==null)
            {
                return RedirectToAction("Dangnhap", "Login");
            }
            else
            {
                if (listgiohang.Count > 0)
                {
                    sp1 = listgiohang.SingleOrDefault(c => c.MaSP == MaSp);

                }

                if (sp1 != null)
                {

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

            }

            GioHang gioHang = new GioHang(MaSp);
            listgiohang.Add(gioHang);
            CTGH cTGH = new CTGH();
            cTGH.ID_SP = gioHang.MaSP;
            cTGH.ID_User = ad.ID;
            cTGH.SoLuong = gioHang.SoLuong;
            db.CTGHs.Add(cTGH);
            db.SaveChanges();
            return RedirectToAction("GioHang");

        }
        // tong sl
        public double TongSoLuong()
        {
            //Layay giỏ hàng
            var ad = (Admin)Session["Taikhoan"];
            if (Session["Taikhoan"]!=null)
            {
                List<CTGH> cTGHs = db.CTGHs.Where(x => x.ID_User == ad.ID).ToList();
             
                if (cTGHs == null)
                {
                    return 0;
                }
                else
                {
                    return (Double)cTGHs.Sum(x => x.SoLuong);

                }
            } return 0;
         
        }
        //tong tien
        public decimal tongtien()
        {
            var ad = (Admin)Session["TaiKhoan"];
            
            if (Session["Taikhoan"] != null)
            {
                List<CTGH> listgiohang = db.CTGHs.Where(x => x.ID_User == ad.ID && x.ID_SP==x.SanPham.ID).ToList();
                
                if (listgiohang == null)
                {
                    return 0;
                }
                else
                {
                    return (decimal)(listgiohang.Sum(x => x.SoLuong * x.SanPham.Gia));
                }
            }return 0;
        }
        public ActionResult XemGioHang()
        {
            Admin ad = (Admin)Session["Taikhoan"];
            if (Session["Taikhoan"]!=null)
            {
              
                ViewBag.Ten = ad.UserName;
            }
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
            CTGH ct = new CTGH();
            List<SanPham> sp = new List<SanPham>();
            if(Session["Taikhoan"]!=null)
            {
                 ViewBag.Ten = ad.UserName;
                ViewBag.listgio = db.CTGHs.Where(x => x.ID_User == ad.ID).ToList();
               
                return View(ad);

            }
            ViewBag.listgio = new List<CTGH>();
            return View(ad);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSp)
        {

            Admin ad = (Admin)Session["Taikhoan"];
            ViewBag.Ten = ad.UserName;
          
            //kt sp co ton tai k
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == MaSp);
            if (sp == null)
            {
                //đường dẫn bị lỗi
                Response.StatusCode = 404;
                return null;
            }
            //Lay list hang tư session
            List<CTGH> listgiohang = db.CTGHs.Where(x => x.ID_User == ad.ID).ToList();
            //kt sp co trong gio hang hay chua
           CTGH sp2 =db.CTGHs.Where(x => x.ID_User == ad.ID && x.ID_SP == MaSp).ToList().Last();
            if (sp2 == null)
            {
                return RedirectToAction("Trangchu", "Home");
            }
            //ViewBag.GioHang = listgiohang;
            ViewBag.Giohang = listgiohang;
            GioHang DH=new GioHang();
            DH.SoLuong = int.Parse(sp2.SoLuong.ToString());
            DH.MaSP = sp2.ID_SP;
            return View(DH);
        }
        //su li viec cap nhap
        [HttpPost]
        public ActionResult UpdateGioHang(GioHang gioHang)
        {
            var ad = (Admin)Session["TaiKhoan"];
            SanPham sp3 = db.SanPhams.SingleOrDefault(c => c.ID == gioHang.MaSP);
            if (sp3.SoLuongTon < gioHang.SoLuong)
            {
                return View("ThongBao");
            }
            //cap nhap gio hang
          
            var sp2 = db.CTGHs.Where(x => x.ID_User == ad.ID && x.ID_SP == gioHang.MaSP).ToList().Last();
            sp2.SoLuong = gioHang.SoLuong;
            db.SaveChanges();
           
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int MaSp)
        {
            Admin ad = (Admin)Session["Taikhoan"];
            ViewBag.Ten = ad.UserName;
            
            ////kt sp co ton tai k
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == MaSp);
            if (sp == null)
            {
                //đường dẫn bị lỗi
                Response.StatusCode = 404;
                return null;
            }      
           var sp2 =db.CTGHs.Where(x => x.ID_User == ad.ID && x.ID_SP==MaSp ).ToList();
         
            if (sp2.Count==0)
            {
                return RedirectToAction("Trangchu", "Home");
            }
            else
            {
                foreach (var item in sp2)
                {
                    db.CTGHs.Remove(item);
                    db.SaveChanges();
                }
            }
            //xoa sp
         //   listgiohang.Remove(sp2);
            return RedirectToAction("GioHang");
        }
     
        public ActionResult Dathang(Admin mode)
        {

                Admin ad = mode;
                ViewBag.Ten = ad.UserName;
                // kt gio hang
               
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
            List<CTGH> listgiohang = db.CTGHs.Where(x => x.ID_SP == x.SanPham.ID).ToList();
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
      
            //them ctdh

            foreach (var item in listgiohang)
                {
                    CTDonHang ctdh = new CTDonHang();
                    ctdh.ID_DH = donHang.ID;
                    ctdh.ID_SP = item.ID_SP;
                    ctdh.TenSP = item.SanPham.Name;
                    ctdh.SoLuong = item.SoLuong;
                    ctdh.DonGia = item.SanPham.Gia;
                    db.CTDonHangs.Add(ctdh);

             

            }
            db.SaveChanges();

            // List<CTDonHang>ctdh1 = db.CTDonHangs.Where(c => c.ID_SP == c.SanPham.ID ).ToList();
            // SanPham sp = new SanPham();
            // sp.SoLuongTon = (sp.Amount - (int)(ctdh1.SoLuong);
            //db.SaveChanges();
            // db.SanPhams.RemovedbRange(lí);
            db.CTGHs.RemoveRange(listgiohang);
                db.SaveChanges();
            List<CTDonHang> ctdh1 = db.CTDonHangs.Where(c => c.ID_DH== c.DonHang.ID).ToList();
            foreach(var item in ctdh1)
            {
                SanPham sanPham =db.SanPhams.Where(c=>c.ID==item.ID_SP).FirstOrDefault();
                sanPham.SoLuongTon = sanPham.Amount - item.SoLuong;
                db.SaveChanges();
            }
          
            db.SaveChanges();
            return RedirectToAction("GioHang","GioHang");
            //Them ctgh
          
            
         
        }

           
        }

    }
