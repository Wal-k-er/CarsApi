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
        return _context.Cars.OrderBy(p => p.Id).ToList();
    }
}