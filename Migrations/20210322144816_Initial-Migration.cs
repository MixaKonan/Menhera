using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    is_superadmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "ban",
                columns: table => new
                {
                    ban_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(nullable: false),
                    admin_id = table.Column<int>(nullable: false),
                    anon_ip_hash = table.Column<string>(nullable: true),
                    time = table.Column<DateTime>(nullable: false),
                    reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ban", x => x.ban_id);
                });

            migrationBuilder.CreateTable(
                name: "board",
                columns: table => new
                {
                    board_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    prefix = table.Column<string>(nullable: true),
                    postfix = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    is_hidden = table.Column<bool>(nullable: false),
                    anon_has_no_name = table.Column<bool>(nullable: false),
                    has_subject = table.Column<bool>(nullable: false),
                    files_are_allowed = table.Column<bool>(nullable: false),
                    file_limit = table.Column<int>(nullable: false),
                    anon_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board", x => x.board_id);
                });

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    file_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(nullable: false),
                    thread_id = table.Column<int>(nullable: false),
                    post_id = table.Column<int>(nullable: false),
                    file_name = table.Column<string>(nullable: true),
                    thumbnail_name = table.Column<string>(nullable: true),
                    info = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.file_id);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    post_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(nullable: false),
                    thread_id = table.Column<int>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    anon_name = table.Column<string>(nullable: true),
                    bump_in_unix_time = table.Column<int>(nullable: false),
                    is_pinned = table.Column<bool>(nullable: false),
                    time = table.Column<DateTime>(nullable: false),
                    anon_ip_hash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.post_id);
                });

            migrationBuilder.CreateTable(
                name: "report",
                columns: table => new
                {
                    report_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(nullable: false),
                    thread_id = table.Column<int>(nullable: false),
                    post_id = table.Column<int>(nullable: false),
                    reason = table.Column<string>(nullable: true),
                    time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "thread",
                columns: table => new
                {
                    thread_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(nullable: false),
                    is_closed = table.Column<string>(nullable: true),
                    op = table.Column<string>(nullable: true),
                    is_hidden = table.Column<bool>(nullable: false),
                    anon_has_no_name = table.Column<bool>(nullable: false),
                    anon_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thread", x => x.thread_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    login = table.Column<string>(nullable: true),
                    password_hash = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    is_admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "ban");

            migrationBuilder.DropTable(
                name: "board");

            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "report");

            migrationBuilder.DropTable(
                name: "thread");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
