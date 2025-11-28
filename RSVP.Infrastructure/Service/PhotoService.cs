using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RSVP.Application.Interfaces;

namespace RSVP.Infrastructure.Service;

public class PhotoService:IPhotoService
{
private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        ImageUploadResult uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    
    public async Task<IEnumerable<ImageUploadResult>> AddPhotosAsync(IEnumerable<IFormFile> files)
    {
        // specific list to hold the tasks that haven't finished yet
        var uploadTasks = new List<Task<ImageUploadResult>>();

        foreach (var file in files)
        {
           
            uploadTasks.Add(AddPhotoAsync(file));
        }

     
        var results = await Task.WhenAll(uploadTasks);

        return results;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }
    public async Task<long> DeleteMultiplePhotosAsync(IEnumerable<string> publicIds)
    {
        var deleteParams = new DelResParams()
        {
            PublicIds = publicIds.ToList()
        };
        var result = await _cloudinary.DeleteResourcesAsync(deleteParams);
        return result.Deleted.Count;
    }
}
