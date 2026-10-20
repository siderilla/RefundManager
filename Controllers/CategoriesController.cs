using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefundManager.Data;
using RefundManager.DTOs;
using RefundManager.Interfaces;
using RefundManager.Models;
using RefundManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefundManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;


        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories/with-budget
        [HttpGet("with-budget")]
        public async Task<ActionResult<IEnumerable<CategoryOutputDto>>> GetCategoriesWithBudget()
        {
            var allCatWithBudgets = await _categoryService.GetAllCategoriesWithBudget();
            return Ok(allCatWithBudgets);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostOutlayByEmployee([FromBody] CategoryInputDto categoryInputDto)
        {
            var newCategory = await _categoryService.CreateCategoryAsync(categoryInputDto);

            return Ok(new
            {
                Message = "New category added",
                Category = newCategory,
            });
        }
    }
}
