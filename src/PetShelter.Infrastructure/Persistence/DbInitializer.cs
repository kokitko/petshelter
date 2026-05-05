using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetShelter.Domain.Entities;
using PetShelter.Infrastructure.Authentication;

namespace PetShelter.Infrastructure.Persistence;

public static class DbInitializer
{
    private static PasswordHasher _passwordHasher = new PasswordHasher();
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetShelterDbContext>();

        await context.Database.MigrateAsync();

        if (!context.AppUsers.Any())
        {
            context.AppUsers.AddRange(
            new AppUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Email = "adminemail@gmail.com",
                PasswordHash = _passwordHasher.HashPassword("password"),
                PhoneNumber = "1234567890",
                Role = UserRole.Admin
            },
            new AppUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Email = "orgemail@gmail.com",
                PasswordHash = _passwordHasher.HashPassword("password"),
                PhoneNumber = "0987654321",
                Role = UserRole.Organization
            },
            new AppUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Email = "orgemail2@gmail.com",
                PasswordHash = _passwordHasher.HashPassword("password"),
                PhoneNumber = "0987654321",
                Role = UserRole.Organization
            },
            new AppUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Email = "useremail@gmail.com",
                PasswordHash = _passwordHasher.HashPassword("password"),
                PhoneNumber = "1234567890",
                Role = UserRole.User
            }
            );
        }

        if (!context.UserProfiles.Any())
        {
            context.UserProfiles.AddRange(
                new UserProfile
                {
                    Id = Guid.Parse("00000000-0000-0000-0001-000000000001"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    FirstName = "John",
                    LastName = "Doe 2"
                },
                new UserProfile
                {
                    Id = Guid.Parse("00000000-0000-0000-0001-000000000002"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    FirstName = "John",
                    LastName = "Doe"
                }
            );
        }

        if (!context.OrgProfiles.Any())
        {
            context.OrgProfiles.AddRange(
                new OrgProfile
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000001"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    OrgName = "Happy Doggy",
                    Address = "Zhelazna 23, Warsaw",
                    Website = "https://happydoggy.pl",
                    IsVerified = true
                },
                new OrgProfile
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000002"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    OrgName = "Happy Kitty",
                    Address = "Wawelska 15, Warsaw",
                    Website = "https://happykitty.pl",
                    IsVerified = false
                }
            );
        }

        if (!context.Pets.Any())
        {
            context.Pets.AddRange(
                new Pet
                {
                    Id = Guid.Parse("00000000-0000-0001-0000-000000000001"),
                    Name = "Bella",
                    Species = "Dog",
                    Breed = "Labrador Retriever",
                    Age = 4,
                    Description = "Loyal and friendly dog",
                    Status = PetStatus.Pending,
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                },
                new Pet
                {
                    Id = Guid.Parse("00000000-0000-0001-0000-000000000002"),
                    Name = "Buddy",
                    Species = "Dog",
                    Breed = "Golden Retriever",
                    Age = 3,
                    Description = "Friendly and energetic dog",
                    Status = PetStatus.Available,
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                },
                new Pet
                {
                    Id = Guid.Parse("00000000-0000-0001-0000-000000000003"),
                    Name = "Mittens",
                    Species = "Cat",
                    Breed = "Siamese",
                    Age = 2,
                    Description = "Calm and affectionate cat",
                    Status = PetStatus.Pending,
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                },
                new Pet
                {
                    Id = Guid.Parse("00000000-0000-0001-0000-000000000004"),
                    Name = "Charlie",
                    Species = "Dog",
                    Breed = "Beagle",
                    Age = 4,
                    Description = "Loyal and curious dog",
                    Status = PetStatus.Adopted,
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                },
                new Pet
                {
                    Id = Guid.Parse("00000000-0000-0001-0000-000000000005"),
                    Name = "Luna",
                    Species = "Cat",
                    Breed = "Persian",
                    Age = 1,
                    Description = "Playful and independent cat",
                    Status = PetStatus.Available,
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000003")
                },
                new Pet
                {
                    Id = Guid.Parse("00000000-0000-0001-0000-000000000006"),
                    Name = "Max",
                    Species = "Dog",
                    Breed = "Bulldog",
                    Age = 5,
                    Description = "Gentle and loving dog",
                    Status = PetStatus.Available,
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000003")
                }
            );
        }
        if (!context.PetImages.Any())
        {            
            context.PetImages.AddRange(
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000001"),
                    Url = "https://example.com/images/bella.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000001"),
                    IsMain = true
                },
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000002"),
                    Url = "https://example.com/images/bella2.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000001"),
                    IsMain = false
                },
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000003"),
                    Url = "https://example.com/images/buddy.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000002"),
                    IsMain = true
                },
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000004"),
                    Url = "https://example.com/images/mittens.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000003"),
                    IsMain = true
                },
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000005"),
                    Url = "https://example.com/images/charlie.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000004"),
                    IsMain = true
                },
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000006"),
                    Url = "https://example.com/images/luna.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000005"),
                    IsMain = true
                },
                new PetImage
                {
                    Id = Guid.Parse("00000000-0001-0001-0000-000000000007"),
                    Url = "https://example.com/images/max.jpg",
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000006"),
                    IsMain = true
                }
            );
        }
        if (!context.AdoptionApplications.Any())
        {
            context.AdoptionApplications.AddRange(
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000001"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000001"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Status = ApplicationStatus.Rejected
                },
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000002"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000001"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = ApplicationStatus.Approved
                },
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000003"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000001"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Status = ApplicationStatus.Pending 
                },
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000004"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000002"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Status = ApplicationStatus.Pending
                },
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000005"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000002"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = ApplicationStatus.Pending
                },
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000006"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000003"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Status = ApplicationStatus.Rejected
                },
                new AdoptionApplication
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000007"),
                    PetId = Guid.Parse("00000000-0000-0001-0000-000000000004"),
                    ApplicantId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = ApplicationStatus.Approved
                }
            );
        }

        await context.SaveChangesAsync();
    }
}
