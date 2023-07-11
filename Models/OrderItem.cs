namespace Cars.Models;

public class OrderItem
{
    public int CarId { get; set; }
    public int OrderId { get; set; }
    public Car Car { get; set; }
    public Order Order { get; set; }
}