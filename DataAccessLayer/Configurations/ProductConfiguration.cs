using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductId).ValueGeneratedOnAdd();

            builder.Property(p => p.ProductName).HasColumnType("NVARCHAR(100)").IsRequired();

            builder.HasIndex(p => p.Barcode).IsUnique().HasFilter("[Barcode] IS NOT NULL");
            builder.Property(p => p.Barcode).HasColumnType("NVARCHAR(20)").IsRequired(false);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CostPrice).HasPrecision(18, 4);
            builder.Property(p => p.SellPrice).HasPrecision(18, 4);
            builder.Property(p => p.ProfitMargin).HasPrecision(18, 4);

            builder.Property(p => p.ProfitMargin)
     .HasComputedColumnSql(
                 @"CASE 
                 WHEN [SellPrice] = 0 THEN 0 
                ELSE (([SellPrice] - [CostPrice]) * 100.0 / [SellPrice])
                END",
                 stored: true);

            builder.HasOne(p => p.Creator).WithMany().HasForeignKey(t => t.CreatedByUserId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UpdatedByUser).WithMany().HasForeignKey
                (t => t.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ShoppingListItems).WithOne(p => p.Product).HasForeignKey(p => p.ProductId).IsRequired();
            builder.ToTable("Products",p=>p.HasCheckConstraint("CK_Product_Prices","[SellPrice] >= [CostPrice]"));


        }
    }
}
