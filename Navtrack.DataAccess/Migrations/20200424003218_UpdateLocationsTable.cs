using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations
{
    public partial class UpdateLocationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Speed",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Heading",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Altitude",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<short>(
                name: "GsmSignal",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Odometer",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PositionStatus",
                table: "Locations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GsmSignal",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Odometer",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PositionStatus",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "Speed",
                table: "Locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Heading",
                table: "Locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "Altitude",
                table: "Locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
