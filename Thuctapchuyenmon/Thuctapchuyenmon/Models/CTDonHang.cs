namespace Thuctapchuyenmon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTDonHang")]
    public partial class CTDonHang
    {
        [StringLength(500)]
        public string TenSP { get; set; }

        public int ID { get; set; }

        public int? ID_SP { get; set; }

        public int? SoLuong { get; set; }

        public decimal? DonGia { get; set; }

        public int? ID_DH { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
