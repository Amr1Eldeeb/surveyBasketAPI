using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyBasket.Api.Date.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRolesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a39603a-b8c8-41d1-80b4-00c1b941e521");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "AspNetUsers",
                newName: "FirstName");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a60d0c5-17cc-4a5a-bdd6-a2b303dda53a", 0, "38dfa186-8a84-416d-b54c-abcde043e7ca", "admin@example.com", true, "Amr", "khaled", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEMSnGobm5a3j9PYAm0j2HswoDKXBgTsbdq72ztpfNylab0qx/K4nMfwvSjwLZFa2sA==", null, false, "fd153d30-61be-4f03-afca-3b380c0e24da", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a60d0c5-17cc-4a5a-bdd6-a2b303dda53a");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "Firstname");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Firstname", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a39603a-b8c8-41d1-80b4-00c1b941e521", 0, "b0b14b3c-3555-4532-aaee-ed1e39621ee1", "admin@example.com", true, "Amr", "khaled", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEORCijGVLDLhWhu5EkJ/8fuZ9F6UuCyEyOi98Km00GM4Iv6MAB5rPxvHmxeZKwWHlQ==", null, false, "edf00b60-308e-4dc0-b556-07bbd985c106", false, "admin" });
        }
    }
}
