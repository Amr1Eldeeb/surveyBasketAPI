using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyBasket.Api.Date.Migrations
{
    /// <inheritdoc />
    public partial class editDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Firstname", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d51cbdb1-1fba-486a-b504-b93a541e9ce4", 0, "d8d23389-c2b5-4c38-9c21-7cd32a5521c6", "admin@example.com", true, "Amr", "khaled", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAECOsct77KtDIquC+cbyxNE1DxMlXsweM0DGKIUsElu5FSRG5VC+tsoOPIektakMIrg==", null, false, "4b8af155-1a4d-4662-8d5f-1273c8c1f982", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d51cbdb1-1fba-486a-b504-b93a541e9ce4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Firstname", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "556cddf0-d460-4cfa-8c83-3415306974a1", "admin@example.com", true, "Amr", "khaled", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEJhVzhXrCfqq8pVJqNAZEBVZ+342lasXSYp0kb4CkX80+oCFst8gF5Ti5CARa2yOlA==", null, false, "a1383c7f-52cc-48cb-bee5-58c69af4c520", false, "admin" });
        }
    }
}
