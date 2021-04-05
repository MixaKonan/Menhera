using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class BumpTimeDefaultDropped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "report_time_in_unix_seconds",
                table: "report",
                type: "bigint(20)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "time_in_unix_seconds",
                table: "post",
                type: "bigint(20)",
                nullable: false,
                defaultValueSql: "'unix_timestamp(current_timestamp())'",
                oldClrType: typeof(long),
                oldType: "bigint(20)");

            migrationBuilder.AlterColumn<long>(
                name: "ban_time_in_unix_seconds",
                table: "ban",
                type: "bigint(20)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "report_time_in_unix_seconds",
                table: "report",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint(20)");

            migrationBuilder.AlterColumn<long>(
                name: "time_in_unix_seconds",
                table: "post",
                type: "bigint(20)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint(20)",
                oldDefaultValueSql: "'unix_timestamp(current_timestamp())'");

            migrationBuilder.AlterColumn<long>(
                name: "ban_time_in_unix_seconds",
                table: "ban",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint(20)");
        }
    }
}
