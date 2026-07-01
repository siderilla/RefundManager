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
                    DailyBudget = c.DailyBudget,
                    MonthlyBudget = c.MonthlyBudget
                })
                .ToListAsync();

            return categoryList;
        }

        public async Task<CategoryOutputDto> CreateCategoryAsync(CategoryInputDto categoryInputDto)
        {
            var category = new Category
            {
                Name = categoryInputDto.Name,
                DailyBudget = categoryInputDto.DailyBudget,
                MonthlyBudget= categoryInputDto.MonthlyBudget
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryOutputDto
            {
                Id = category.Id,
                Name = category.Name,
                DailyBudget = category.DailyBudget,
                MonthlyBudget = category.MonthlyBudget
            };
        }

    }
}
