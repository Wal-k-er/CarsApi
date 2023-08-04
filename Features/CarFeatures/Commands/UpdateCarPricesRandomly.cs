using System.Runtime.Serialization;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CarFeatures.Commands;

public class UpdateCarPricesRandomly : IRequest<Unit>
{
    
}

public class UpdateCarPricesRandomlyHandler : IRequestHandler<UpdateCarPricesRandomly, Unit>
{
    private readonly ICarRepository _carRepository;
    private readonly IRandomPriceRepository _randomPriceRepository;

    public UpdateCarPricesRandomlyHandler(ICarRepository carRepository, IRandomPriceRepository randomPriceRepository)
    {
        _carRepository = carRepository;
        _randomPriceRepository = randomPriceRepository;
    }

    public async Task<Unit> Handle(UpdateCarPricesRandomly request, CancellationToken cancellationToken)
    {
        var cars = await _carRepository.GetCars();

        foreach (var car in cars)
        {
            car.Price = _randomPriceRepository.GeneratePrice();
            await _carRepository.UpdateCar(car);
        }

        return Unit.Value;
    }
}