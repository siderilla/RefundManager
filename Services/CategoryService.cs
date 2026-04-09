using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;

namespace RefundManager.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly RefundManagerContext _context;

        public CategoryService(RefundManagerContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryOutputDto>> GetAllCategoriesWithBudget()
        {
            var categoryList = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryOutputDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    BudgetAmount = c.Budget
                })
                .ToListAsync();

            return categoryList;
        }

        public async Task<Category> CreateCategoryAsync(CategoryInputDto categoryInputDto)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryInputDto.Name,
                Budget = categoryInputDto.Budget
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

    }
}
