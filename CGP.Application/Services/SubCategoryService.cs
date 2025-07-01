using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.SubCategory;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService; 
        private static string FOLDER = "subCategories";
        public SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper, ISubCategoryRepository subCategoryRepository, ICloudinaryService cloudinaryService)
        {
            _subCategoryRepository = subCategoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<SubCategory> GetByName(string Name)
        {
            var result = await _subCategoryRepository.GetSubCategoryByName(Name);

            return result;
        }

        public async Task<Result<object>> GetById(Guid Id)
        {
            var result = _mapper.Map<ViewSubCategoryDTO>(await _unitOfWork.subCategoryRepository.GetSubCategoryById(Id));

            return new Result<object>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result,
            };
        }

        public async Task<Result<List<ViewSubCategoryDTO>>> GetSubs()
        {
            var result = _mapper.Map<List<ViewSubCategoryDTO>>(await _subCategoryRepository.GetSubCategories());

            return new Result<List<ViewSubCategoryDTO>>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result,
            };
        }

        public async Task<Result<object>> Update(Guid Id, string Name, int Status)
        {
            var subCategory = await _unitOfWork.subCategoryRepository.GetByIdAsync(Id);

            if (subCategory == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any category, please try again!",
                    Data = null
                };
            }

            subCategory.SubName = Name;

            _unitOfWork.subCategoryRepository.Update(subCategory);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Update category successfully" : "Update category fail",
                Data = subCategory
            };
        }

        public async Task<Result<object>> Create(CreateSubCategoryDTO request)
        {
            var subCategory = _mapper.Map<SubCategory>(request);
            var uploadResult = await _cloudinaryService.UploadProductImage(request.Image, FOLDER);
            subCategory.Image = uploadResult.SecureUrl.ToString();
            await _subCategoryRepository.AddAsync(subCategory);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Create category successfully" : "Create category fail",
                Data = result,
            };
        }

        public async Task<Result<object>> AddSubCategoryToCategory(CreateSubCategoryDTO request, Guid CategoryId)
        {
            var cateogry = await _unitOfWork.categoryRepository.GetCategoryById(CategoryId);

            if (cateogry == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any category, please try again!",
                    Data = null
                };
            }

            var subCategory = _mapper.Map<SubCategory>(request);
            var uploadResult = await _cloudinaryService.UploadProductImage(request.Image, FOLDER);
            subCategory.Image = uploadResult.SecureUrl.ToString();

            cateogry.SubCategories.Add(subCategory);

            _unitOfWork.categoryRepository.Update(cateogry);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Create category successfully" : "Create category fail",
                Data = subCategory
            };
        }

        public async Task<Result<SubCategory>> GetSubByIdWithoutMap(Guid Id)
        {
            var result = await _unitOfWork.subCategoryRepository.GetSubCategoryById(Id);

            return new Result<SubCategory>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result,
            };
        }

        public async Task<Result<object>> Delete(Guid Id)
        {
            var subCategory = await _subCategoryRepository.GetSubCategoryById(Id);

            if (subCategory == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any category, please try again!",
                    Data = null
                };
            }

            _subCategoryRepository.DeleteSubCategory(subCategory);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Delete category successfully" : "Delete category fail",
                Data = subCategory,
            };
        }
    }
}
