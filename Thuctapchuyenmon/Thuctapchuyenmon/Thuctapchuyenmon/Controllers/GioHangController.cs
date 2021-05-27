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
                List<GioHang> listGH = new List<GioHang>();
                Session["GioHang"] = listgiohang;
                return listgiohang;

            }
            return listgiohang;
        }
        //Them giỏ hàng
        public ActionResult ThemvaoGioHang(int MaSp, string url)
        {
            //kiểm tra sp có trong csdl không
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
            GioHang sp1 = listgiohang.SingleOrDefault(c => c.MaSP == MaSp);
            if (sp1 != null)
            {
                if (sp.Amount < sp1.SoLuong)
                {
                    return View("ThongBao");
                }
                sp1.SoLuong++;
                return Redirect(url);
            }
          
            GioHang gioHang = new GioHang(MaSp);
            listgiohang.Add(gioHang);
            return Redirect(url);


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
                ViewBag.Tinhtonhtien=0;
                return View();
            }
            else
            {
                ViewBag.Tinhtongsl = TongSoLuong();
                ViewBag.Tinhtonhtien = tongtien();
                return View();
            }
           
        }
      
    }
}