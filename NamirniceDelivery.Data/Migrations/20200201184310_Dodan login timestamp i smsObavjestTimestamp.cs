using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class DodanlogintimestampismsObavjestTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginTimestamp",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ZadnjaSMSObavijest",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginTimestamp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ZadnjaSMSObavijest",
                table: "AspNetUsers");
        }
    }
}
