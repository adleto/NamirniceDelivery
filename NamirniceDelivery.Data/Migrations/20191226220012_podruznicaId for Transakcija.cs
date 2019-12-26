using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class podruznicaIdforTransakcija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PodruznicaId",
                table: "Transakcija",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_PodruznicaId",
                table: "Transakcija",
                column: "PodruznicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_Podruznica_PodruznicaId",
                table: "Transakcija",
                column: "PodruznicaId",
                principalTable: "Podruznica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transakcija_Podruznica_PodruznicaId",
                table: "Transakcija");

            migrationBuilder.DropIndex(
                name: "IX_Transakcija_PodruznicaId",
                table: "Transakcija");

            migrationBuilder.DropColumn(
                name: "PodruznicaId",
                table: "Transakcija");
        }
    }
}
