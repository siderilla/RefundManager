using RefundManager.DTOs;
using RefundManager.Models;

namespace RefundManager.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryOutputDto>> GetAllCategoriesWithBudget();
        Task<Category> CreateCategoryAsync(CategoryInputDto categoryInputDto);

    }
}
