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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);
            builder.Property(c => c.CategoryId).ValueGeneratedOnAdd();

            builder.Property(c => c.Name).HasColumnType("NVARCHAR(50)").IsRequired();
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(c => c.Creator).WithMany().HasForeignKey(t => t.CreatedByUserId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.UpdatedByUser).WithMany().HasForeignKey
                (t => t.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.Products).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.SetNull);
            builder.ToTable("Categories");
        }
    }
}
