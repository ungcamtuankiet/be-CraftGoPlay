using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.Category;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        const string CATEGORY_CACHE_KEY = "category:list";

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository, IRedisService redisService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
        }

        public async Task<Result<object>> Create(Category category)
        {
            await _categoryRepository.AddAsync(category);

            var result = await _unitOfWork.SaveChangeAsync();

            if (result > 0)
            {
                await _redisService.RemoveCacheAsync("category:list");
            }


            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Create category successfully" : "Create category fail",
                Data = result
            };
        }

        public async Task<Result<object>> Delete(Guid Id)
        {
            var Category = await _unitOfWork.categoryRepository.GetByIdAsync(Id);

            if (Category == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any category, please try again!",
                    Data = null
                };
            }

            _categoryRepository.DeleteCategory(Category);

            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                await _redisService.RemoveCacheAsync("category:list");
            }

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Delete category successfully" : "Delete category fail",
                Data = result
            };
        }

        public async Task<Result<List<ViewCategoryDTO>>> GetCategory()
        {
            string cacheKey = "category:list";
            var cachedData = await _redisService.GetCacheAsync<List<ViewCategoryDTO>>(cacheKey);

            if (cachedData != null)
            {
                return new Result<List<ViewCategoryDTO>>
                {
                    Error = 0,
                    Message = "Get successfully (from cache)",
                    Data = cachedData
                };
            }

            var result = _mapper.Map<List<ViewCategoryDTO>>(await _unitOfWork.categoryRepository.GetCategories());

            //Lưu dữ liệu vào cache 10p
            await _redisService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return new Result<List<ViewCategoryDTO>>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result
            };
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            var result = await _categoryRepository.GetCategoryByName(name);

            return result;
        }

        public async Task<Result<ViewCategoryDTO>> GetCategoryIdWithMap(Guid Id)
        {
            var result = _mapper.Map<ViewCategoryDTO>(await _categoryRepository.GetByIdAsync(Id));

            return new Result<ViewCategoryDTO>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result
            };
        }

        public async Task<Category?> GetCategoryIdWithoutMap(Guid Id)
        {
            return await _categoryRepository.GetByIdAsync(Id);
        }

        public async Task<Result<object>> UpdateCateWithNameAndStatus(Category category, string cateName, int Status)
        {
            category.CategoryName = cateName;

            _categoryRepository.Update(category);

            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                await _redisService.RemoveCacheAsync("category:list");
            }
            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Update category successfully" : "Update category fail",
                Data = result
            };
        }
    }
}
