using Microsoft.AspNetCore.Identity;

namespace CleanMvcApp.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        string[] roles = { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        const string adminEmail = "admin@cleanmvc.com";
        const string adminPassword = "Admin@123456";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
