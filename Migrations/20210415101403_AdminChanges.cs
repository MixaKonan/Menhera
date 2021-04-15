using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class AdminChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admin_ip_hash",
                table: "admin");

            migrationBuilder.AddColumn<string>(
                name: "nickname_color_code",
                table: "admin",
                type: "varchar(15)",
                nullable: true,
                defaultValueSql: "'##FFFFFF'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nickname_color_code",
                table: "admin");

            migrationBuilder.AddColumn<string>(
                name: "admin_ip_hash",
                table: "admin",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci");
        }
    }
}
