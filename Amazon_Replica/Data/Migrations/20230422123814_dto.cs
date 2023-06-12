using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon_Replica.Data.Migrations
{
    /// <inheritdoc />
    public partial class dto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "Identity",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Identity",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "Identity",
                newName: "UserLogins");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "Identity",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Identity",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "Identity",
                newName: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "Identity",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Identity",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "CustomUser",
                schema: "Identity",
                newName: "CustomUser");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Identity",
                newName: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserTokens",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "UserLogins",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaims",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "User",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "RoleClaims",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Role",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "CustomUser",
                newName: "CustomUser",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "Identity");
        }
    }
}
