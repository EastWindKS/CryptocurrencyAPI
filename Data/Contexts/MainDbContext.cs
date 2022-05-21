using CryptocurrencyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrencyAPI.Data.Contexts;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    public DbSet<CryptocurrencyInformation> CryptocurrencyInformation { get; set; }

    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CryptocurrencyInformation>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,4)");
    }
}