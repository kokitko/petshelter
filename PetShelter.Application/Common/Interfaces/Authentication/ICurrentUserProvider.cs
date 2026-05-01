namespace PetShelter.Application.Common.Interfaces.Authentication;

public interface ICurrentUserProvider
{
    Guid? GetCurrentUserId();
}
