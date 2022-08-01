using Microsoft.EntityFrameworkCore.Migrations;

namespace Harvested.AI.Databases.Migrations
{
    public partial class AddedNullableCompanyInfoRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountType_AccountTypeCompanyInfo_CompanyInfoId",
                table: "AccountType");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyInfoId",
                table: "AccountType",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_AccountType_AccountTypeCompanyInfo_CompanyInfoId",
                table: "AccountType",
                column: "CompanyInfoId",
                principalTable: "AccountTypeCompanyInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountType_AccountTypeCompanyInfo_CompanyInfoId",
                table: "AccountType");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyInfoId",
                table: "AccountType",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountType_AccountTypeCompanyInfo_CompanyInfoId",
                table: "AccountType",
                column: "CompanyInfoId",
                principalTable: "AccountTypeCompanyInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
