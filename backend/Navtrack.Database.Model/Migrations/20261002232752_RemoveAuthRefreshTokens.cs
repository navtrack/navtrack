using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Navtrack.Database.Model.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuthRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_refresh_tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auth_refresh_tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AccessToken = table.Column<string>(type: "jsonb", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    ConsumedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    Lifetime = table.Column<int>(type: "integer", nullable: false),
                    SubjectId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_refresh_tokens", x => x.Id);
                });
        }
    }
}
