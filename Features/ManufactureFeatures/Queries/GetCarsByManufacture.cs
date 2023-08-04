using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Queries;

public class GetCarsByManufacture : IRequest<ICollection<CarDto>>
{
    public GetCarsByManufacture(int manufactureId)
    {
        Id = manufactureId;
    }

    public int Id { get; }
}

public class GetCarsByManufactureHandler : IRequestHandler<GetCarsByManufacture, ICollection<CarDto>>
{
    private readonly IManufactureRepository _manufactureRepository;
    private readonly IMapper _mapper;

    public GetCarsByManufactureHandler(IManufactureRepository manufactureRepository, IMapper mapper)
    {
        _manufactureRepository = manufactureRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<CarDto>> Handle(GetCarsByManufacture request, CancellationToken cancellationToken)
    {
        if (!await _manufactureRepository.ManufactureExists(request.Id))
            throw new EntityNotFoundException($"Не найдено производство {request.Id}");

        var cars = await _manufactureRepository
            .GetCarsByManufacture(request.Id);

        return _mapper.Map<ICollection<CarDto>>(cars);
    }
}