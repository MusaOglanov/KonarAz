using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konar.az.Migrations
{
    public partial class SeedDataBiosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bios",
                columns: new[] { "Id", "Adress", "Email", "FacebookLink", "FooterLogo", "HeaderLogo", "InstagramLink", "LinkedinLink", "Number", "WhatsappLink" },
                values: new object[] { 1, "Azərbaycan, Baku", "sales@konar.az", "https://www.facebook.com/", "konarlogo.svg", "konarlogo.svg", "https://www.instagram.com/", "https://www.linkedin.com/", "+994517005010", "https://www.whatsapp.com/" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
