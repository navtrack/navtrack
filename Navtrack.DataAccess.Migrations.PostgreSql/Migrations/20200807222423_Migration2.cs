using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Devices_DeviceId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_DeviceId",
                table: "Assets");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AssetId",
                table: "Devices",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DeviceId",
                table: "Assets",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Devices_DeviceId",
                table: "Assets",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Assets_AssetId",
                table: "Devices",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Devices_DeviceId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Assets_AssetId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_AssetId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Assets_DeviceId",
                table: "Assets");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DeviceId",
                table: "Assets",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Devices_DeviceId",
                table: "Assets",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }
    }
}
