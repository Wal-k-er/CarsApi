using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
        
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Country> Countries { get; set; }
    
    public DbSet<Manufacture> Manufactures { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarManufacture> CarManufactures { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarManufacture>()
            .HasKey(pc => new { pc.CarId, pc.ManufactureId });
        modelBuilder.Entity<CarManufacture>()
            .HasOne(p => p.Car)
            .WithMany(pc => pc.CarManufactures)
            .HasForeignKey(c=>c.CarId);
        modelBuilder.Entity<CarManufacture>()
            .HasOne(p => p.Manufacture)
            .WithMany(pc => pc.CarManufactures)
            .HasForeignKey(c=>c.ManufactureId);
        
        modelBuilder.Entity<OrderItem>()
            .HasKey(pc => new { pc.CarId, pc.OrderId });
        modelBuilder.Entity<OrderItem>()
            .HasOne(p => p.Car)
            .WithMany(pc => pc.OrderItems)
            .HasForeignKey(c=>c.CarId);
        modelBuilder.Entity<OrderItem>()
            .HasOne(p => p.Order)
            .WithMany(pc => pc.OrderItems)
            .HasForeignKey(c=>c.OrderId);
    }
}