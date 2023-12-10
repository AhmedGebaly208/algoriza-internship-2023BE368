using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeataApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AlterOnVezeataUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "DateOfBirh",
                table: "AspNetUsers",
                newName: "DateOfBirth");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "AspNetUsers",
                newName: "DateOfBirh");

            migrationBuilder.AddColumn<string>(
                name: "AccountTypeId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
