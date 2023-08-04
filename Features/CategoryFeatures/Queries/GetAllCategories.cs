using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CategoryFeatures.Queries;

public class GetAllCategories : IRequest<ICollection<CategoryDto>>
{
    
}

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, ICollection<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<ICollection<CategoryDto>> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetCategories();

        return _mapper.Map<ICollection<CategoryDto>>(categories);
    }
}