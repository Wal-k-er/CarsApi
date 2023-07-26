using Cars.Models;

namespace Cars.Interfaces;

public interface ICategoryRepository
{
    ICollection<Category> GetCategories();
    Category GetCategory(int categoryId);
    ICollection<Car> GetCarsByCategory(int categoryId);
    bool CategoryExists(int categoryId);
    bool CreateCategory(Category category);
    bool UpdateCategory(Category category);
    bool DeleteCategory(Category category);
    bool Save();
}