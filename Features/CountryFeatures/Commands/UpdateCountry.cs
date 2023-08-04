using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CountryFeatures.Commands;

public class UpdateCountry : IRequest<int>
{
    public UpdateCountry(int countryId, CountryDto countryDto)
    {
        CountryId = countryId;
        Model = countryDto;
    }

    public int CountryId { get; }
    public CountryDto Model { get; }

    public class UpdateCountryHandler : IRequestHandler<UpdateCountry, int>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public UpdateCountryHandler(ICountryRepository countryRepository,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCountry request, CancellationToken cancellationToken)
        {
            if (!await _countryRepository.CountryExsists(request.CountryId))
                throw new EntityNotFoundException($"Не найдена страна {request.CountryId}");

            var model = request.Model;

            var entity = _mapper.Map<Country>(model);
            entity.Id = request.CountryId;

            await _countryRepository.UpdateCountry(entity);

            return entity.Id;
        }
    }
}