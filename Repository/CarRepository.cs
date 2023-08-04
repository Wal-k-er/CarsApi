using Cars.Data;
using Cars.Interfaces;
using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Repository;

public class CarRepository : ICarRepository
{
    private readonly DataContext _context;
    
    public CarRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Car>> GetCars()
    {
        return await _context.Cars.OrderBy(c => c.Id).ToListAsync();
    }

    public async Task<Car>  GetCar(int id)
    {
        return await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Car> GetCar(string name)
    {
        return await _context.Cars.FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    public async Task<ICollection<Manufacture>> GetCarManufactures(int carId)
    {
        var carManufactures = _context.CarManufactures
            .Where(c => c.CarId == carId).Select(cm => cm.Manufacture).ToListAsync();
        return await carManufactures;
    }

    public async Task<bool> CarExists(int carId)
    {
        return await _context.Cars.AnyAsync(c => c.Id == carId);
    }

    public async Task CreateCar(Car car)
    {
        await _context.AddAsync(car);
        await Save();
    }

    public async Task UpdateCar(Car car)
    {
        _context.Update(car);
        await Save();
    }

    public async Task DeleteCar(Car car)
    {
        _context.Remove(car);
        await Save();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}