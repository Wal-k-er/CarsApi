using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CategoryFeatures.Commands;

public class UpdateCategory : IRequest<int>
{
    public int CategoryId { get; }
    public CategoryDto Model { get; }

    public UpdateCategory(int categoryId, CategoryDto categoryDto)
    {
        CategoryId = categoryId;
        Model = categoryDto;
    }
    
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategory, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository, 
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        
        public async Task<int> Handle(UpdateCategory request, CancellationToken cancellationToken)
        {
            if(!await _categoryRepository.CategoryExists(request.CategoryId))
                throw new EntityNotFoundException($"Не найдена категория {request.CategoryId}");
            
            CategoryDto model = request.Model;

            var entity = _mapper.Map<Category>(model);
            entity.Id = request.CategoryId;

            await _categoryRepository.UpdateCategory(entity);

            return entity.Id;
        }
    }
}