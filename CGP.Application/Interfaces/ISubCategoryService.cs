using CGP.Contract.DTO.SubCategory;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ISubCategoryService
    {
        public Task<Result<List<ViewSubCategoryDTO>>> GetSubs();
        public Task<Result<object>> GetById(Guid Id);
        public Task<Result<object>> UpdateSubCategoryAsync(Guid Id, UpdateSubCategoryDTO request);
        public Task<Result<SubCategory>> GetSubByIdWithoutMap(Guid Id);
        public Task<Result<object>> Create(CreateSubCategoryDTO request);
        public Task<Result<object>> Update(Guid Id, string Name, int Status);
        public Task<SubCategory> GetByName(string Name);
        public Task<Result<object>> AddSubCategoryToCategory(CreateSubCategoryDTO request, Guid CategoryId);
        public Task<Result<object>> Delete(Guid Id);
    }
}
