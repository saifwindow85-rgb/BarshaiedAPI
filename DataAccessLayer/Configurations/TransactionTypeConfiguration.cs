using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstracts;

namespace DataAccessLayer.Configurations
{
    public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
    {
        public void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            builder.HasKey(t => t.TransactionTypeId);
            builder.Property(t => t.TransactionTypeId).ValueGeneratedOnAdd();

            builder.HasOne(t => t.Creator).WithMany().HasForeignKey(t => t.CreatedByUserId)
                        .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.HasOne(t => t.UpdatedByUser).WithMany().HasForeignKey
                (t => t.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()").IsRequired();

            builder.HasIndex(t => t.TransactionName).IsUnique();
            builder.Property(t => t.TransactionName).HasColumnType("NVARCHAR(50)").IsRequired();
            builder.ToTable("TransactionTypes");

            //builder.HasData(builder.HasData(
            //    new TransactionType { TransactionTypeId = 1, TransactionName = "Sale",CreatedByUserId = 1 },
            //    new TransactionType { TransactionTypeId = 2, TransactionName = "Purchase",CreatedByUserId =1 }
            // ));

        }
    }
}
