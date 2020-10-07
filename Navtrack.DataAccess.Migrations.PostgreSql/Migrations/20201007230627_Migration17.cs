using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "EndLocationId",
                table: "Trips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartLocationId",
                table: "Trips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_EndLocationId",
                table: "Trips",
                column: "EndLocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_StartLocationId",
                table: "Trips",
                column: "StartLocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Locations_EndLocationId",
                table: "Trips",
                column: "EndLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Locations_StartLocationId",
                table: "Trips",
                column: "StartLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Locations_EndLocationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Locations_StartLocationId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_EndLocationId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_StartLocationId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EndLocationId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartLocationId",
                table: "Trips");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "Trips",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "Trips",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
