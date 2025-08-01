﻿using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Category;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _categoryService.GetCategory();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateCategory")]
        [ProducesResponseType(204, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDTO CategoryCreate)
        {
            if (CategoryCreate == null)
                return BadRequest(ModelState);

            if (await _categoryService.GetCategoryByName(CategoryCreate.CategoryName) != null)
            {
                ModelState.AddModelError("", "Danh mục đã tồn tại.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.Create(CategoryCreate);

            return Ok(result);
        }

        [HttpPut("UpdateCategory/{categoryId}")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromForm] UpdateCategoryDTO updateCategoryDTO)
        {
            var result = await _categoryService.UpdateCategory(categoryId, updateCategoryDTO);
            if(result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpDelete("CategoryId/Delete")]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(204, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteCategory([FromQuery] string CategoryId)
        {
            Guid id;

            try
            {
                id = new(CategoryId);
            }
            catch
            {
                return NotFound("Danh mục này không tồn tại!!!");
            }

            var result = await _categoryService.Delete(id);

            return Ok(result);
        }
    }
}

