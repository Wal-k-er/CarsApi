using Cars.Data;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Cars.Repository;

public class ManufactureRepository : IManufactureRepository
{
    private readonly DataContext _context;
    

    public ManufactureRepository(DataContext context)
    {
        _context = context;
        
    }
    public async Task<ICollection<Manufacture>> GetManufactures()
    {
        return await _context.Manufactures.OrderBy(m => m.Id)
            .Include(c=>c.CarManufactures)
            .ThenInclude(c=>c.Car)
            .ToListAsync();
    }

    public async Task<Manufacture> GetManufacture(int manufactureId)
    {
        return await _context.Manufactures.Where(m => m.Id == manufactureId)
            .Include(c=>c.CarManufactures)
            .ThenInclude(c=>c.Car)
            .FirstOrDefaultAsync();

    }

    public async Task<ICollection<Car>> GetCarsByManufacture(int manufactureId)
    {
        var carsByManufacture = _context.CarManufactures
            .Where(c => c.ManufactureId == manufactureId).Select(cm=>cm.Car).ToListAsync();
        return await carsByManufacture;
    }

    public async Task<bool> ManufactureExists(int manufactureId)
    {
        return await _context.Manufactures.AnyAsync(m => m.Id==manufactureId);
    }
    
    public async Task CreateManufacture(Manufacture manufacture)
    {
        _context.Add(manufacture);
        await Save();
    }

    public async Task<CarManufacture> GetCarManufacture(int manufactureId, int carId)
    {
        var carManufacture = _context.CarManufactures
            .Where(c=>c.ManufactureId==manufactureId).FirstOrDefaultAsync(c => c.CarId==carId);
        return await carManufacture;
    }

    public async Task<bool> CarManufactureExists(int manufactureId, int carId)
    {
        return await _context.CarManufactures
            .Where(c => c.ManufactureId == manufactureId)
            .AnyAsync(c => c.CarId == carId);
    }

    public async Task CreateCarManufacture(CarManufacture carManufacture)
    {
        _context.CarManufactures.Add(carManufacture);
        await Save();
    }

    public async Task DeleteCarManufacture(CarManufacture carManufacture)
    {
        _context.CarManufactures.Remove(carManufacture);
        await Save();
    }

    public async Task DeleteCarsManufacture(Manufacture manufacture)
    {
        var carsManufacture = _context.CarManufactures.Where(c => 
            c.Manufacture == manufacture);
        _context.RemoveRange(carsManufacture);
        await Save();
    }

    public async Task UpdateManufacture(Manufacture manufacture)
    {
        _context.Update(manufacture);
        await Save();
    }

    public async Task DeleteManufacture(Manufacture manufacture)
    {
        await DeleteCarsManufacture(manufacture);
        _context.Remove(manufacture);
        await Save();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}