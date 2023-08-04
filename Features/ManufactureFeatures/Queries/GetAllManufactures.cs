using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.ManufactureFeatures.Queries;

public class GetAllManufactures : IRequest<ICollection<ManufactureDto>>
{
}

public class GetAllManufacturesHandler : IRequestHandler<GetAllManufactures, ICollection<ManufactureDto>>
{
    private readonly IManufactureRepository _manufactureRepository;
    private readonly IMapper _mapper;

    public GetAllManufacturesHandler(IManufactureRepository manufactureRepository, IMapper mapper)
    {
        _manufactureRepository = manufactureRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<ManufactureDto>> Handle(GetAllManufactures request,
        CancellationToken cancellationToken)
    {
        var manufactures = await _manufactureRepository.GetManufactures();

        return _mapper.Map<ICollection<ManufactureDto>>(manufactures);
    }
}