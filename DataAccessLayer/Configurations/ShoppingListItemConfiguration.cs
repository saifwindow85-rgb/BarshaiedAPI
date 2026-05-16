using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class ShoppingListItemConfiguration : IEntityTypeConfiguration<ShoppingListItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
        {
            // Primary Key
            builder.HasKey(l => l.ShoppingListItemId);

            builder.Property(l => l.ShoppingListItemId)
                   .ValueGeneratedOnAdd();

            builder.Property(l => l.UnitPrice).HasPrecision(18, 2).IsRequired();
            builder.Property(l => l.Total).HasPrecision(18, 2).IsRequired();

            // Product Relationship
            builder.HasOne(l => l.Product)
                   .WithMany(l=>l.ShoppingListItems)
                   .HasForeignKey(l => l.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // ShoppingListPage Relationship
            builder.HasOne(l => l.shoppingListPage)
                   .WithMany(p => p.ShoppingListItems)
                   .HasForeignKey(l => l.PageId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Audit Relationships
            builder.HasOne(l => l.Creator)
                   .WithMany()
                   .HasForeignKey(l => l.CreatedByUserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.UpdatedByUser)
                   .WithMany()
                   .HasForeignKey(l => l.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Default CreatedAt
            builder.Property(l => l.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            // Computed Column:
            // Total = Product.Price * Quantity
            // عدّل اسم العمود [Price] إذا كان اسم سعر المنتج مختلفًا في جدول Products
            builder.Property(l => l.Total)
                   .HasComputedColumnSql(
                       @"(
                          [UnitPrice] * [Quantity]
                       )",
                       stored: true);

            // Table Name
            builder.ToTable("ShoppingListItems");
        }
    }
}
