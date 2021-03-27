using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class BanTableUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ban_board_BoardId",
                table: "ban");

            migrationBuilder.DropIndex(
                name: "IX_ban_BoardId",
                table: "ban");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "ban");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "ban",
                type: "int(11)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ban_BoardId",
                table: "ban",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ban_board_BoardId",
                table: "ban",
                column: "BoardId",
                principalTable: "board",
                principalColumn: "board_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
