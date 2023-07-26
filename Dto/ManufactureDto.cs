namespace Cars.Dto;

public class ManufactureDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Itn { get; set; }
    public string Psrn { get; set; }
    
    //Список "Id" производимых машин
    public List<CarManufactureDto> ManufacturedCars { get; set; }
}