using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CountryFeatures.Commands;

public class CreateCountry : IRequest<int>
{
    public CountryDto Model { get; }

    public CreateCountry(CountryDto countryDto)
    {
        Model = countryDto;
    }
    
    public class CreateCountryHandler : IRequestHandler<CreateCountry, int>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CreateCountryHandler(ICountryRepository countryRepository,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCountry request, CancellationToken cancellationToken)
        {
            CountryDto model = request.Model;
            
            var entity = _mapper.Map<Country>(model);
            
            await _countryRepository.CreateCountry(entity);
            
            return entity.Id;
        }
    }
}