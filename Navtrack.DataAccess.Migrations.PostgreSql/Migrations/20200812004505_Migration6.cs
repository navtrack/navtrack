using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Assets_AssetId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Assets_AssetId",
                table: "Devices",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Assets_AssetId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Assets_AssetId",
                table: "Devices",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");
        }
    }
}
