using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using System.Reflection;

namespace StockFlow.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Administrators> Administrators { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Suppliers> Suppliers { get; set; }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<SaleItems> SaleItems { get; set; }
    public DbSet<Purchases> Purchases { get; set; }
    public DbSet<PurchaseItems> PurchaseItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
