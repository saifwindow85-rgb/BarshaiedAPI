using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ShoppingListPageConfiguration : IEntityTypeConfiguration<ShoppingListPage>
    {
        public void Configure(EntityTypeBuilder<ShoppingListPage> builder)
        {
            // Primary Key
            builder.HasKey(p => p.PageId);

            builder.Property(p => p.PageId)
                   .ValueGeneratedOnAdd();
            // Optional Note
            builder.Property(p => p.Note)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            // One-to-Many Relationship
            builder.HasMany(p => p.ShoppingListItems)
                   .WithOne(i => i.shoppingListPage)
                   .HasForeignKey(i => i.PageId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Computed Column:
            // Total = SUM(ShoppingListItems.Total) for this page   

            // Table Name
            builder.ToTable("ShoppingListPages");
        }
    }
}
