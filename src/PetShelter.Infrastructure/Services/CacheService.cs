using PetShelter.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using StackExchange.Redis;

namespace PetShelter.Infrastructure.Services;

public class CacheService(IDistributedCache cache, IConnectionMultiplexer redis) : ICacheService
{
    private static readonly JsonSerializerOptions _options = new() 
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<object?> GetAsync(string key, Type type, CancellationToken ct = default)
    {
        var json = await cache.GetStringAsync(key, ct);
        if (json == null) return null;

        return JsonSerializer.Deserialize(json, type, _options);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await cache.RemoveAsync(key, ct);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        try
        {
            var server = redis.GetServer(redis.GetEndPoints().FirstOrDefault() ?? throw new InvalidOperationException("No Redis endpoints available"));
            
            var keys = server.Keys(pattern: $"PetShelter_{prefix}*").ToList();
            
            if (keys.Count > 0)
            {
                await Task.WhenAll(keys.Select(key => redis.GetDatabase().KeyDeleteAsync(key)));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error removing cache by prefix '{prefix}': {ex.Message}");
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(value, _options);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
        };
        await cache.SetStringAsync(key, json, options, ct);
    }
}
