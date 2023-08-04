using Cars.Dto;
using Cars.Models;

namespace Cars.Interfaces;

public interface IManufactureRepository
{
    Task<ICollection<Manufacture>> GetManufactures();
    Task<Manufacture> GetManufacture(int manufactureId);
    Task<ICollection<Car>> GetCarsByManufacture(int manufactureId);
    Task<bool> ManufactureExists(int manufactureId);
    Task CreateManufacture(Manufacture manufacture);
    Task<CarManufacture> GetCarManufacture(int manufactureId, int carId);
    Task<bool> CarManufactureExists(int manufactureId, int carId);
    Task CreateCarManufacture(CarManufacture carManufacture);
    Task DeleteCarManufacture(CarManufacture carManufacture);
    Task DeleteCarsManufacture(Manufacture manufacture);
    Task UpdateManufacture(Manufacture manufacture);
    Task DeleteManufacture(Manufacture manufacture);
    Task Save();
}