using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CountryFeatures.Queries;

public class GetManufacturesByCountry : IRequest<ICollection<ManufactureDto>>
{
    public int Id { get; }

    public GetManufacturesByCountry(int id)
    {
        Id = id;
    }
}

public class GetManufacturesByCountryHandler : IRequestHandler<GetManufacturesByCountry, ICollection<ManufactureDto>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public GetManufacturesByCountryHandler(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<ManufactureDto>> Handle(GetManufacturesByCountry request, 
        CancellationToken cancellationToken)
    {
        if(!await _countryRepository.CountryExsists(request.Id))
            throw new EntityNotFoundException($"Не найдена страна {request.Id}");
        
        var manufactures = await _countryRepository
            .GetManufacturesByCountry(request.Id);

        return _mapper.Map<ICollection<ManufactureDto>>(manufactures);
    }
}