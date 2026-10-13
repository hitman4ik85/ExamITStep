using Microsoft.EntityFrameworkCore;

namespace AutoSales.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<Seller> Sellers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=AutoSales.db");
    }
}