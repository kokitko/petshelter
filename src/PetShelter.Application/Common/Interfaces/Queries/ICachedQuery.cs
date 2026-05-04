namespace PetShelter.Application.Common.Interfaces.Queries;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
