using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CarFeatures.Queries;

public class GetCarById : IRequest<CarDto>
{
    public GetCarById(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

public class GetCarByIdHandler : IRequestHandler<GetCarById, CarDto>
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public GetCarByIdHandler(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<CarDto> Handle(GetCarById request, CancellationToken cancellationToken)
    {
        if (!await _carRepository.CarExists(request.Id))
            throw new EntityNotFoundException($"Не найдена машина {request.Id}");

        var car = await _carRepository.GetCar(request.Id);

        return _mapper.Map<CarDto>(car);
    }
}