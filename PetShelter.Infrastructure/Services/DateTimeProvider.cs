using PetShelter.Application.Common.Interfaces.Services;

namespace PetShelter.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
