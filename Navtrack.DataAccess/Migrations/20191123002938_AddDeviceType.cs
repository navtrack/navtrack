using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations
{
    public partial class AddDeviceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Devices_DeviceId",
                table: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "DeviceTypeId",
                table: "Devices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(maxLength: 200, nullable: false),
                    Model = table.Column<string>(maxLength: 200, nullable: false),
                    ProtocolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceTypeId",
                table: "Devices",
                column: "DeviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceTypes_DeviceTypeId",
                table: "Devices",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Devices_DeviceId",
                table: "Locations",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceTypes_DeviceTypeId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Devices_DeviceId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceTypeId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "Devices");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Assets_AssetId",
                table: "Locations",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Devices_DeviceId",
                table: "Locations",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
