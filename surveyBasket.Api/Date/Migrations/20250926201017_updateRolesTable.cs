using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyBasket.Api.Date.Migrations
{
    /// <inheritdoc />
    public partial class updateRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a60d0c5-17cc-4a5a-bdd6-a2b303dda53a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a60d0c5-17cc-4a5a-bdd6-a2b303dda53a", 0, "38dfa186-8a84-416d-b54c-abcde043e7ca", "admin@example.com", true, "Amr", "khaled", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEMSnGobm5a3j9PYAm0j2HswoDKXBgTsbdq72ztpfNylab0qx/K4nMfwvSjwLZFa2sA==", null, false, "fd153d30-61be-4f03-afca-3b380c0e24da", false, "admin" });
        }
    }
}
