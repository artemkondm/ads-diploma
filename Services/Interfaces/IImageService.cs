namespace Ads.Services.Interfaces;

public interface IImageService
{
    Task<(string OriginalUrl, string? ThumbnailUrl)> UploadImageAsync(IFormFile file);
}