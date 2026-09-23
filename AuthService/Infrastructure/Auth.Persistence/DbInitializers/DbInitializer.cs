using Auth.Domain.Contracts;
using Auth.Domain.Entities;
using Auth.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        //if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        //{
        //    await dbContext.Database.MigrateAsync();
        //}

        await SeedRolesAsync();
        await SeedPermissionsAsync();
        await SeedAdminAsync();
        await SeedFarmerAsync();
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
        const string adminPhone = "+201120936540";
        const string adminEmail = "Admin@gmail.com";
        const string Password = "P@ssword123";

        var admin = await userManager.Users
            .FirstOrDefaultAsync(x => x.PhoneNumber == adminPhone);
    

        if (admin is not null)
            return;

        admin = new AppUser
        {
            FullName = "Admin User",
            UserName = adminPhone,
            PhoneNumber = adminPhone,
            Email=adminEmail,
           
            
        };

    

        var result = await userManager.CreateAsync( admin, Password);

        if (!result.Succeeded)
        {
            logger.LogError(
                string.Join(", ", result.Errors.Select(x => x.Description)));

            return;
        }

        await userManager.AddToRoleAsync(admin, "Admin");
    }

    private async Task SeedFarmerAsync()
    {
        const string farmerPhone = "+201060874564";
        const string farmerEmail = "mohamedelsawymh06@gmail.com";
        const string farmerFullName = "Mohamed Elsawy";
        const string password = "P@ssw0rd2026";

        // TODO: Region/Village weren't provided — filled in as empty for now.
        // Update these two if you have real values for this farmer.
        const string region = "";
        const string village = "";

        var farmer = await userManager.Users
            .FirstOrDefaultAsync(x => x.PhoneNumber == farmerPhone);

        if (farmer is not null)
            return;

        farmer = new AppUser
        {
            FullName = farmerFullName,
            UserName = farmerPhone,
            PhoneNumber = farmerPhone,
            Email = farmerEmail,
            pictures = string.Empty, // no image provided
            Address = new Address
            {
                Region = region,
                Village = village,
            },
        };

        var result = await userManager.CreateAsync(farmer, password);

        if (!result.Succeeded)
        {
            logger.LogError(
                string.Join(", ", result.Errors.Select(x => x.Description)));

            return;
        }

        await userManager.AddToRoleAsync(farmer, "Farmer");
    }
}