using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CategoryFeatures.Queries;

public class GetCategoryById : IRequest<CategoryDto>
{
    public int Id { get; }

    public GetCategoryById(int id)
    {
        Id = id;
    }
}

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryById, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<CategoryDto> Handle(GetCategoryById request, CancellationToken cancellationToken)
    {
        if (!await _categoryRepository.CategoryExists(request.Id))
            throw new EntityNotFoundException($"Не найдена категория {request.Id}");
        
        var category = await _categoryRepository.GetCategory(request.Id);

        return _mapper.Map<CategoryDto>(category);
    }
}