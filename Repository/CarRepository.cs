using Cars.Data;
using Cars.Interfaces;
using Cars.Models;

namespace Cars.Repository;

public class CarRepository : ICarRepository
{
    private readonly DataContext _context;
    
    public CarRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Car> GetCars()
    {
        return _context.Cars.OrderBy(c => c.Id).ToList();
    }

    public Car GetCar(int id)
    {
        return _context.Cars.Where(c => c.Id == id).FirstOrDefault();
    }

    public Car GetCar(string name)
    {
        return _context.Cars.Where(c => c.Name == name).FirstOrDefault();
    }

    public ICollection<Manufacture> getCarManufactures(int carId)
    {
        var carManufactures = _context.CarManufactures
            .Where(c => c.CarId == carId).Select(cm => cm.Manufacture).ToList();
        return carManufactures;
    }

    public bool CarExists(int carId)
    {
        return _context.Cars.Any(c => c.Id == carId);
    }

    public bool CreateCar(Car car)
    {
        _context.Add(car);
        return Save();
    }

    public bool UpdateCar(Car car)
    {
        _context.Update(car);
        return Save();
    }

    public bool DeleteCar(Car car)
    {
        _context.Remove(car);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return
            saved > 0 ? true : false;
    }
}