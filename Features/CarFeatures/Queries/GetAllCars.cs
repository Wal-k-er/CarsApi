using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CarFeatures.Queries;

public class GetAllCars : IRequest<ICollection<CarDto>>
{
    
}

public class GetAllCarsHandler : IRequestHandler<GetAllCars, ICollection<CarDto>>
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public GetAllCarsHandler(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }
    
    public async Task<ICollection<CarDto>> Handle(GetAllCars request, CancellationToken cancellationToken)
    {
        var cars = await _carRepository.GetCars();
        
        return _mapper.Map<ICollection<CarDto>>(cars);
    }
}