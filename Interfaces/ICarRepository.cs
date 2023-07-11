using Cars.Models;

namespace Cars.Interfaces;

public interface ICarRepository
{
    ICollection<Car> GetCars();
}