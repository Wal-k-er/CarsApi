using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CategoryFeatures.Commands;

public class CreateCategory : IRequest<int>
{
    public CreateCategory(CategoryDto categoryDto)
    {
        Model = categoryDto;
    }

    public CategoryDto Model { get; }

    public class CreateCategoryHandler : IRequestHandler<CreateCategory, int>
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public CreateCategoryHandler(ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCategory request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            var entity = _mapper.Map<Category>(model);

            await _categoryRepository.CreateCategory(entity);

            return entity.Id;
        }
    }
}