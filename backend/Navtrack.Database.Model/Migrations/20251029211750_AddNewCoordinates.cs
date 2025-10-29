using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Navtrack.Database.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "OldMessage",
                table: "devices_messages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "NewCoordinates",
                table: "devices_messages",
                type: "geography (point)",
                nullable: true);
            
            migrationBuilder.Sql("UPDATE devices_messages SET \"NewCoordinates\" = ST_SetSRID(ST_MakePoint(\"Coordinates\"[0], \"Coordinates\"[1]), 4326);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewCoordinates",
                table: "devices_messages");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "OldMessage",
                table: "devices_messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
