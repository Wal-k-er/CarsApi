using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Commands;

public class DeleteManufacture : IRequest<int>
{
    public DeleteManufacture(int manufactureId)
    {
        Id = manufactureId;
    }

    public int Id { get; }
}

public class DeleteManufactureHandler : IRequestHandler<DeleteManufacture, int>
{
    private readonly IManufactureRepository _manufactureRepository;


    public DeleteManufactureHandler(IManufactureRepository manufactureRepository)
    {
        _manufactureRepository = manufactureRepository;
    }

    public async Task<int> Handle(DeleteManufacture request, CancellationToken cancellationToken)
    {
        if (!await _manufactureRepository.ManufactureExists(request.Id))
            throw new EntityNotFoundException($"Не найдено производство {request.Id}");

        var manufacture = await _manufactureRepository.GetManufacture(request.Id);

        await _manufactureRepository.DeleteManufacture(manufacture);

        return request.Id;
    }
}