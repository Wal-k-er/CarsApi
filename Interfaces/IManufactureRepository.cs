using Cars.Dto;
using Cars.Models;

namespace Cars.Interfaces;

public interface IManufactureRepository
{
    ICollection<Manufacture> GetManufactures();
    Manufacture GetManufacture(int manufactureId);
    ICollection<Car> GetCarsByManufacture(int manufactureId);
    bool ManufactureExists(int manufactureId);
    bool CreateManufacture(Manufacture manufacture);
    bool CreateCarManufacture(CarManufacture carManufacture);
    bool UpdateManufacture(Manufacture manufacture);
    bool DeleteManufacture(Manufacture manufacture);
    bool Save();
}