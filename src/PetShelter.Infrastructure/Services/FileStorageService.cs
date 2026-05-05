using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PetShelter.Application.Common.Interfaces.Services;

namespace PetShelter.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _storagePath;

    public FileStorageService(IConfiguration configuration)
    {
        var configPath = configuration.GetValue<string>("FileStorage:Path");
        
        if (!string.IsNullOrEmpty(configPath))
        {
            _storagePath = configPath;
        }
        else
        {
            // fallback for a docker env
            var appDir = AppContext.BaseDirectory;
            _storagePath = Path.Combine(appDir, "FileStorage");
        }

        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task DeleteAsync(string fileUrl)
    {
        if (File.Exists(fileUrl))
            File.Delete(fileUrl);

        await Task.CompletedTask;
    }

    public async Task<string> UploadFileAsync(IFormFile fileStream, string fileName, string contextType)
    {
        var filePath = Path.Combine(_storagePath, fileName);

        using (var fileStreamOutput = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            await fileStream.CopyToAsync(fileStreamOutput);

        return await Task.FromResult(filePath);
    }
}
