using Cars.Data;
using Cars.Interfaces;
using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<ICollection<Country>> GetCountries()
    {
        return await _context.Countries.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<Country> GetCountry(int countryId)
    {
        return await _context.Countries.FirstOrDefaultAsync(p => p.Id == countryId);
    }

    public async Task<ICollection<Manufacture>> GetManufacturesByCountry(int countryId)
    {
        var manufacturesByCountry = await _context.Manufactures
            .Where(p => p.Country.Id == countryId).ToListAsync();
        return manufacturesByCountry;
    }

    public async Task<bool> CountryExsists(int countryId)
    {
        return await _context.Countries.AnyAsync(c => c.Id == countryId);
    }

    public async Task CreateCountry(Country country)
    {
        _context.Add(country);
        await Save();
    }

    public async Task UpdateCountry(Country country)
    {
        _context.Update(country);
        await Save();
    }

    public async Task DeleteCountry(Country country)
    {
        _context.Remove(country);
        await Save();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    
}