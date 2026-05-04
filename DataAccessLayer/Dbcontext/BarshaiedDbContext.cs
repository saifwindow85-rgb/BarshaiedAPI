using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.AppDbContext
{
    public class BarshaiedDbContext:DbContext
    {
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingListItem>ShoppingListItems { get; set; }
        public DbSet<User>Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType>TransactionTypes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public BarshaiedDbContext(DbContextOptions<BarshaiedDbContext> options)
        : base(options)
        {
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BarshaiedDbContext).Assembly);
        }
    }
}
