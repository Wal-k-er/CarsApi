using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CountryFeatures.Queries;

public class GetCountryById : IRequest<CountryDto>
{
    public GetCountryById(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

public class GetCountryByIdHandler : IRequestHandler<GetCountryById, CountryDto>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public GetCountryByIdHandler(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    public async Task<CountryDto> Handle(GetCountryById request, CancellationToken cancellationToken)
    {
        if (!await _countryRepository.CountryExsists(request.Id))
            throw new EntityNotFoundException($"Не найдена страна {request.Id}");

        var country = await _countryRepository.GetCountry(request.Id);

        return _mapper.Map<CountryDto>(country);
    }
}