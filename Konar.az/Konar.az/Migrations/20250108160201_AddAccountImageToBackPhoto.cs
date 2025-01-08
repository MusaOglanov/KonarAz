using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konar.az.Migrations
{
    public partial class AddAccountImageToBackPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountImage",
                table: "BackPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountImage",
                table: "BackPhotos");
        }
    }
}
