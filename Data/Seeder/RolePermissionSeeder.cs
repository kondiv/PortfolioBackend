using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.SeedData;

namespace Data.Seeder
{
    public class RolePermissionSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly PortfolioContext _context;

        private readonly Dictionary<string, List<string>> rolePermissions = new Dictionary<string, List<string>>()
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
            PortfolioContext context)
        {
            _roleManager = roleManager;
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
                var exist = await _context.Permissions.AnyAsync(p => p.Name == permissionName);

                if (!exist)
                {
                    await AddPermissionAsync(permissionName);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedRolesAsync()
        {
            foreach(var roleName in DefaultRoles.All())
            {
                var exist = await _context.Roles.AnyAsync(r => r.Name == roleName);

                if(!exist)
                {
                    await AddRoleAsync(roleName);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedRolePermissions()
        {
            var relevantRoles = await GetRelevantRolesAsync();

            foreach(var (roleName, permissions) in rolePermissions)
            {
                var role = relevantRoles.First(r => r.Name == roleName);

                foreach(var permissionName in permissions)
                {
                    await AddRolePermissionIfNotExistAsync(role, permissionName);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task AddPermissionAsync(string permissionName)
        {
            var permission = new Permission()
            {
                Name = permissionName
            };

            await _context.Permissions.AddAsync(permission);
        }

        private async Task AddRoleAsync(string roleName)
        {
            var role = new ApplicationRole()
            {
                Name = roleName,
            };

            await _roleManager.CreateAsync(role);
        }

        private async Task<List<ApplicationRole>> GetRelevantRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        private async Task AddRolePermissionIfNotExistAsync(ApplicationRole role, string permissionName)
        {
            var permission = await _context.Permissions.FirstAsync(p => p.Name == permissionName);

            bool exist = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleId == role.Id && rp.PermissionId == permission.PermissionId);

            if(!exist)
            {
                await AddRolePermissionAsync(role.Id, permission.PermissionId);
            }
        }

        private async Task AddRolePermissionAsync(string roleId, int permissionId)
        {
            var rolePermission = new RolePermission()
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await _context.RolePermissions.AddAsync(rolePermission);
        }
    }
}
