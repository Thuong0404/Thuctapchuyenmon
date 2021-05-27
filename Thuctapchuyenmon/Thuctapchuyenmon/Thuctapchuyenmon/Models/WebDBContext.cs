namespace Thuctapchuyenmon.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WebDBContext : DbContext
    {
        public WebDBContext()
            : base("name=WebDBContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<CTDH> CTDHs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }
        public virtual DbSet<NhomSP> NhomSPs { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.Admin)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.Thanhtien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.Tongien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DonHang>()
                .HasMany(e => e.CTDHs)
                .WithRequired(e => e.DonHang)
                .HasForeignKey(e => e.Id_DH)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NhaSanXuat>()
                .HasMany(e => e.SanPhams)
                .WithOptional(e => e.NhaSanXuat)
                .HasForeignKey(e => e.Id_NSX);

            modelBuilder.Entity<NhomSP>()
                .HasMany(e => e.SanPhams)
                .WithOptional(e => e.NhomSP)
                .HasForeignKey(e => e.Id_NhomSp);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.Gia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.CTDHs)
                .WithRequired(e => e.SanPham)
                .HasForeignKey(e => e.Id_SP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.SanPham)
                .HasForeignKey(e => e.Id_SP);
        }
    }
}
