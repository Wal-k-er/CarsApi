using Cars.Data;
using Cars.Interfaces;
using Cars.Models;

namespace Cars.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Country> GetCountries()
    {
        return _context.Countries.OrderBy(p => p.Id).ToList();
    }

    public Country GetCountry(int countryId)
    {
        return _context.Countries.Where(p => p.Id == countryId).FirstOrDefault();
    }

    public ICollection<Manufacture> GetManufacturesByCountry(int countryId)
    {
        var manufacturesByCountry = _context.Manufactures
            .Where(p => p.Country.Id == countryId).ToList();
        return manufacturesByCountry;
    }

    public bool CountryExsists(int countryId)
    {
        return _context.Countries.Any(c => c.Id == countryId);
    }

    public bool CreateCountry(Country country)
    {
        _context.Add(country);
        return Save();
    }

    public bool UpdateCountry(Country country)
    {
        _context.Update(country);
        return Save();
    }

    public bool DeleteCountry(Country country)
    {
        _context.Remove(country);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
    
}