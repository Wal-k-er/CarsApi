using Cars.Models;

namespace Cars.Interfaces;

public interface ICarRepository
{
    Task<ICollection<Car>>  GetCars();
    Task<Car> GetCar(int id);
    
    Task<ICollection<Manufacture>>  GetCarManufactures(int carId);
    Task<bool> CarExists(int carId);
    Task CreateCar(Car car);
    Task UpdateCar(Car car);
    Task DeleteCar(Car car);
    Task Save();
}