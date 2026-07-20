using E_Commerce.Domain.Contract;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class IdentityDataSeeder(
        StoreIdentityDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<IdentityDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
                if (PendingMigrations.Any())
                    await dbContext.Database.MigrateAsync(ct);
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!userManager.Users.Any())
                {
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Ibrahim Hsona",
                        UserName = "IbrahimHsona",
                        Email = "Hsona@gmail.com",
                        PhoneNumber = "1234567890",
                    };
                    var createUser = await userManager.CreateAsync(admin, "P@ssw0rd");

                    if (createUser.Succeeded)
                        await userManager.AddToRoleAsync(admin, "SuperAdmin");

                    else
                    {
                        var errors = string.Join(", ", createUser.Errors.Select(x => x.Description));
                        logger.LogWarning($"Failed to create user: {errors}");
                    }
                }
            }
            catch(Exception ex) 
            {
                logger.LogError(ex, "An error occurred while seeding identity data.");
                return;
            }

        }
    }
}
