using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Navtrack.Database.Model.Migrations
{
    /// <inheritdoc />
    public partial class RenameOpenIddictTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                table: "OpenIddictTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                table: "OpenIddictTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenIddictTokens",
                table: "OpenIddictTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenIddictScopes",
                table: "OpenIddictScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenIddictAuthorizations",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenIddictApplications",
                table: "OpenIddictApplications");

            migrationBuilder.RenameTable(
                name: "OpenIddictTokens",
                newName: "auth_tokens");

            migrationBuilder.RenameTable(
                name: "OpenIddictScopes",
                newName: "auth_scopes");

            migrationBuilder.RenameTable(
                name: "OpenIddictAuthorizations",
                newName: "auth_authorizations");

            migrationBuilder.RenameTable(
                name: "OpenIddictApplications",
                newName: "auth_applications");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "auth_tokens",
                newName: "IX_auth_tokens_ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "auth_tokens",
                newName: "IX_auth_tokens_AuthorizationId");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
                table: "auth_tokens",
                newName: "IX_auth_tokens_ApplicationId_Status_Subject_Type");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "auth_scopes",
                newName: "IX_auth_scopes_Name");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
                table: "auth_authorizations",
                newName: "IX_auth_authorizations_ApplicationId_Status_Subject_Type");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "auth_applications",
                newName: "IX_auth_applications_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_auth_tokens",
                table: "auth_tokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_auth_scopes",
                table: "auth_scopes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_auth_authorizations",
                table: "auth_authorizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_auth_applications",
                table: "auth_applications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_auth_authorizations_auth_applications_ApplicationId",
                table: "auth_authorizations",
                column: "ApplicationId",
                principalTable: "auth_applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_auth_tokens_auth_applications_ApplicationId",
                table: "auth_tokens",
                column: "ApplicationId",
                principalTable: "auth_applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_auth_tokens_auth_authorizations_AuthorizationId",
                table: "auth_tokens",
                column: "AuthorizationId",
                principalTable: "auth_authorizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auth_authorizations_auth_applications_ApplicationId",
                table: "auth_authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_auth_tokens_auth_applications_ApplicationId",
                table: "auth_tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_auth_tokens_auth_authorizations_AuthorizationId",
                table: "auth_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auth_tokens",
                table: "auth_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auth_scopes",
                table: "auth_scopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auth_authorizations",
                table: "auth_authorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auth_applications",
                table: "auth_applications");

            migrationBuilder.RenameTable(
                name: "auth_tokens",
                newName: "OpenIddictTokens");

            migrationBuilder.RenameTable(
                name: "auth_scopes",
                newName: "OpenIddictScopes");

            migrationBuilder.RenameTable(
                name: "auth_authorizations",
                newName: "OpenIddictAuthorizations");

            migrationBuilder.RenameTable(
                name: "auth_applications",
                newName: "OpenIddictApplications");

            migrationBuilder.RenameIndex(
                name: "IX_auth_tokens_ReferenceId",
                table: "OpenIddictTokens",
                newName: "IX_OpenIddictTokens_ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_auth_tokens_AuthorizationId",
                table: "OpenIddictTokens",
                newName: "IX_OpenIddictTokens_AuthorizationId");

            migrationBuilder.RenameIndex(
                name: "IX_auth_tokens_ApplicationId_Status_Subject_Type",
                table: "OpenIddictTokens",
                newName: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type");

            migrationBuilder.RenameIndex(
                name: "IX_auth_scopes_Name",
                table: "OpenIddictScopes",
                newName: "IX_OpenIddictScopes_Name");

            migrationBuilder.RenameIndex(
                name: "IX_auth_authorizations_ApplicationId_Status_Subject_Type",
                table: "OpenIddictAuthorizations",
                newName: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type");

            migrationBuilder.RenameIndex(
                name: "IX_auth_applications_ClientId",
                table: "OpenIddictApplications",
                newName: "IX_OpenIddictApplications_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenIddictTokens",
                table: "OpenIddictTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenIddictScopes",
                table: "OpenIddictScopes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenIddictAuthorizations",
                table: "OpenIddictAuthorizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenIddictApplications",
                table: "OpenIddictApplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~",
                table: "OpenIddictAuthorizations",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                table: "OpenIddictTokens",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId",
                principalTable: "OpenIddictAuthorizations",
                principalColumn: "Id");
        }
    }
}
