using System;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace RSVP.Application.Interfaces;

public interface IPhotoService
{
Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
Task<IEnumerable<ImageUploadResult>> AddPhotosAsync(IEnumerable<IFormFile> files);
Task<DeletionResult> DeletePhotoAsync(string publicId);
Task<long> DeleteMultiplePhotosAsync(IEnumerable<string> publicIds);
}
    