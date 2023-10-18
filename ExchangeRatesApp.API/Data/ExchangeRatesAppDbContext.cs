using ExchangeRatesApp.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExchangeRatesApp.API.Data
{
    public class ExchangeRatesAppDbContext : DbContext
    {
        public ExchangeRatesAppDbContext(DbContextOptions<ExchangeRatesAppDbContext> options) : base(options)
        {
        }

        public DbSet<CurrencyRates> CurrencyRates { get; set; }
        public DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRates>()
                .HasMany(cr => cr.Rates)
                .WithOne(r => r.CurrencyRates)
                .HasForeignKey(r => r.CurrencyRatesId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
