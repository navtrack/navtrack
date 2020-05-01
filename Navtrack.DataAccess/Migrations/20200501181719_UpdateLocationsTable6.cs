using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations
{
    public partial class UpdateLocationsTable6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProtocolData",
                table: "Locations");

            migrationBuilder.AlterColumn<decimal>(
                name: "Speed",
                table: "Locations",
                type: "decimal(6, 2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<bool>(
                name: "PositionStatus",
                table: "Locations",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "Heading",
                table: "Locations",
                type: "decimal(5, 2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "HDOP",
                table: "Locations",
                type: "decimal(4, 2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Altitude",
                table: "Locations",
                type: "decimal(7, 2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Speed",
                table: "Locations",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PositionStatus",
                table: "Locations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Heading",
                table: "Locations",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "HDOP",
                table: "Locations",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Altitude",
                table: "Locations",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7, 2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtocolData",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
