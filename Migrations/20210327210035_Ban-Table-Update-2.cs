using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class BanTableUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardBan",
                table: "ban");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ban",
                table: "ban",
                column: "ban_id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ban_board_BoardId",
                table: "ban");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ban",
                table: "ban");

            migrationBuilder.DropIndex(
                name: "IX_ban_BoardId",
                table: "ban");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardBan",
                table: "ban",
                column: "BoardId",
                principalTable: "board",
                principalColumn: "board_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
