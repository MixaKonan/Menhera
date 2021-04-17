using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class AddAdminIdFKOnPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "admin_id",
                table: "post",
                type: "int(11)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "fkAdminId_Post",
                table: "post",
                column: "admin_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminPost",
                table: "post",
                column: "admin_id",
                principalTable: "admin",
                principalColumn: "admin_id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminPost",
                table: "post");

            migrationBuilder.DropIndex(
                name: "fkAdminId_Post",
                table: "post");

            migrationBuilder.DropColumn(
                name: "admin_id",
                table: "post");
        }
    }
}
