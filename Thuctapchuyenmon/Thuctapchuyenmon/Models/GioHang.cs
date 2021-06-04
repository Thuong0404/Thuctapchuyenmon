using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thuctapchuyenmon.Models
{
    public class GioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HAnh { get; set; }
      

        public GioHang(int MaSp)
        {
            using (WebDBContext db = new WebDBContext())
            {
                this.MaSP = MaSp;
                SanPham sp = db.SanPhams.Single(c => c.ID == MaSp);
                this.TenSP = sp.Name;
                this.HAnh = sp.Anh;
                this.DonGia = sp.Gia.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;






            }
        }
        public GioHang(int MaSp, int sl)
        {
            using (WebDBContext db = new WebDBContext())
            {
                this.MaSP = MaSp;
                SanPham sp = db.SanPhams.Single(c => c.ID == MaSp);
                this.TenSP = sp.Name;
                this.HAnh = sp.Anh;
                this.DonGia = sp.Gia.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;






            }
        }
            public GioHang()
        {

        }

    }
}
