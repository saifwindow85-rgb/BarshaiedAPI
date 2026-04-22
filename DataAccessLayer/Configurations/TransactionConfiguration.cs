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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);
            builder.Property(t => t.TransactionId).ValueGeneratedOnAdd();

            builder.HasOne(t => t.Creator).WithMany().HasForeignKey(t => t.CreatedByUserId).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.UpdatedByUser).WithMany().HasForeignKey
                (t => t.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TransactionType).WithMany().HasForeignKey
                (t => t.TransactionTypeId).OnDelete(DeleteBehavior.Restrict);
      
            builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.ToTable("Transactions"); 
        }
    }
}
