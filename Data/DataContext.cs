using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
        try
        {
            Database.EnsureCreated();
        }
        catch (Exception e)
        {
        }
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Manufacture> Manufactures { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarManufacture> CarManufactures { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    //Внесение изменений на создание моделей
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Значение полей "Id" при создании записей будут всегда генерироваться как идентифицирующие
        modelBuilder.Entity<Car>().Property(p => p.Id).UseIdentityAlwaysColumn();
        modelBuilder.Entity<Category>().Property(p => p.Id).UseIdentityAlwaysColumn();
        modelBuilder.Entity<Country>().Property(p => p.Id).UseIdentityAlwaysColumn();
        modelBuilder.Entity<Customer>().Property(p => p.Id).UseIdentityAlwaysColumn();
        modelBuilder.Entity<Manufacture>().Property(p => p.Id).UseIdentityAlwaysColumn();
        modelBuilder.Entity<Order>().Property(p => p.Id).UseIdentityAlwaysColumn();
        
        //Настройка связей многие-ко-многим
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