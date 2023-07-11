namespace Cars.Models;

public class CarManufacture
{
    public int CarId { get; set; }
    public int ManufactureId { get; set; }
    public Car Car { get; set; }
    public Manufacture Manufacture { get; set; }
}