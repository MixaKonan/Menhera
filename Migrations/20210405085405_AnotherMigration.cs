using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class AnotherMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "is_closed",
                table: "thread",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AddColumn<long>(
                name: "bump_in_unix_time",
                table: "thread",
                type: "bigint(20)",
                nullable: false,
                defaultValue: 0L);

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
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "is_pinned",
                table: "post",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<bool>(
                name: "is_hidden",
                table: "board",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<bool>(
                name: "has_subject",
                table: "board",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "files_are_allowed",
                table: "board",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "anon_has_no_name",
                table: "board",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<long>(
                name: "ban_time_in_unix_seconds",
                table: "ban",
                type: "bigint(20)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "has_access_to_panel",
                table: "admin",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<bool>(
                name: "can_delete_posts",
                table: "admin",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<bool>(
                name: "can_close_threads",
                table: "admin",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<bool>(
                name: "can_ban_users",
                table: "admin",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(ulong),
                oldType: "bit",
                oldDefaultValueSql: "b'0'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bump_in_unix_time",
                table: "thread");

            migrationBuilder.AlterColumn<ulong>(
                name: "is_closed",
                table: "thread",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");

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
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint(20)");

            migrationBuilder.AlterColumn<ulong>(
                name: "is_pinned",
                table: "post",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<ulong>(
                name: "is_hidden",
                table: "board",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<ulong>(
                name: "has_subject",
                table: "board",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<ulong>(
                name: "files_are_allowed",
                table: "board",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<ulong>(
                name: "anon_has_no_name",
                table: "board",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<long>(
                name: "ban_time_in_unix_seconds",
                table: "ban",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint(20)");

            migrationBuilder.AlterColumn<ulong>(
                name: "has_access_to_panel",
                table: "admin",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<ulong>(
                name: "can_delete_posts",
                table: "admin",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'1'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'1'");

            migrationBuilder.AlterColumn<ulong>(
                name: "can_close_threads",
                table: "admin",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");

            migrationBuilder.AlterColumn<ulong>(
                name: "can_ban_users",
                table: "admin",
                type: "bit",
                nullable: false,
                defaultValueSql: "b'0'",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "b'0'");
        }
    }
}
