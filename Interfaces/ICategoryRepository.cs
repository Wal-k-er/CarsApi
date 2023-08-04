using Cars.Models;

namespace Cars.Interfaces;

public interface ICategoryRepository
{
    Task<ICollection<Category>>  GetCategories();
    Task<Category> GetCategory(int categoryId);
    Task<ICollection<Car>> GetCarsByCategory(int categoryId);
    Task<bool> CategoryExists(int categoryId);
    Task CreateCategory(Category category);
    Task UpdateCategory(Category category);
    Task DeleteCategory(Category category);
    Task Save();
}