using SimpleECommerce.Core.Services;

namespace SimpleECommerce.Core.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoResult> AddPhotoAsync(IFormFile file);
        Task DeletePhotoAsync(string filename);
    }
}
