using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models;

public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateProduction { get; set; }
    public Category Category { get; set; }
    public ICollection<CarManufacture> CarManufactures { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
