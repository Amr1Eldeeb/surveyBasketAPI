using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyBasket.Api.Date.Migrations
{
    /// <inheritdoc />
    public partial class adduserToData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Firstname", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a39603a-b8c8-41d1-80b4-00c1b941e521", 0, "b0b14b3c-3555-4532-aaee-ed1e39621ee1", "admin@example.com", true, "Amr", "khaled", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEORCijGVLDLhWhu5EkJ/8fuZ9F6UuCyEyOi98Km00GM4Iv6MAB5rPxvHmxeZKwWHlQ==", null, false, "edf00b60-308e-4dc0-b556-07bbd985c106", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2a39603a-b8c8-41d1-80b4-00c1b941e521");
        }
    }
}
