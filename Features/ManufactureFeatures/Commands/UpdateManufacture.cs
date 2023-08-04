using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Commands;

public class UpdateManufacture : IRequest<int>
{
    public UpdateManufacture(int manufactureId, ManufactureDto manufactureDto)
    {
        ManufactureId = manufactureId;
        Model = manufactureDto;
    }

    public int ManufactureId { get; }
    public ManufactureDto Model { get; }
}

public class UpdateManufactureHandler : IRequestHandler<UpdateManufacture, int>
{
    private readonly ICarRepository _carRepository;
    private readonly IManufactureRepository _manufactureRepository;
    private readonly IMapper _mapper;

    public UpdateManufactureHandler(IManufactureRepository manufactureRepository, ICarRepository carRepository,
        IMapper mapper)
    {
        _manufactureRepository = manufactureRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateManufacture request, CancellationToken cancellationToken)
    {
        if (!await _manufactureRepository.ManufactureExists(request.ManufactureId))
            throw new EntityNotFoundException($"Не найдено производство {request.ManufactureId}");

        var model = request.Model;

        var entity = _mapper.Map<Manufacture>(model);
        entity.Id = request.ManufactureId;

        await _manufactureRepository.UpdateManufacture(entity);
        
        await _manufactureRepository.DeleteCarsManufacture(entity);
        
        foreach (var manufacturedCar in model.ManufacturedCars)
        {
            if (!await _carRepository.CarExists(manufacturedCar.CarId))
                throw new EntityNotFoundException($"Не найдена машина {manufacturedCar.CarId}");
            
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