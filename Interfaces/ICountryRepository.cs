using Cars.Models;

namespace Cars.Interfaces;

public interface ICountryRepository
{
    Task<ICollection<Country>> GetCountries();
    Task<Country>  GetCountry(int countryId);
    Task<ICollection<Manufacture>> GetManufacturesByCountry(int countryId);
    Task<bool> CountryExsists(int countryId);
    Task CreateCountry(Country country);
    Task UpdateCountry(Country country);
    Task DeleteCountry(Country country);
    Task Save();
}