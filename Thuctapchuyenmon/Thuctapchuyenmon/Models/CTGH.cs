namespace Thuctapchuyenmon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTGH")]
    public partial class CTGH
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_SP { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_User { get; set; }

        public int? SoLuong { get; set; }

        public double? DonGia { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
