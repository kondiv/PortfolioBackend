using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixedDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_Permissions_permission_id",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "permission");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "permission",
                newName: "permission_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission",
                columns: new[] { "permission_id", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission",
                table: "permission",
                column: "permission_id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_permission_id",
                table: "role_permission",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "permission_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_permission_id",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission",
                table: "permission");

            migrationBuilder.RenameTable(
                name: "permission",
                newName: "Permissions");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                table: "Permissions",
                newName: "PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_Permissions_permission_id",
                table: "role_permission",
                column: "permission_id",
                principalTable: "Permissions",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
