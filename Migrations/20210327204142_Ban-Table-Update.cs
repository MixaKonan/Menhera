using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class BanTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "fkBoardId_Ban",
                "Ban");
            
            migrationBuilder.DropColumn(
                "board_id",
                "ban");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "ban",
                newName: "board_id");

            migrationBuilder.RenameIndex(
                name: "IX_ban_BoardId",
                table: "ban",
                newName: "fkBoardId_Ban");

            migrationBuilder.AlterColumn<int>(
                name: "board_id",
                table: "ban",
                type: "int(11)",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
