namespace PetShelter.Application.Common.Interfaces.Services;

public interface ICacheService
{
    Task<object?> GetAsync(string key, Type type, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
    Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default);
}
