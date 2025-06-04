using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.SeedData;

namespace Data.Seeder
{
    public class RolePermissionSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PortfolioContext _context;

        private Dictionary<string, List<string>> rolePermissions = new Dictionary<string, List<string>>()
        {
            [DefaultRoles.Guest] = [DefaultPermissions.ViewProjects],
            [DefaultRoles.Developer] =
            [
                DefaultPermissions.ViewProjects,
                DefaultPermissions.InteractWithEmployers,
                DefaultPermissions.InteractWithProjects
            ],
            [DefaultRoles.Employer] =
            [
                DefaultPermissions.ViewProjects,
                DefaultPermissions.InteractWithDevelopers,
                DefaultPermissions.InteractWithProjects,
            ],
            [DefaultRoles.Admin] = DefaultPermissions.All()
        };

        public RolePermissionSeeder(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            PortfolioContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            await SeedPermissionsAsync();
            await SeedRolesAsync();
            await SeedRolePermissions();
        }

        private async Task SeedPermissionsAsync()
        {
            foreach(var permissionName in DefaultPermissions.All())
            {
                if (!await _context.Permissions.AnyAsync(p => p.Name == permissionName))
                {
                    await _context.AddAsync(new Permission() { Name = permissionName });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedRolesAsync()
        {
            foreach(var roleName in DefaultRoles.All())
            {
                bool exist = await _context.Roles.AnyAsync(r => r.Name == roleName);

                if(!exist)
                {
                    await AddRoleAsync(roleName);
                }
            }
        }

        private async Task SeedRolePermissions()
        {
            
        }

        private async Task AddRoleAsync(string roleName)
        {
            var role = new ApplicationRole()
            {
                Name = roleName,
            };

            await _roleManager.CreateAsync(role);
        }
    }
}
