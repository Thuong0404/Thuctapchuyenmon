using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thuctapchuyenmon.Models;

namespace Thuctapchuyenmon.Controllers
{
    public class QuanLisanphamController : Controller
    {
        // GET: QuanLisanpham
        WebDBContext db = new WebDBContext();
        public ActionResult QuanLisanpham()
        {
            return View(db.SanPhams);
        }
        public ActionResult QuanliUser()
        {

            return View(db.Admins);
        }
        [HttpGet]
        public ActionResult Themsp()
        {
            //load droplist
            ViewBag.Id_NSX = new SelectList(db.NhaSanXuats.OrderBy(c => c.Name), "ID", "Name");
            ViewBag.Id_NhomSp = new SelectList(db.NhomSPs.OrderBy(c => c.ID), "ID", "Name");



            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Themsp(SanPham sp, HttpPostedFileBase Anh)
        {
            if (Anh.ContentLength > 0)
            {
                var filename = Path.GetFileName(Anh.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), filename);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload("Hình ảnh đã tồn tại");
                    return View();
                }
                //chưa có thì thêm vào
                else
                {
                    Anh.SaveAs(path);
                    sp.Anh = filename;

                }
            }
                db.SanPhams.Add(sp);
                db.SaveChanges();
                return RedirectToAction("QuanLisanpham");
           
        }
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult uploadsp( int? Id)
        {
            ViewBag.Id_NSX = new SelectList(db.NhaSanXuats.OrderBy(c => c.Name), "ID", "Name");
            ViewBag.Id_NhomSp = new SelectList(db.NhomSPs.OrderBy(c => c.ID), "ID", "Name");
            if (Id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == Id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_NSX = new SelectList(db.NhaSanXuats.OrderBy(c => c.Name), "ID", "Name", sp.Id_NSX);
            ViewBag.Id_NhomSp = new SelectList(db.NhomSPs.OrderBy(c => c.ID), "ID", "Name",sp.Id_NhomSp);
            
            return View(sp);
        }
        
        [HttpPost]
        public ActionResult uploadsp(SanPham sp)
        {
            if(ModelState.IsValid)
            {
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuanLisanpham");
            }
            return View(sp);
        }
        [HttpGet]
            public ActionResult Deletesp(int? Id)
        {
            ViewBag.Id_NSX = new SelectList(db.NhaSanXuats.OrderBy(c => c.Name), "ID", "Name");
            ViewBag.Id_NhomSp = new SelectList(db.NhomSPs.OrderBy(c => c.ID), "ID", "Name");
            if (Id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == Id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_NSX = new SelectList(db.NhaSanXuats.OrderBy(c => c.Name), "ID", "Name", sp.Id_NSX);
            ViewBag.Id_NhomSp = new SelectList(db.NhomSPs.OrderBy(c => c.ID), "ID", "Name", sp.Id_NhomSp);

            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Deletesp(int Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);


            }
            SanPham sp = db.SanPhams.SingleOrDefault(c => c.ID == Id);
            if(sp == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("QuanLisanpham");
            
        }
        public ActionResult QLDMSP()
        {
            return View(db.NhomSPs);
        }
        [HttpGet]
        public ActionResult ThemDMSP()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemDMSP(NhomSP nhomSP, HttpPostedFileBase Anh)
        {
            if (Anh.ContentLength > 0)
            {
                var filename = Path.GetFileName(Anh.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), filename);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload("Hình ảnh đã tồn tại");
                    return View();
                }
                //chưa có thì thêm vào
                else
                {
                    Anh.SaveAs(path);
                    nhomSP.Anh = filename;

                }
            }
            db.NhomSPs.Add(nhomSP);
            db.SaveChanges();
            return RedirectToAction("QuanLisanpham");

        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult uploadDMSP(int? Id)
        {
           
            if (Id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
          NhomSP nhom = db.NhomSPs.SingleOrDefault(c => c.ID == Id);
            if (nhom == null)
            {
                return HttpNotFound();
            }
            

            return View(nhom);
        }

        [HttpPost]
        public ActionResult uploadDMSP(NhomSP nhom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhom).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLDMSP");
            }
            return View(nhom);
        }

        [HttpGet]
        public ActionResult DeleteDMSP(int? Id)
        {
            
            if (Id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            NhomSP nhom = db.NhomSPs.SingleOrDefault(c => c.ID == Id);
            if (nhom == null)
            {
                return HttpNotFound();
            }
            
            return View(nhom);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DeleteDMSP(int Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);


            }
            NhomSP nhom = db.NhomSPs.SingleOrDefault(c => c.ID == Id);
            if (nhom == null)
            {
                return HttpNotFound();
            }
            db.NhomSPs.Remove(nhom);
            db.SaveChanges();
            return RedirectToAction("QLDMSP");

        }




        public ActionResult QLNSX()
        {
            return View(db.NhaSanXuats);
        }
        [HttpGet]
        public ActionResult ThemNSX()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemNSX(NhaSanXuat nhaSanXuat)
        {  
            db.NhaSanXuats.Add(nhaSanXuat);
            db.SaveChanges();
            return RedirectToAction("QLNSX");

        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult uploadNSX(int? Id)
        {

            if (Id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.SingleOrDefault(c => c.ID == Id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }


            return View(nhaSanXuat);
        }

        [HttpPost]
        public ActionResult uploadNSX(NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaSanXuat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLNSX");
            }
            return View(nhaSanXuat);
        }

        [HttpGet]
        public ActionResult DeleteNSX(int? Id)
        {

            if (Id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.SingleOrDefault(c => c.ID == Id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }

            return View(nhaSanXuat);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DeleteNSX(int Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);


            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.SingleOrDefault(c => c.ID == Id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            db.NhaSanXuats.Remove(nhaSanXuat);
            db.SaveChanges();
            return RedirectToAction("QLNSX");

        }
       

        }
}
