using Microsoft.AspNetCore.Identity;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Data.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            await roleManager.CreateAsync(new AppRole(Roles.Admin.ToString())); 
        }
    }
}
