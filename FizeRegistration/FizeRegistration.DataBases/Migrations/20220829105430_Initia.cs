using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FizeRegistration.DataBases.Migrations
{
    public partial class Initia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencion_UserIdentities_UserIdentityId",
                table: "Agencion");

            migrationBuilder.DropIndex(
                name: "IX_Agencion_UserIdentityId",
                table: "Agencion");

            migrationBuilder.AlterColumn<long>(
                name: "UserIdentityId",
                table: "Agencion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Agencion_UserIdentityId",
                table: "Agencion",
                column: "UserIdentityId",
                unique: true,
                filter: "[UserIdentityId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Agencion_UserIdentities_UserIdentityId",
                table: "Agencion",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencion_UserIdentities_UserIdentityId",
                table: "Agencion");

            migrationBuilder.DropIndex(
                name: "IX_Agencion_UserIdentityId",
                table: "Agencion");

            migrationBuilder.AlterColumn<long>(
                name: "UserIdentityId",
                table: "Agencion",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agencion_UserIdentityId",
                table: "Agencion",
                column: "UserIdentityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agencion_UserIdentities_UserIdentityId",
                table: "Agencion",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
