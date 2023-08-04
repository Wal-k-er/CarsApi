using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CarFeatures.Queries;

public class GetCarManufactures: IRequest<ICollection<ManufactureDto>>
{
    public int Id { get; }

    public GetCarManufactures(int id)
    {
        Id = id;
    }
}

public class GetCarManufacuresHandler : IRequestHandler<GetCarManufactures, ICollection<ManufactureDto>>
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public GetCarManufacuresHandler(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }
    public async Task<ICollection<ManufactureDto>> Handle(GetCarManufactures request, CancellationToken cancellationToken)
    {
        if (!await _carRepository.CarExists(request.Id))
            throw new EntityNotFoundException($"Не найдена машина {request.Id}");
        
        var manufactures = await _carRepository.GetCarManufactures(request.Id);
        
        return _mapper.Map<ICollection<ManufactureDto>>(manufactures);
    }
}