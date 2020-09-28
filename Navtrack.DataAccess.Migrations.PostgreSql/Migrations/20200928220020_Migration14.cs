using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasurementSystem",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementSystem",
                table: "Users");
        }
    }
}
