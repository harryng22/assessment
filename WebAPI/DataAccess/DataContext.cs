using Microsoft.EntityFrameworkCore;
using WebAPI.DataAccess.Entities;

namespace WebAPI.Data;

public class DataContext : DbContext
{
    protected readonly IConfiguration configuration;

    public DataContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
    }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
}