using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatch.Migrations
{
    public partial class added_isActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppMovies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppMovies");
        }
    }
}
