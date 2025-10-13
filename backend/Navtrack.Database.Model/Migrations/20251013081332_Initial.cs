using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Navtrack.Database.Model.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auth_refresh_tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lifetime = table.Column<int>(type: "integer", nullable: false),
                    ConsumedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccessToken = table.Column<string>(type: "jsonb", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubjectId = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_refresh_tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "devices_connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Port = table.Column<short>(type: "smallint", nullable: false),
                    IPAddress = table.Column<string>(type: "text", nullable: true),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices_connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetsCount = table.Column<int>(type: "integer", nullable: false),
                    UsersCount = table.Column<int>(type: "integer", nullable: false),
                    TeamsCount = table.Column<int>(type: "integer", nullable: false),
                    DevicesCount = table.Column<int>(type: "integer", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "system_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "system_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "jsonb", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(88)", maxLength: 88, nullable: true),
                    PasswordSalt = table.Column<string>(type: "character varying(44)", maxLength: 44, nullable: true),
                    UnitsType = table.Column<int>(type: "integer", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users_password_resets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    Invalid = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_password_resets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "devices_connections_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices_connections_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_devices_connections_data_devices_connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "devices_connections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetsCount = table.Column<int>(type: "integer", nullable: false),
                    UsersCount = table.Column<int>(type: "integer", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teams_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizations_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations_users", x => x.Id);
                    table.UniqueConstraint("AK_organizations_users_OrganizationId_UserId", x => new { x.OrganizationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_organizations_users_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_organizations_users_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams_users", x => x.Id);
                    table.UniqueConstraint("AK_teams_users_TeamId_UserId", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_teams_users_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teams_users_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true),
                    LastMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastPositionMessageId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_assets_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assets_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets_users", x => x.Id);
                    table.UniqueConstraint("AK_assets_users_AssetId_UserId", x => new { x.AssetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_assets_users_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assets_users_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    DeviceTypeId = table.Column<string>(type: "text", nullable: false),
                    ProtocolPort = table.Column<int>(type: "integer", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    OldId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_devices_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_devices_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams_assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams_assets", x => x.Id);
                    table.UniqueConstraint("AK_teams_assets_TeamId_AssetId", x => new { x.TeamId, x.AssetId });
                    table.ForeignKey(
                        name: "FK_teams_assets_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teams_assets_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "devices_messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessagePriority = table.Column<byte>(type: "smallint", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Coordinates = table.Column<NpgsqlPoint>(type: "point", nullable: false),
                    OldMessage = table.Column<string>(type: "text", nullable: true),
                    Speed = table.Column<short>(type: "smallint", nullable: true),
                    Heading = table.Column<short>(type: "smallint", nullable: true),
                    Altitude = table.Column<short>(type: "smallint", nullable: true),
                    Satellites = table.Column<short>(type: "smallint", nullable: true),
                    PDOP = table.Column<float>(type: "real", nullable: true),
                    HDOP = table.Column<float>(type: "real", nullable: true),
                    Valid = table.Column<bool>(type: "boolean", nullable: true),
                    DeviceOdometer = table.Column<int>(type: "integer", nullable: true),
                    DeviceBatteryLevel = table.Column<byte>(type: "smallint", nullable: true),
                    DeviceBatteryVoltage = table.Column<float>(type: "real", nullable: true),
                    DeviceBatteryCurrent = table.Column<float>(type: "real", nullable: true),
                    VehicleOdometer = table.Column<int>(type: "integer", nullable: true),
                    VehicleIgnition = table.Column<bool>(type: "boolean", nullable: true),
                    VehicleIgnitionDuration = table.Column<int>(type: "integer", nullable: true),
                    VehicleFuelConsumption = table.Column<float>(type: "real", nullable: true),
                    VehicleVoltage = table.Column<float>(type: "real", nullable: true),
                    GSMSignalStrength = table.Column<short>(type: "smallint", nullable: true),
                    GSMSignalLevel = table.Column<byte>(type: "smallint", nullable: true),
                    GSMMobileCountryCode = table.Column<string>(type: "text", nullable: true),
                    GSMMobileNetworkCode = table.Column<string>(type: "text", nullable: true),
                    GSMLocationAreaCode = table.Column<string>(type: "text", nullable: true),
                    GSMCellId = table.Column<int>(type: "integer", nullable: true),
                    GSMLteCellId = table.Column<int>(type: "integer", nullable: true),
                    OldId = table.Column<string>(type: "text", nullable: true),
                    AdditionalData = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_devices_messages_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_devices_messages_devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_devices_messages_devices_connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "devices_connections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_assets_DeviceId",
                table: "assets",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assets_LastMessageId",
                table: "assets",
                column: "LastMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assets_LastPositionMessageId",
                table: "assets",
                column: "LastPositionMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assets_OrganizationId",
                table: "assets",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_assets_users_UserId",
                table: "assets_users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_AssetId",
                table: "devices",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_OrganizationId",
                table: "devices",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_connections_data_ConnectionId",
                table: "devices_connections_data",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_messages_AssetId",
                table: "devices_messages",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_messages_ConnectionId",
                table: "devices_messages",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_devices_messages_DeviceId",
                table: "devices_messages",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_users_UserId",
                table: "organizations_users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_teams_OrganizationId",
                table: "teams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_teams_assets_AssetId",
                table: "teams_assets",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_teams_users_UserId",
                table: "teams_users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_devices_DeviceId",
                table: "assets",
                column: "DeviceId",
                principalTable: "devices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_devices_messages_LastMessageId",
                table: "assets",
                column: "LastMessageId",
                principalTable: "devices_messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_devices_messages_LastPositionMessageId",
                table: "assets",
                column: "LastPositionMessageId",
                principalTable: "devices_messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_devices_DeviceId",
                table: "assets");

            migrationBuilder.DropForeignKey(
                name: "FK_devices_messages_devices_DeviceId",
                table: "devices_messages");

            migrationBuilder.DropForeignKey(
                name: "FK_assets_devices_messages_LastMessageId",
                table: "assets");

            migrationBuilder.DropForeignKey(
                name: "FK_assets_devices_messages_LastPositionMessageId",
                table: "assets");

            migrationBuilder.DropTable(
                name: "assets_users");

            migrationBuilder.DropTable(
                name: "auth_refresh_tokens");

            migrationBuilder.DropTable(
                name: "devices_connections_data");

            migrationBuilder.DropTable(
                name: "organizations_users");

            migrationBuilder.DropTable(
                name: "system_events");

            migrationBuilder.DropTable(
                name: "system_settings");

            migrationBuilder.DropTable(
                name: "teams_assets");

            migrationBuilder.DropTable(
                name: "teams_users");

            migrationBuilder.DropTable(
                name: "users_password_resets");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "devices_messages");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "devices_connections");

            migrationBuilder.DropTable(
                name: "organizations");
        }
    }
}
