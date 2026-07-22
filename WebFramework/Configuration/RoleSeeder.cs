using Entites.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFramework.Configuration
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            string[] roles = { "Admin", "Customer" };
            foreach (var roleName in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new Role
                    {
                        Name = roleName,
                        Descerption = roleName == "Admin" ? "مدیر سیستم" : "مشتری فروشگاه",



                    });
                }
            }
            string adminUserName = "admin";
            string adminEmail = "admin@test.com";
            string adminPassword = "Admin123!";
            string adminFullName = "Admin";
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                var user = new User
                {
                    FullName = adminFullName,
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsStatic = true
                };
                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
