using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class Nullablepopust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NamirnicaPodruznica_Popust_PopustId",
                table: "NamirnicaPodruznica");

            migrationBuilder.AlterColumn<int>(
                name: "PopustId",
                table: "NamirnicaPodruznica",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_NamirnicaPodruznica_Popust_PopustId",
                table: "NamirnicaPodruznica",
                column: "PopustId",
                principalTable: "Popust",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NamirnicaPodruznica_Popust_PopustId",
                table: "NamirnicaPodruznica");

            migrationBuilder.AlterColumn<int>(
                name: "PopustId",
                table: "NamirnicaPodruznica",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NamirnicaPodruznica_Popust_PopustId",
                table: "NamirnicaPodruznica",
                column: "PopustId",
                principalTable: "Popust",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
