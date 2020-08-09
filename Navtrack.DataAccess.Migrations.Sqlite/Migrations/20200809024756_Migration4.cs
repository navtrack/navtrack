using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.Sqlite.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Devices_DeviceId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "UserDevice");

            migrationBuilder.DropIndex(
                name: "IX_Assets_DeviceId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Assets");

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "Devices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Devices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolPort",
                table: "Devices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AssetId",
                table: "Devices",
                column: "AssetId");

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
                name: "FK_Devices_Assets_AssetId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_AssetId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ProtocolPort",
                table: "Devices");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Logs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Locations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Connections",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Connections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Assets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserDevice",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevice", x => new { x.UserId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_UserDevice_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDevice_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DeviceId",
                table: "Assets",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_DeviceId",
                table: "UserDevice",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Devices_DeviceId",
                table: "Assets",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }
    }
}
