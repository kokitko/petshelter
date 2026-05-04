using MediatR;
using PetShelter.Application.Common.Interfaces.Queries;
using PetShelter.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using ErrorOr;

namespace PetShelter.Application.Common.Behaviours;

public class CachingBehaviour<TRequest, TResponse>(
    ICacheService cacheService
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : IErrorOr
{
public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var valueType = typeof(TResponse).GetGenericArguments()[0];
        
        var cachedData = await cacheService.GetAsync(request.CacheKey, valueType, ct);

        if (cachedData != null)
        {
            return (dynamic)cachedData; 
        }

        var result = await next();

        if (!result.IsError)
        {
            await cacheService.SetAsync(request.CacheKey, ((dynamic)result).Value, request.Expiration, ct);
        }

        return result;
    }
}
