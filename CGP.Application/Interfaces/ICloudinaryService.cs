using CGP.Contract.Abstractions.CloudinaryService;
using CGP.Domain.Entities;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadProductImage(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
