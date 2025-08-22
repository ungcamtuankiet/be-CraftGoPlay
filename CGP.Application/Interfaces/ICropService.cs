using CGP.Contract.DTO.Crop;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ICropService
    {
        public Task<Result<ViewCropDTO>> GetCropByIdAsync(Guid cropId);
        public Task<Result<object>> AddCropAsync(AddCropDTO request);
        public Task<Result<object>> UpdateCropAsync(UpdateCropDTO request);
        public Task<Result<object>> RemoveCropAsync(Guid cropId);
    }
}
