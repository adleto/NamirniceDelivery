using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class ForeignkeyNamirnicauKupljeneNamirnice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NamirnicaId",
                table: "KupljeneNamirnice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KupljeneNamirnice_NamirnicaId",
                table: "KupljeneNamirnice",
                column: "NamirnicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_KupljeneNamirnice_Namirnica_NamirnicaId",
                table: "KupljeneNamirnice",
                column: "NamirnicaId",
                principalTable: "Namirnica",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KupljeneNamirnice_Namirnica_NamirnicaId",
                table: "KupljeneNamirnice");

            migrationBuilder.DropIndex(
                name: "IX_KupljeneNamirnice_NamirnicaId",
                table: "KupljeneNamirnice");

            migrationBuilder.DropColumn(
                name: "NamirnicaId",
                table: "KupljeneNamirnice");
        }
    }
}
