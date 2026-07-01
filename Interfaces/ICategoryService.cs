using RefundManager.DTOs;
using RefundManager.Models;

namespace RefundManager.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryOutputDto>> GetAllCategoriesWithBudget();
        Task<CategoryOutputDto> CreateCategoryAsync(CategoryInputDto categoryInputDto);

    }
}
