using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceConnectionMessageId",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_DeviceConnectionMessageId",
                table: "Locations",
                column: "DeviceConnectionMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_DeviceConnectionMessages_DeviceConnectionMessageId",
                table: "Locations",
                column: "DeviceConnectionMessageId",
                principalTable: "DeviceConnectionMessages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_DeviceConnectionMessages_DeviceConnectionMessageId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_DeviceConnectionMessageId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DeviceConnectionMessageId",
                table: "Locations");
        }
    }
}
