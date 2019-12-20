using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class Rejting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RejtingRadnik",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RejtingKupac",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejtingRadnik",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RejtingKupac",
                table: "AspNetUsers");
        }
    }
}
