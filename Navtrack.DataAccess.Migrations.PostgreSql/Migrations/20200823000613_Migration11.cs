using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "DeviceConnections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnections_DeviceId",
                table: "DeviceConnections",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceConnections_Devices_DeviceId",
                table: "DeviceConnections",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceConnections_Devices_DeviceId",
                table: "DeviceConnections");

            migrationBuilder.DropIndex(
                name: "IX_DeviceConnections_DeviceId",
                table: "DeviceConnections");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "DeviceConnections");
        }
    }
}
