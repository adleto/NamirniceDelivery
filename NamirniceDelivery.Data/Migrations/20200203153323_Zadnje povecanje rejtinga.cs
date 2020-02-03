using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class Zadnjepovecanjerejtinga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZadnjaSMSObavjest",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "ZadnjaSMSObavijest",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ZadnjePovecanjeRejtinga",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZadnjaSMSObavijest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ZadnjePovecanjeRejtinga",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "ZadnjaSMSObavjest",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
