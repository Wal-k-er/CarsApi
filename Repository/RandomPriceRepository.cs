using Cars.Interfaces;

namespace Cars.Repository;

public class RandomPriceRepository : IRandomPriceRepository
{
    private readonly Random _random;
    
    public RandomPriceRepository()
    {
        _random = new Random();
    }
    public float GeneratePrice()
    {
        return (float)Math.Round(_random.NextSingle() * 200000, 2);
    }
}