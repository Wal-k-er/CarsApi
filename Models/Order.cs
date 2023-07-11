namespace Cars.Models;

public class Order
{
    public int Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Customer Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}