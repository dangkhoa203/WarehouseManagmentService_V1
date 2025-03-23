﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Model.Receipt;
namespace WarehouseInventoryManagementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>(entity =>
            entity.HasOne(e => e.ServiceRegistered)
                  .WithMany(s => s.Accounts)
                  .HasForeignKey(e => e.ServiceId)
            );
            builder.Entity<ProductGroup>(entity =>
            {
                entity.HasMany(g => g.Products)
                      .WithOne(p => p.ProductGroup)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<CustomerGroup>(entity =>
            {
                entity.HasMany(g => g.Customers)
                      .WithOne(p => p.CustomerGroup)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<VendorGroup>(entity =>
            {
                entity.HasMany(g => g.Vendors)
                      .WithOne(p => p.VendorGroup)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<VendorReplenishReceiptDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ReceiptId });

                entity.HasOne(d => d.ProductNav)
                    .WithMany(p => p.VendorReplenishReceiptDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_replenishdetail_product");

                entity.HasOne(d => d.ReceiptNav)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_replenishdetail_receipt");
            });
            builder.Entity<CustomerBuyReceiptDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ReceiptId });

                entity.HasOne(d => d.ProductNav)
                    .WithMany(p => p.CustomerBuyReceiptDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orderdetail_product");

                entity.HasOne(d => d.ReceiptNav)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orderdetail_receipt");
            });
            builder.Entity<Stock>()
                   .HasOne(s => s.ProductNav)
                   .WithOne(p => p.Stocks)
                   .HasForeignKey<Stock>(s => s.ProductId);

            builder.Entity<ReturnBuyForm>()
                   .HasOne(f => f.Receipt)
                   .WithOne(r => r.ReturnBuyForm)
                   .HasForeignKey<ReturnBuyForm>(f => f.ReceiptId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<ReturnReplenishForm>()
                   .HasOne(f => f.Receipt)
                   .WithOne(r => r.ReturnReplenishForm)
                   .HasForeignKey<ReturnReplenishForm>(f => f.ReceiptId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
            
            builder.Entity<StockExportForm>()
                   .HasOne(f => f.Receipt)
                   .WithOne(r => r.StockExportReport)
                   .HasForeignKey<StockExportForm>(f => f.ReceiptId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<StockImportForm>()
                   .HasOne(f => f.Receipt)
                   .WithOne(r => r.StockImportReport)
                   .HasForeignKey<StockImportForm>(f => f.ReceiptId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-N0V3KE1\SQLEXPRESS;Database=Dev;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<ServiceRegistered> ServiceRegistereds { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerGroup> CustomerGroups { get; set; }
        public virtual DbSet<CustomerBuyReceipt> CustomerBuyReceipts { get; set; }
        public virtual DbSet<CustomerBuyReceiptDetail> CustomerBuyReceiptDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }

        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorGroup> VendorGroups { get; set; }
        public virtual DbSet<VendorReplenishReceipt> VendorReplenishReceipts { get; set; }
        public virtual DbSet<VendorReplenishReceiptDetail> VendorReplenishReceiptDetails { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<ReturnBuyForm> ReturnBuyForm { get; set; }
        public virtual DbSet<ReturnReplenishForm> ReturnReplenishForms { get; set; }
        public virtual DbSet<StockExportForm> StockExportReports { get; set; }
        public virtual DbSet<StockImportForm> StockImportReports { get; set; }


    }
}
