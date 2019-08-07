using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations
{
    public partial class FixedLatLongColumnTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                type: "decimal(9, 6)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Locations",
                type: "decimal(9, 6)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9, 6)");
        }
    }
}
