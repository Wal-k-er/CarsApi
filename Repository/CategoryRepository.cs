using System.Diagnostics;
using Cars.Data;
using Cars.Interfaces;
using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Repository;

public class CategoryRepository: ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<ICollection<Category>> GetCategories()
    {
        return await _context.Categories.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<Category> GetCategory(int categoryId)
    {
        return await _context.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);
    }

    public async Task<ICollection<Car>> GetCarsByCategory(int categoryId)
    {
        var carsByCategory = await _context.Cars
            .Where(p => p.Category.Id == categoryId).ToListAsync();
        return carsByCategory;
    }

    public async Task<bool> CategoryExists(int categoryId)
    {
        return _context.Categories.Any(p => p.Id == categoryId);
    }

    public async Task CreateCategory(Category category)
    {
        Console.WriteLine(category.Name);
        
        _context.Add(category);
        await Save();
    }

    public async Task UpdateCategory(Category category)
    {
        _context.Update(category);
        await Save();
    }

    public async Task DeleteCategory(Category category)
    {
        _context.Remove(category);
        await Save();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
        
    }
}