using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konar.az.Migrations
{
    public partial class CreateBackPhotosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaqImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackPhotos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackPhotos");
        }
    }
}
