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
                Message = result > 0 ? "Tạo danh mục thành công." : "Tạo danh mục thất bại.",
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
                    Message = "Không tìm thấy danh mục nào, vui lòng thử lại!",
                    Data = null
                };
            }

            _categoryRepository.DeleteCategory(Category);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Xóa danh mục thành công" : "Xóa danh mục không thành công",
                Data = result
            };
        }

        public async Task<Result<List<ViewCategoryDTO>>> GetCategory()
        {

            var result = _mapper.Map<List<ViewCategoryDTO>>(await _unitOfWork.categoryRepository.GetCategories());

            return new Result<List<ViewCategoryDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách danh mục thành công.",
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
                Message = "Lấy danh mục thành công",
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
                Message = result > 0 ? "Cập nhật danh mục thành công." : "Cập nhật danh mục thất bại.",
                Data = result
            };
        }

        public async Task<Result<object>> UpdateCategory(Guid id, UpdateCategoryDTO updateCategoryDTO)
        {
            var getCategory = await _categoryRepository.GetByIdAsync(id);
            if(getCategory == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy danh mục nào, vui lòng thử lại!",
                    Data = null
                };
            }
            await _cloudinaryService.DeleteImageAsync(getCategory.Image);
            var uploadResult = await _cloudinaryService.UploadProductImage(updateCategoryDTO.Image, FOLDER);
            getCategory.Image = uploadResult.SecureUrl.ToString();
            getCategory.CategoryName = updateCategoryDTO.CategoryName;
            getCategory.CategoryStatus = updateCategoryDTO.CategoryStatus;
            getCategory.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.categoryRepository.Update(getCategory);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Cập nhật danh mục thành công.",
                Data = getCategory
            };
        }
    }
}
