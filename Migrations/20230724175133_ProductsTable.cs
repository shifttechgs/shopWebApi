using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopWebApi.Migrations
{
    /// <inheritdoc />
    public partial class ProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "52af7903-d3ff-45ec-bad8-6fe4dbc0d0fd");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "68e2ddb1-6fa9-4198-b808-90655e7e2442");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "537db965-defa-495d-a00a-4e21791d0c37", null, "Visitor", "VISITOR" },
                    { "a8c47801-6231-4092-8c00-c05203a32a9a", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "537db965-defa-495d-a00a-4e21791d0c37");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a8c47801-6231-4092-8c00-c05203a32a9a");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52af7903-d3ff-45ec-bad8-6fe4dbc0d0fd", null, "Administrator", "ADMINISTRATOR" },
                    { "68e2ddb1-6fa9-4198-b808-90655e7e2442", null, "Visitor", "VISITOR" }
                });
        }
    }
}
