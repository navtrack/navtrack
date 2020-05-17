using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.Sqlite.Migrations
{
    public partial class RenameImeiToDeviceIdOnDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMEI",
                table: "Devices");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Devices");

            migrationBuilder.AddColumn<string>(
                name: "IMEI",
                table: "Devices",
                type: "TEXT",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
