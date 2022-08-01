using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FizeRegistration.DataBases.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserIdentities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoggedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPasswordExpired = table.Column<bool>(type: "bit", nullable: false),
                    ForceChangePassword = table.Column<bool>(type: "bit", nullable: false),
                    CanUserResetExpiredPassword = table.Column<bool>(type: "bit", nullable: false),
                    PasswordExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentities_Email",
                table: "UserIdentities",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIdentities");
        }
    }
}
