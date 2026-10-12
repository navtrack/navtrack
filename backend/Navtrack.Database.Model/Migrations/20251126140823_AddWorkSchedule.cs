using Microsoft.EntityFrameworkCore.Migrations;
using Navtrack.Database.Model.Shared;

#nullable disable

namespace Navtrack.Database.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<WorkScheduleEntity>(
                name: "WorkSchedule",
                table: "organizations",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkSchedule",
                table: "organizations");
        }
    }
}
