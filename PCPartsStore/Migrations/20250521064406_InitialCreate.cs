using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCPartsStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductsCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Other" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductsCategory",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
