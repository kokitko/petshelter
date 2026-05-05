using PetShelter.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace PetShelter.Infrastructure.Services;

public class CacheService(
    IDistributedCache cache,
    IConnectionMultiplexer redis,
    ILogger<CacheService> logger) : ICacheService
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
        if (json == null) 
        {
            logger.LogWarning("Cache not found for key: {CacheKey}", key);
            return null;
        }

        logger.LogInformation("Cache found for key: {CacheKey}, deserializing value", key);
        return JsonSerializer.Deserialize(json, type, _options);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        logger.LogInformation("Removing cache for key: {CacheKey}", key);
        await cache.RemoveAsync(key, ct);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        try
        {
            logger.LogInformation("Removing cache entries with prefix: {CachePrefix}", prefix);
            var server = redis.GetServer(redis.GetEndPoints().FirstOrDefault() ?? throw new InvalidOperationException("No Redis endpoints available"));
            
            var keys = server.Keys(pattern: $"PetShelter_{prefix}*").ToList();
            
            if (keys.Count > 0)
            {
                logger.LogInformation("Removing {KeyCount} cache entries with prefix: {CachePrefix}", keys.Count, prefix);
                await Task.WhenAll(keys.Select(key => redis.GetDatabase().KeyDeleteAsync(key)));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing cache by prefix '{CachePrefix}': {ErrorMessage}", prefix, ex.Message);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        logger.LogInformation("Setting cache for key: {CacheKey} with expiration: {Expiration}", key, expiration);
        var json = JsonSerializer.Serialize(value, _options);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
        };
        await cache.SetStringAsync(key, json, options, ct);
        logger.LogInformation("Cache set successfully for key: {CacheKey}", key);
    }
}
