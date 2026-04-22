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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserId).ValueGeneratedOnAdd();

            builder.Property(u => u.UserName).HasColumnType("NVARCHAR(50)").IsRequired();
            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.Password).HasColumnType("NVARCHAR(50)").IsRequired();

            builder.Property(u => u.Permissions).HasColumnType("Tinyint").IsRequired();
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()").IsRequired();

            builder.HasOne(u => u.Creator).WithMany().HasForeignKey
                (u => u.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.ToTable("Users");
            builder.HasData(new User
            {   UserId =1,
                UserName = "Admin",
                Password = "12345",
                IsActive = true,
                Permissions = 1,
                CreatedAt = DateTime.Now
            });
        }
    }
}
