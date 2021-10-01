using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DL.Entities
{
    public partial class ShoppingAppDBContext : DbContext
    {
        public ShoppingAppDBContext()
        {
        }

        public ShoppingAppDBContext(DbContextOptions<ShoppingAppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customerorder> Customerorders { get; set; }
        public virtual DbSet<Lineitem> Lineitems { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Phonenumber)
                    .HasName("PK__CUSTOMER__8F2B07B02283D058");

                entity.ToTable("CUSTOMERS");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PHONENUMBER");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Password1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD1");
            });

            modelBuilder.Entity<Customerorder>(entity =>
            {
                entity.ToTable("CUSTOMERORDER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Customerphone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMERPHONE");

                entity.Property(e => e.Orderdate)
                    .HasColumnType("date")
                    .HasColumnName("ORDERDATE");

                entity.Property(e => e.Storeid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STOREID");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("TOTAL");

                entity.HasOne(d => d.CustomerphoneNavigation)
                    .WithMany(p => p.Customerorders)
                    .HasForeignKey(d => d.Customerphone)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CUSTOMERO__CUSTO__282DF8C2");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Customerorders)
                    .HasForeignKey(d => d.Storeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CUSTOMERO__STORE__29221CFB");
            });

            modelBuilder.Entity<Lineitem>(entity =>
            {
                entity.ToTable("LINEITEM");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("COST");

                entity.Property(e => e.Orderid).HasColumnName("ORDERID");

                entity.Property(e => e.Productid).HasColumnName("PRODUCTID");

                entity.Property(e => e.Productname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTNAME");

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Lineitems)
                    .HasForeignKey(d => d.Orderid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LINEITEM__ORDERI__5224328E");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Lineitems)
                    .HasForeignKey(d => d.Productid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LINEITEM__PRODUC__531856C7");
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.HasKey(e => e.Phonenumber)
                    .HasName("PK__MANAGER__8F2B07B0F4E11754");

                entity.ToTable("MANAGER");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PHONENUMBER");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Password1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD1");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCTS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Stock).HasColumnName("STOCK");

                entity.Property(e => e.Storeid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STOREID");

                entity.Property(e => e.Unitprice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("UNITPRICE");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__STORE__75433F7ED1F81256");

                entity.ToTable("STORE");

                entity.Property(e => e.Number)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NUMBER");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.Managerphone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MANAGERPHONE");

                entity.Property(e => e.Zipcode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ZIPCODE");

                entity.HasOne(d => d.ManagerphoneNavigation)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.Managerphone)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__STORE__MANAGERPH__6754599E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
