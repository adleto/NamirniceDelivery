using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class Nazivzapodruznicu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Naziv",
                table: "Podruznica",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Naziv",
                table: "Podruznica");
        }
    }
}
