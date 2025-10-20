using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Navtrack.Database.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddDevicesMessagesIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_devices_messages_AssetId_Date",
                table: "devices_messages",
                columns: new[] { "AssetId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_devices_messages_AssetId_Date",
                table: "devices_messages");
        }
    }
}
