namespace Models.Entities
{
    public class Permission
    {
        public int PermissionId { get; set; }

        public string Name { get; set; } = default!;

        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
