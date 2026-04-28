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
            builder.HasKey(l => l.ShoppingListItemId);
            builder.Property(l => l.ShoppingListItemId).ValueGeneratedOnAdd();

            builder.HasMany(l => l.Transactions).WithOne(l=>l.ShoppingListItem).HasForeignKey(s => s.ShoppingListItemId)
                .IsRequired(false).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Creator).WithMany().HasForeignKey(l => l.CreatedByUserId).IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.UpdatedByUser).WithMany().HasForeignKey
                (t => t.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(l => l.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(l => l.Notes).HasColumnType("NVARCHAR(300)").IsRequired(false);
            builder.ToTable("ShoppingListItems");
        }
    }
}
