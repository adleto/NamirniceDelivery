using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class uklonipovecanjerejtinganepotrebno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZadnjePovecanjeRejtinga",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ZadnjePovecanjeRejtinga",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
