using Microsoft.AspNetCore.Http;

namespace PetShelter.Application.Common.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile fileStream, string fileName, string contextType);
    Task DeleteAsync(string fileUrl);
}
