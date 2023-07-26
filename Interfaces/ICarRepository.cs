using Cars.Models;

namespace Cars.Interfaces;

public interface ICarRepository
{
    ICollection<Car> GetCars();
    Car GetCar(int id);
    Car GetCar(string name);
    ICollection<Manufacture> getCarManufactures(int carId);
    bool CarExists(int carId);
    bool CreateCar(Car car);
    bool UpdateCar(Car car);
    bool DeleteCar(Car car);
    bool Save();
}