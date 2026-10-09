
using Auth.Domain.Contracts;
using Auth.Domain.Entities;
using Auth.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Auth.Persistence.DbInitializers;

internal class DbInitializer(
    AppDbContext dbContext,
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager,
    ILogger<DbInitializer> logger)
    : IDbInitializer
{
    public async Task InitializeAsync()
    {
        await SeedRolesAsync();
        await SeedPermissionsAsync();
        await SeedAdminAsync();

        await SeedFarmersAsync();
        await SeedExpertsAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roles =
        {
            "Admin",
            "Supervisor",
            "Expert",
            "Technician",
            "Farmer"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole
                {
                    Name = role,
                    Description = $"{role} Role"
                });
            }
        }
    }

    private async Task SeedPermissionsAsync()
    {
        if (await dbContext.Permissions.AnyAsync())
            return;

        var permissions = new List<Permission>
        {
            new() { Name = "users.manage", Description = "Manage Users" },
            new() { Name = "roles.manage", Description = "Manage Roles" },
            new() { Name = "reports.create", Description = "Create Reports" },
            new() { Name = "reports.view", Description = "View Reports" },
            new() { Name = "reports.update", Description = "Update Reports" },
            new() { Name = "reports.delete", Description = "Delete Reports" }
        };

        await dbContext.Permissions.AddRangeAsync(permissions);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedAdminAsync()
    {
        const string phone = "+201120936540";
        const string email = "Admin@gmail.com";
        const string password = "P@ssword123";

        await SeedUserAsync(
            fullName: "Admin User",
            phone: phone,
            email: email,
            password: password,
            role: "Admin");
    }

    private async Task SeedFarmersAsync()
    {
        var farmers = new[]
        {
            new
            {
                FullName = "Mohamed Elsawy",
                Phone = "+201060874564",
                Email = "mohamed.elsawy@gmail.com"
            },
            new
            {
                FullName = "Ahmed Hassan",
                Phone = "+201010000001",
                Email = "ahmed.hassan@gmail.com"
            },
            new
            {
                FullName = "Mahmoud Ali",
                Phone = "+201010000002",
                Email = "mahmoud.ali@gmail.com"
            }
        };

        foreach (var farmer in farmers)
        {
            await SeedUserAsync(
                fullName: farmer.FullName,
                phone: farmer.Phone,
                email: farmer.Email,
                password: "P@ssw0rd2026",
                role: "Farmer");
        }
    }

    private async Task SeedExpertsAsync()
    {
        var experts = new[]
        {
            new
            {
                FullName = "Ahmed Expert",
                Phone = "+201010000003",
                Email = "ahmed.expert@gmail.com"
            },
            new
            {
                FullName = "Mohamed Expert",
                Phone = "+201010000004",
                Email = "mohamed.expert@gmail.com"
            },
            new
            {
                FullName = "Omar Expert",
                Phone = "+201010000005",
                Email = "omar.expert@gmail.com"
            },
            new
            {
                FullName = "Youssef Expert",
                Phone = "+201010000006",
                Email = "youssef.expert@gmail.com"
            },
            new
            {
                FullName = "Khaled Expert",
                Phone = "+201010000007",
                Email = "khaled.expert@gmail.com"
            },
            new
            {
                FullName = "Mahmoud Expert",
                Phone = "+201010000008",
                Email = "mahmoud.expert@gmail.com"
            },
            new
            {
                FullName = "Mostafa Expert",
                Phone = "+201010000009",
                Email = "mostafa.expert@gmail.com"
            }
        };

        foreach (var expert in experts)
        {
            await SeedUserAsync(
                fullName: expert.FullName,
                phone: expert.Phone,
                email: expert.Email,
                password: "P@ssw0rd2026",
                role: "Expert");
        }
    }

    private async Task SeedUserAsync(
        string fullName,
        string phone,
        string email,
        string password,
        string role)
    {
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(x => x.PhoneNumber == phone);

        if (existingUser is not null)
            return;

        var user = new AppUser
        {
            FullName = fullName,
            UserName = phone,
            PhoneNumber = phone,
            Email = email,
            pictures = string.Empty,
            Address = new Address
            {
                Region = string.Empty,
                Village = string.Empty
            }
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            logger.LogError(
                "Failed to create {Role} {Phone}: {Errors}",
                role,
                phone,
                string.Join(", ", result.Errors.Select(x => x.Description)));

            return;
        }

        var roleResult = await userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            logger.LogError(
                "Failed to add {Phone} to role {Role}: {Errors}",
                phone,
                role,
                string.Join(", ", roleResult.Errors.Select(x => x.Description)));
        }
    }
}
