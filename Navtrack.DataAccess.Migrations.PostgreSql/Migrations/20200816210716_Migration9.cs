using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Navtrack.DataAccess.Migrations.PostgreSql.Migrations
{
    public partial class Migration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceConnectionMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hex = table.Column<string>(nullable: true),
                    String = table.Column<string>(nullable: true),
                    DeviceConnectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConnectionMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceConnectionMessages_DeviceConnections_DeviceConnection~",
                        column: x => x.DeviceConnectionId,
                        principalTable: "DeviceConnections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnectionMessages_DeviceConnectionId",
                table: "DeviceConnectionMessages",
                column: "DeviceConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceConnectionMessages");
        }
    }
}
