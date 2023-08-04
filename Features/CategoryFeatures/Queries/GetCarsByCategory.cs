using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CategoryFeatures.Queries;

public class GetCarsByCategory : IRequest<ICollection<CarDto>>
{
    public int Id { get; }

    public GetCarsByCategory(int id)
    {
        Id = id;
    }
}

public class GetCarsByCategoryHandler : IRequestHandler<GetCarsByCategory, ICollection<CarDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCarsByCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<ICollection<CarDto>> Handle(GetCarsByCategory request, CancellationToken cancellationToken)
    {
        if(!await _categoryRepository.CategoryExists(request.Id))
            throw new EntityNotFoundException($"Не найдена категория {request.Id}");

        var cars = await _categoryRepository.GetCarsByCategory(request.Id);

        return _mapper.Map<ICollection<CarDto>>(cars);
    }
}