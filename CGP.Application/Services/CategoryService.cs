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
using static System.Net.Mime.MediaTypeNames;

namespace CGP.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private static string FOLDER = "categorys";

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository, IRedisService redisService, ICloudinaryService cloudinaryService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<object>> Create(CreateCategoryDTO request)
        {
            var category = _mapper.Map<Category>(request);
            var uploadResult = await _cloudinaryService.UploadProductImage(request.Image, FOLDER);
            category.Image = uploadResult.SecureUrl.ToString();
            await _categoryRepository.AddAsync(category);

            var result = await _unitOfWork.SaveChangeAsync();

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

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Delete category successfully" : "Delete category fail",
                Data = result
            };
        }

        public async Task<Result<List<ViewCategoryDTO>>> GetCategory()
        {

            var result = _mapper.Map<List<ViewCategoryDTO>>(await _unitOfWork.categoryRepository.GetCategories());

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
