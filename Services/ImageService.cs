using System.Net.Mime;
using Ads.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Ads.Services;

public class ImageService : IImageService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "ads");

    public ImageService()
    {
        if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
    }
    public async Task<(string OriginalUrl, string? ThumbnailUrl)> UploadImageAsync(IFormFile file, bool createThumbnail = false)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(_storagePath, fileName);
        string? thumbName = null;
        
        using var image = await Image.LoadAsync(file.OpenReadStream());
        
        if (image.Width > 1920) image.Mutate(x => x.Resize(1920, 0));
        await image.SaveAsync(fullPath);
        
        if (createThumbnail)
        {
            thumbName = $"thumb_{fileName}";
            var thumbPath = Path.Combine(_storagePath, thumbName);
            
            using var thumbnail = image.Clone(x => x.Resize(300, 0));
            await thumbnail.SaveAsync(thumbPath);
        }
        return (fileName, thumbName);
    }
}