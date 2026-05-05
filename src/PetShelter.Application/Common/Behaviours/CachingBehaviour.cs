using MediatR;
using PetShelter.Application.Common.Interfaces.Queries;
using PetShelter.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using ErrorOr;

namespace PetShelter.Application.Common.Behaviours;

public class CachingBehaviour<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<TRequest> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var valueType = typeof(TResponse).GetGenericArguments()[0];
        var cacheKey = request.CacheKey;
        
        var cachedData = await cacheService.GetAsync(cacheKey, valueType, ct);

        if (cachedData != null)
        {
            logger.LogInformation("Cache HIT for key: {CacheKey}", cacheKey);
            return (dynamic)cachedData;
        }

        logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);
        var result = await next();

        if (!result.IsError)
        {
            await cacheService.SetAsync(cacheKey, ((dynamic)result).Value, request.Expiration, ct);
            logger.LogInformation("Cache SET for key: {CacheKey} with expiration: {Expiration}", 
                cacheKey, request.Expiration);
        }

        return result;
    }
}
