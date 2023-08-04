using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CarFeatures.Commands;

public class CreateCar : IRequest<int>
{
    public CreateCar(CarDto carDto, int catId)
    {
        Model = carDto;
        CategoryId = catId;
    }

    public CarDto Model { get; }
    public int CategoryId { get; }

    public class CreateCarHandler : IRequestHandler<CreateCar, int>
    {
        private readonly ICarRepository _carRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public CreateCarHandler(ICarRepository carRepository, ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCar request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            var entity = _mapper.Map<Car>(model);

            if (!await _categoryRepository.CategoryExists(request.CategoryId))
                throw new EntityNotFoundException($"Не найдена категория {request.CategoryId}");

            entity.Category = await _categoryRepository.GetCategory(request.CategoryId);

            await _carRepository.CreateCar(entity);

            return entity.Id;
        }
    }
}