using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatch.Migrations
{
    public partial class remove_isActive_movie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppMovies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppMovies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
