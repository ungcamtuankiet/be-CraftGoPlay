using CGP.Application.Interfaces;
using CGP.Contract.Abstractions.CloudinaryService;
using CGP.Domain.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
            _cloudinary.Api.Secure = true;
        }


        public async Task<ImageUploadResult> UploadProductImage(IFormFile file, string folder)
        {
            var uploadResult = new ImageUploadResult();

            // Kiểm tra kích thước tối đa: 2MB = 2 * 1024 * 1024 bytes
            const long maxFileSize = 2 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                throw new InvalidOperationException("Kích thước ảnh không được vượt quá 2MB.");
            }

            // Kiểm tra định dạng ảnh hợp lệ
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Định dạng ảnh không hợp lệ. Chỉ chấp nhận: .jpg, .jpeg, .png, .webp, .gif");
            }

            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
