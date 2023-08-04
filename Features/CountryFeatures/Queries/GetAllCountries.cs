using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CountryFeatures.Queries;

public class GetAllCountries : IRequest<ICollection<CountryDto>>
{
    
}

public class GetAllCountriesHandler : IRequestHandler<GetAllCountries, ICollection<CountryDto>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public GetAllCountriesHandler(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<CountryDto>> Handle(GetAllCountries request, CancellationToken cancellationToken)
    {
        var countries = await _countryRepository.GetCountries();

        return _mapper.Map<ICollection<CountryDto>>(countries);
    }
}