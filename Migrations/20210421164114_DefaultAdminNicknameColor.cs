using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class DefaultAdminNicknameColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nickname_color_code",
                table: "admin",
                type: "varchar(15)",
                nullable: true,
                defaultValueSql: "'#000000'",
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldNullable: true,
                oldDefaultValueSql: "'#FFFFFF'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nickname_color_code",
                table: "admin",
                type: "varchar(15)",
                nullable: true,
                defaultValueSql: "'#FFFFFF'",
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldNullable: true,
                oldDefaultValueSql: "'#000000'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_general_ci");
        }
    }
}
