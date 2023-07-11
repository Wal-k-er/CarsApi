namespace Cars.Models;

public class Car
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset DateProduction { get; set; }
    public Category Category { get; set; }
    public ICollection<CarManufacture> CarManufactures { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}