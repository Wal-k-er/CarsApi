using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CarFeatures.Commands;

public class DeleteCar: IRequest<int>
{
    public int Id { get; }
    
    public DeleteCar(int carId)
    {
        Id = carId;
    }
    
    public class DeleteCarHandler : IRequestHandler<DeleteCar, int>
    {
        private readonly ICarRepository _carRepository;

        public DeleteCarHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        
        public async Task<int> Handle(DeleteCar request, CancellationToken cancellationToken)
        {
            if(!await _carRepository.CarExists(request.Id))
                throw new EntityNotFoundException($"Не найдена машина {request.Id}");

            var car = await _carRepository.GetCar(request.Id);
            
            await _carRepository.DeleteCar(car);
            
            return request.Id;
        }
    }
}