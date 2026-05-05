using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Services;

namespace PetShelter.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _storagePath;
    private readonly ILogger<FileStorageService> _logger;
    public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
    {
        _logger = logger;

        var configPath = configuration.GetValue<string>("FileStorage:Path");
        _logger.LogInformation("Initializing FileStorageService with configuration: {FileStoragePath}", configPath);

        if (!string.IsNullOrEmpty(configPath))
        {
            _logger.LogInformation("Using configured file storage path: {FileStoragePath}", configPath);
            _storagePath = configPath;
        }
        else
        {
            _logger.LogWarning("No file storage path configured, using default path under application directory");
            var appDir = AppContext.BaseDirectory;
            _storagePath = Path.Combine(appDir, "FileStorage");
        }

        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task DeleteAsync(string fileUrl)
    {
        try {
            _logger.LogInformation("Attempting to delete file at URL: {FileUrl}", fileUrl);
            if (File.Exists(fileUrl))
            {
                File.Delete(fileUrl);
                _logger.LogInformation("File deleted successfully from URL: {FileUrl}", fileUrl);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting file from URL: {FileUrl}", fileUrl);
            throw;
        }

        await Task.CompletedTask;
    }

    public async Task<string> UploadFileAsync(IFormFile fileStream, string fileName, string contextType)
    {
        var filePath = Path.Combine(_storagePath, fileName);

        try {
            _logger.LogInformation("Uploading file with name: {FileName} to path: {FilePath}", fileName, filePath);
            using (var fileStreamOutput = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(fileStreamOutput);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading file with name: {FileName} to path: {FilePath}", fileName, filePath);
            throw;
        }

        return await Task.FromResult(filePath);
    }
}
