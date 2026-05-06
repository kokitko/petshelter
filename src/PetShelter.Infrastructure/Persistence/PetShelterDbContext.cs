using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Domain.Common.Models;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence;

public class PetShelterDbContext(
    DbContextOptions<PetShelterDbContext> options) : DbContext(options)
{
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<PetImage> PetImages => Set<PetImage>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AdoptionApplication> AdoptionApplications => Set<AdoptionApplication>();
    public DbSet<OrgProfile> OrgProfiles => Set<OrgProfile>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<Pet>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Pets)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<PetImage>()
            .HasOne(pi => pi.Pet)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<AdoptionApplication>()
            .HasOne(aa => aa.Pet)
            .WithMany(p => p.Applications)
            .HasForeignKey(aa => aa.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<AdoptionApplication>()
            .HasOne(aa => aa.Applicant)
            .WithMany(u => u.Applications)
            .HasForeignKey(aa => aa.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<OrgProfile>()
            .HasOne(op => op.User)
            .WithOne(u => u.OrgProfile)
            .HasForeignKey<OrgProfile>(op => op.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<UserProfile>()
            .HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        mb.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
