using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Commands;

public class CreateManufacture : IRequest<int>
{
    public CreateManufacture(ManufactureDto manufactureDto, int countryId)
    {
        Model = manufactureDto;
        CountryId = countryId;
    }

    public ManufactureDto Model { get; }

    public int CountryId { get; }
}

public class CreateManufactureHandler : IRequestHandler<CreateManufacture, int>
{
    private readonly ICarRepository _carRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IManufactureRepository _manufactureRepository;
    private readonly IMapper _mapper;

    public CreateManufactureHandler(IManufactureRepository manufactureRepository, ICountryRepository countryRepository,
        ICarRepository carRepository, IMapper mapper)
    {
        _manufactureRepository = manufactureRepository;
        _countryRepository = countryRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateManufacture request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        var entity = _mapper.Map<Manufacture>(model);
        if (!await _countryRepository.CountryExsists(request.CountryId))
            throw new EntityNotFoundException($"Не найдена машина {request.CountryId}");

        var country = await _countryRepository.GetCountry(request.CountryId);
        entity.Country = country;

        await _manufactureRepository.CreateManufacture(entity);

        foreach (var manufacturedCar in model.ManufacturedCars)
        {
            var car = await _carRepository.GetCar(manufacturedCar.CarId);
            var carManufacture = new CarManufacture
            {
                Manufacture = entity,
                ManufactureId = entity.Id,
                Car = car,
                CarId = car.Id
            };
            await _manufactureRepository.CreateCarManufacture(carManufacture);
        }

        return entity.Id;
    }
}