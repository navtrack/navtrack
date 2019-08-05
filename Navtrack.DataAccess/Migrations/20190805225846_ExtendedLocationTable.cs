using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations
{
    public partial class ExtendedLocationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HDOP",
                table: "Locations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ProtocolData",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Satellites",
                table: "Locations",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HDOP",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ProtocolData",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Satellites",
                table: "Locations");
        }
    }
}
