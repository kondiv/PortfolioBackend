using Microsoft.AspNetCore.Identity;

namespace Models.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
