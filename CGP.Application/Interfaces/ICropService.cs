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
        public Task<Result<List<ViewCropDTO>>> GetCropsByUserIdAsync(Guid userId);
        public Task<Result<ViewCropDTO>> GetCropByIdAsync(Guid cropId);
        public Task<Result<object>> PlantCropAsync(PlantCropDTO request);
        public Task<Result<object>> UpdateCropCropAsync(UpdateCropDTO request);
        public Task<Result<object>> WaterCropAsync(Guid cropId);
        public Task<Result<object>> HarvestCropAsync(Guid cropId);
        public Task<Result<object>> RemoveCropAsync(Guid cropId);
    }
}
