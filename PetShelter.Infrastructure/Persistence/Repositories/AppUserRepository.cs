using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class AppUserRepository(PetShelterDbContext context) : IAppUserRepository
{
    public async Task AddAsync(AppUser user)
    {
        context.AppUsers.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        var user = await context.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public async Task<AppUser?> GetByIdAsync(Guid id)
    {
        var user = await context.AppUsers
            .Include(u => u.OrgProfile)
            .Include(u => u.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async Task UpdateAsync(AppUser user)
    {
        context.AppUsers.Update(user);
        await context.SaveChangesAsync();
    }
}
