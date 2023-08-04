using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CategoryFeatures.Commands;

public class DeleteCategory : IRequest<int>
{
    public DeleteCategory(int catId)
    {
        Id = catId;
    }

    public int Id { get; }

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategory, int>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            if (!await _categoryRepository.CategoryExists(request.Id))
                throw new EntityNotFoundException($"Не найдена категория {request.Id}");

            var category = await _categoryRepository.GetCategory(request.Id);

            await _categoryRepository.DeleteCategory(category);

            return request.Id;
        }
    }
}