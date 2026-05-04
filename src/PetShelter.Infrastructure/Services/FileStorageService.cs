using Microsoft.AspNetCore.Http;
using PetShelter.Application.Common.Interfaces.Services;

namespace PetShelter.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _storagePath = Path.Combine(
        Directory.GetDirectoryRoot(
        Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData)), "FileStorage");

    public FileStorageService()
    {
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
