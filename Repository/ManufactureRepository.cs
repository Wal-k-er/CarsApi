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
    public ICollection<Manufacture> GetManufactures()
    {
        return _context.Manufactures.OrderBy(m => m.Id)
            .Include(c=>c.CarManufactures)
            .ThenInclude(c=>c.Car)
            .ToList();
    }

    public Manufacture GetManufacture(int manufactureId)
    {
        return _context.Manufactures.Where(m => m.Id == manufactureId)
            .Include(c=>c.CarManufactures)
            .ThenInclude(c=>c.Car)
            .FirstOrDefault();

    }

    public ICollection<Car> GetCarsByManufacture(int manufactureId)
    {
        var carsByManufacture = _context.CarManufactures
            .Where(c => c.ManufactureId == manufactureId).Select(cm=>cm.Car).ToList();
        return carsByManufacture;
    }

    public bool ManufactureExists(int manufactureId)
    {
        return _context.Manufactures.Any(m => m.Id==manufactureId);
    }
    
    public bool CreateManufacture(Manufacture manufacture)
    {
        _context.Add(manufacture);
        return Save();
    }

    public bool CreateCarManufacture(CarManufacture carManufacture)
    {
        _context.CarManufactures.Add(carManufacture);
        return Save();
    }
    
    public bool UpdateManufacture(Manufacture manufacture)
    {
        _context.Update(manufacture);
        return Save();
    }

    public bool DeleteManufacture(Manufacture manufacture)
    {
        var carsManufacture = _context.CarManufactures.Where(c => 
            c.Manufacture == manufacture);
        _context.RemoveRange(carsManufacture);
        _context.Remove(manufacture);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}