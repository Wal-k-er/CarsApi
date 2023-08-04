using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CountryFeatures.Commands;

public class DeleteCountry : IRequest<int>
{
    public DeleteCountry(int countryId)
    {
        Id = countryId;
    }

    public int Id { get; }

    public class DeleteCountryHandler : IRequestHandler<DeleteCountry, int>
    {
        private readonly ICountryRepository _countryRepository;


        public DeleteCountryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }


        public async Task<int> Handle(DeleteCountry request, CancellationToken cancellationToken)
        {
            if (!await _countryRepository.CountryExsists(request.Id))
                throw new EntityNotFoundException($"Не найдена страна {request.Id}");

            var country = await _countryRepository.GetCountry(request.Id);

            await _countryRepository.DeleteCountry(country);

            return request.Id;
        }
    }
}