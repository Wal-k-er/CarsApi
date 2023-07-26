using System.Diagnostics;
using Cars.Data;
using Cars.Interfaces;
using Cars.Models;

namespace Cars.Repository;

public class CategoryRepository: ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Category> GetCategories()
    {
        return _context.Categories.OrderBy(p => p.Id).ToList();
    }

    public Category GetCategory(int categoryId)
    {
        return _context.Categories.Where(p => p.Id == categoryId).FirstOrDefault();
    }

    public ICollection<Car> GetCarsByCategory(int categoryId)
    {
        var carsByCategory = _context.Cars
            .Where(p => p.Category.Id == categoryId).ToList();
        return carsByCategory;
    }

    public bool CategoryExists(int categoryId)
    {
        return _context.Categories.Any(p => p.Id == categoryId);
    }

    public bool CreateCategory(Category category)
    {
        Console.WriteLine(category.Name);
        
        _context.Add(category);
        return Save();
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);
        return Save();
    }

    public bool DeleteCategory(Category category)
    {
        _context.Remove(category);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}