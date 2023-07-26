using Cars.Models;

namespace Cars.Interfaces;

public interface ICountryRepository
{
    ICollection<Country> GetCountries();
    Country GetCountry(int countryId);
    ICollection<Manufacture> GetManufacturesByCountry(int countryId);
    bool CountryExsists(int countryId);
    bool CreateCountry(Country country);
    bool UpdateCountry(Country country);
    bool DeleteCountry(Country country);
    bool Save();
}