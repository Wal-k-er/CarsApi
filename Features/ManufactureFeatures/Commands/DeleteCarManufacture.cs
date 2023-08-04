using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Commands;

public class DeleteCarManufacture : IRequest<int>
{
    public DeleteCarManufacture(int manufactureId, int carId)
    {
        ManufactureId = manufactureId;
        CarId = carId;
    }
    public int ManufactureId { get; }
    public int CarId { get; }
}

public class DeleteCarManufactureHandler : IRequestHandler<DeleteCarManufacture, int>
{
    private readonly IManufactureRepository _manufactureRepository;

    public DeleteCarManufactureHandler(IManufactureRepository manufactureRepository)
    {
        _manufactureRepository = manufactureRepository;
    }

    public async Task<int> Handle(DeleteCarManufacture request, CancellationToken cancellationToken)
    {
        if (!await _manufactureRepository.ManufactureExists(request.ManufactureId))
            throw new EntityNotFoundException($"Не найдено производство {request.ManufactureId}");

        if (!await _manufactureRepository.CarManufactureExists(request.ManufactureId, request.CarId))
            throw new EntityNotFoundException($"Не найдена позиция производства {request.CarId}");

        var carManufacture = await _manufactureRepository.GetCarManufacture(request.ManufactureId, request.CarId);

        await _manufactureRepository.DeleteCarManufacture(carManufacture);
        
        return request.CarId;
    }
}