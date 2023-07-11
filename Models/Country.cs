namespace Cars.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Manufacture> Manufactures { get; set; }
}