using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public partial class MyShopConText : DbContext
    {
        public MyShopConText()
        {
        }

        public MyShopConText(DbContextOptions<MyShopConText> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source=.; " +
                "Trusted_Connection=Yes; " +
                "Initial Catalog=Phone; " +
                "TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CategoryName)
                    .HasColumnType("ntext")
                    .HasColumnName("Name");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("Id");
                //entity.Property(e => e.Category).HasColumnName("Category");
                entity.Property(e => e.ProductName)
                    .HasColumnType("ntext")
                    .HasColumnName("Name");
                entity.Property(e => e.Quantity).HasColumnName("Quantity");
                entity.Property(e => e.Price).HasColumnName("Price");
                //entity.HasOne(d => d.Category).WithMany(p => p.Products)
                //    .HasForeignKey(d => d.CategoryId)
                //    .OnDelete(DeleteBehavior.SetNull)
                //    .HasConstraintName("FK_Product_Category");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
