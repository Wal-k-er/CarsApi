using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CarFeatures.Commands;

public class UpdateCar : IRequest<int>
{
    public UpdateCar(int carId, CarDto carDto)
    {
        CarId = carId;
        Model = carDto;
    }

    public int CarId { get; }
    public CarDto Model { get; }

    public class UpdateCarHandler : IRequestHandler<UpdateCar, int>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public UpdateCarHandler(ICarRepository carRepository,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCar request, CancellationToken cancellationToken)
        {
            if (!await _carRepository.CarExists(request.CarId))
                throw new EntityNotFoundException($"Не найдена машина {request.CarId}");

            var model = request.Model;

            var entity = _mapper.Map<Car>(model);
            entity.Id = request.CarId;

            await _carRepository.UpdateCar(entity);

            return entity.Id;
        }
    }
}