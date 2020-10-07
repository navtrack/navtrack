using Microsoft.EntityFrameworkCore.Migrations;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Distance",
                table: "Trips",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Distance",
                table: "Trips",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
