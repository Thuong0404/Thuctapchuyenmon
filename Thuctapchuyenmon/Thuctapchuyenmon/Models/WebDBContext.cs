namespace Thuctapchuyenmon.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WebDBContext : DbContext
    {
        public WebDBContext()
            : base("name=WebDBContext3")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<CTDonHang> CTDonHangs { get; set; }
        public virtual DbSet<CTGH> CTGHs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }
        public virtual DbSet<NhomSP> NhomSPs { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasMany(e => e.CTGHs)
                .WithRequired(e => e.Admin)
                .HasForeignKey(e => e.ID_User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Admin>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.Admin)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<CTDonHang>()
                .Property(e => e.DonGia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.Thanhtien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DonHang>()
                .HasMany(e => e.CTDonHangs)
                .WithOptional(e => e.DonHang)
                .HasForeignKey(e => e.ID_DH);

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
                .HasMany(e => e.CTDonHangs)
                .WithOptional(e => e.SanPham)
                .HasForeignKey(e => e.ID_SP);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.CTGHs)
                .WithRequired(e => e.SanPham)
                .HasForeignKey(e => e.ID_SP)
                .WillCascadeOnDelete(false);
        }
    }
}
