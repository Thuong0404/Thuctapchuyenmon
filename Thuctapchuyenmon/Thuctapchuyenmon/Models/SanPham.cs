namespace Thuctapchuyenmon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            CTDonHangs = new HashSet<CTDonHang>();
            CTGHs = new HashSet<CTGH>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Amount { get; set; }

        [StringLength(500)]
        public string Anh { get; set; }

        public int? Id_NhomSp { get; set; }

        public int? Id_NSX { get; set; }

        public decimal? Gia { get; set; }

        public int? SoLuongTon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDonHang> CTDonHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTGH> CTGHs { get; set; }

        public virtual NhaSanXuat NhaSanXuat { get; set; }

        public virtual NhomSP NhomSP { get; set; }
    }
}
