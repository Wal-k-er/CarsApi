using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Queries;

public class GetManufactureById : IRequest<ManufactureDto>
{
    public GetManufactureById(int manufactureId)
    {
        Id = manufactureId;
    }

    public int Id { get; }
}

public class GetManufactureByIdHandler : IRequestHandler<GetManufactureById, ManufactureDto>
{
    private readonly IManufactureRepository _manufactureRepository;
    private readonly IMapper _mapper;

    public GetManufactureByIdHandler(IManufactureRepository manufactureRepository, IMapper mapper)
    {
        _manufactureRepository = manufactureRepository;
        _mapper = mapper;
    }

    public async Task<ManufactureDto> Handle(GetManufactureById request, CancellationToken cancellationToken)
    {
        if (!await _manufactureRepository.ManufactureExists(request.Id))
            throw new EntityNotFoundException($"Не найдено производство {request.Id}");

        var manufacture = await _manufactureRepository.GetManufacture(request.Id);

        return _mapper.Map<ManufactureDto>(manufacture);
    }
}