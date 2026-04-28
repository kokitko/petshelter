using Microsoft.EntityFrameworkCore;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence;

public class PetShelterDbContext(DbContextOptions<PetShelterDbContext> options) : DbContext(options)
{
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<PetImage> PetImages => Set<PetImage>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AdoptionApplication> AdoptionApplications => Set<AdoptionApplication>();
    public DbSet<OrgProfile> OrgProfiles => Set<OrgProfile>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
}
