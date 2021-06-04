namespace Thuctapchuyenmon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            CTDonHangs = new HashSet<CTDonHang>();
        }

        public int ID { get; set; }

        public int? UserID { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }

        [StringLength(500)]
        public string UserAddress { get; set; }

        [StringLength(20)]
        public string UserPhone { get; set; }

        public decimal? Thanhtien { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDat { get; set; }

        public bool? TinhTrangDonhang { get; set; }

        public bool? TrinhTrangGiaohang { get; set; }

        public virtual Admin Admin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDonHang> CTDonHangs { get; set; }
    }
}
