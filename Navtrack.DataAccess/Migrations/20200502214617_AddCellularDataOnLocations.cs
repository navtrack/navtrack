using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations
{
    public partial class AddCellularDataOnLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CellId",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationAreaCode",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MobileCountryCode",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MobileNetworkCode",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationAreaCode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MobileCountryCode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MobileNetworkCode",
                table: "Locations");
        }
    }
}
