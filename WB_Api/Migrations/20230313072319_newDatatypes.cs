using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WB_Api.Migrations
{
    /// <inheritdoc />
    public partial class newDatatypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c878260-3917-48cc-8e80-ed3c7b6933ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab058d17-be4f-4c2a-9f2d-3185e3ae2050");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60e7fe85-16a0-4da2-8b95-b405ea6b845b", "f7160878-f9ea-4182-a5ad-5b7c1554bb92", "User", "USER" },
                    { "70ff3759-115d-4126-914c-07df4c98c355", "f3ca1887-e9fd-4209-888a-8adf7c7243e8", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60e7fe85-16a0-4da2-8b95-b405ea6b845b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70ff3759-115d-4126-914c-07df4c98c355");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3c878260-3917-48cc-8e80-ed3c7b6933ef", "b1ac28f4-35d0-4a68-b8f8-f471ff058bc3", "User", "USER" },
                    { "ab058d17-be4f-4c2a-9f2d-3185e3ae2050", "54e6b0ac-e671-4452-b999-99a47b048112", "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
