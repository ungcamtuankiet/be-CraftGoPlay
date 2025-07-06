using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Category;
using CGP.Contract.DTO.SubCategory;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(IMapper mapper, ISubCategoryService subCategoryService)
        {
            _mapper = mapper;
            _subCategoryService = subCategoryService;
        }

        [HttpGet("GetAllSubCategories")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAllSubCategories()
        {
            var result = await _subCategoryService.GetSubs();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }
        [HttpPost("CreateSubCategory")]
        [ProducesResponseType(204, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateSubCategory([FromForm] CreateSubCategoryDTO CategorySubCreate, string CategoryId)
        {
            if (CategorySubCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _subCategoryService.GetByName(CategorySubCreate.SubName) != null)
            {
                ModelState.AddModelError("", "Danh mục con này đã tồn tại");
                return StatusCode(422, ModelState);
            }

            Guid Id;

            try
            {
                Id = new Guid(CategoryId);
            }
            catch
            {
                return BadRequest("Không có danh mục nào có mã này.");
            }

            var result = await _subCategoryService.AddSubCategoryToCategory(CategorySubCreate, Id);

            return Ok(result);
        }

        [HttpDelete("SubCategoryId/Delete")]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(204, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteSubCategory(string ID)
        {
            Guid SubcategoryId;

            try
            {
                SubcategoryId = new(ID);
            }
            catch
            {
                return NotFound("Danh mục con này không tồn tại!!!");
            }

            var result = await _subCategoryService.Delete(SubcategoryId);

            if (result.Data is null)
            {
                return StatusCode(400, result);
            }

            return Ok(result);
        }
    }
}

