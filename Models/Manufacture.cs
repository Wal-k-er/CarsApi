using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models;

public class Manufacture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Itn { get; set; }
    public string Psrn { get; set; }
    public Country Country { get; set; }
    public ICollection<CarManufacture> CarManufactures { get; set; }
}