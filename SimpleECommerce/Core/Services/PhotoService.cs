using SimpleECommerce.Core.Interfaces;

namespace SimpleECommerce.Core.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly string _uploadsFolderPath;

        public PhotoService(IConfiguration configuration)
        {
            // Access and validate the uploads folder path from configuration
            _uploadsFolderPath = configuration["Photos:UploadsFolderPath"];
            if (string.IsNullOrEmpty(_uploadsFolderPath))
            {
                throw new InvalidOperationException("PhotoService: UploadsFolderPath not configured in appsettings.json.");
            }

            if (!Directory.Exists(_uploadsFolderPath))
            {
                Directory.CreateDirectory(_uploadsFolderPath);
            }
        }

        public async Task<PhotoResult> AddPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!IsValidImageType(file.ContentType))
            {
                throw new InvalidOperationException("Invalid file type. Only image formats are allowed.");
            }

            // Generate a unique filename with extension
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_uploadsFolderPath, fileName);

            // Save the file securely and efficiently
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(stream);
            }

            return new PhotoResult
            {
                Url = Path.Combine("/images", fileName), // Adjust URL based on your server setup
                Filename = fileName
            };
        }

        public async Task DeletePhotoAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            var filePath = Path.Combine(_uploadsFolderPath, filename);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        private bool IsValidImageType(string contentType)
        {
            return contentType.StartsWith("image/");
        }
    }

    public class PhotoResult
    {
        public string Url { get; set; }
        public string Filename { get; set; }
    }
}
