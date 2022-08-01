using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvested.AI.Databases.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserIdentities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: false),
                    PasswordSalt = table.Column<string>(nullable: false),
                    LastLoggedIn = table.Column<DateTime>(nullable: true),
                    IsPasswordExpired = table.Column<bool>(nullable: false),
                    ForceChangePassword = table.Column<bool>(nullable: false),
                    CanUserResetExpiredPassword = table.Column<bool>(nullable: false),
                    PasswordExpiresAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentities_Email",
                table: "UserIdentities",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIdentities");
        }
    }
}
