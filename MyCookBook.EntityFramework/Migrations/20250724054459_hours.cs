using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCookBook.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class hours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Recipes");
        }
    }
}
