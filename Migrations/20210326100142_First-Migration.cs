using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Menhera.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    login = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    email = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    password_hash = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    can_delete_posts = table.Column<bool>(nullable: false, defaultValueSql: "b'1'"),
                    can_close_threads = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    has_access_to_panel = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    can_ban_users = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    admin_ip_hash = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "board",
                columns: table => new
                {
                    board_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    prefix = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    postfix = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    title = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    description = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    is_hidden = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    anon_has_no_name = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    has_subject = table.Column<bool>(nullable: false, defaultValueSql: "b'1'"),
                    files_are_allowed = table.Column<bool>(nullable: false, defaultValueSql: "b'1'"),
                    file_limit = table.Column<short>(type: "smallint(6)", nullable: false, defaultValueSql: "'4'"),
                    anon_name = table.Column<string>(type: "varchar(20)", nullable: true, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board", x => x.board_id);
                });

            migrationBuilder.CreateTable(
                name: "ban",
                columns: table => new
                {
                    ban_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(type: "int(11)", nullable: false),
                    admin_id = table.Column<int>(type: "int(11)", nullable: false),
                    anon_ip_hash = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    ban_time_in_unix_seconds = table.Column<long>(type: "bigint", nullable: false),
                    term = table.Column<long>(type: "bigint(20)", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ban", x => x.ban_id);
                    table.ForeignKey(
                        name: "FK_AdminBan",
                        column: x => x.admin_id,
                        principalTable: "admin",
                        principalColumn: "admin_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardBan",
                        column: x => x.board_id,
                        principalTable: "board",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "thread",
                columns: table => new
                {
                    thread_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(type: "int(11)", nullable: false),
                    is_closed = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    op_ip_hash = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    anon_name = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thread", x => x.thread_id);
                    table.ForeignKey(
                        name: "FK_BoardThread",
                        column: x => x.board_id,
                        principalTable: "board",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(type: "int(11)", nullable: false),
                    thread_id = table.Column<int>(type: "int(11)", nullable: false),
                    email = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    subject = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    comment = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    anon_name = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    is_pinned = table.Column<bool>(nullable: false, defaultValueSql: "b'0'"),
                    time_in_unix_seconds = table.Column<long>(type: "bigint", nullable: false),
                    anon_ip_hash = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_BoardPost",
                        column: x => x.board_id,
                        principalTable: "board",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreadPost",
                        column: x => x.thread_id,
                        principalTable: "thread",
                        principalColumn: "thread_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "report",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(type: "int(11)", nullable: false),
                    thread_id = table.Column<int>(type: "int(11)", nullable: false),
                    post_id = table.Column<int>(type: "int(11)", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    report_time_in_unix_seconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_BoardReport",
                        column: x => x.board_id,
                        principalTable: "board",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreadReport",
                        column: x => x.thread_id,
                        principalTable: "thread",
                        principalColumn: "thread_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    file_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    board_id = table.Column<int>(type: "int(11)", nullable: false),
                    thread_id = table.Column<int>(type: "int(11)", nullable: false),
                    post_id = table.Column<int>(type: "int(11)", nullable: false),
                    file_name = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    thumbnail_name = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    info = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.file_id);
                    table.ForeignKey(
                        name: "FK_BoardFile",
                        column: x => x.board_id,
                        principalTable: "board",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostFile",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThreadFile",
                        column: x => x.thread_id,
                        principalTable: "thread",
                        principalColumn: "thread_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "fkAdminId_Ban",
                table: "ban",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "fkBoardId_Ban",
                table: "ban",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "prefix",
                table: "board",
                column: "prefix",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fkBoardId_File",
                table: "file",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "fkPostId_File",
                table: "file",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "fkThreadId_File",
                table: "file",
                column: "thread_id");

            migrationBuilder.CreateIndex(
                name: "fkBoardId_Post",
                table: "post",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "fkThreadId_Post",
                table: "post",
                column: "thread_id");

            migrationBuilder.CreateIndex(
                name: "fkBoardId_Report",
                table: "report",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "fkThreadId_Report",
                table: "report",
                column: "thread_id");

            migrationBuilder.CreateIndex(
                name: "fkBoardId_Thread",
                table: "thread",
                column: "board_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ban");

            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "report");

            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "thread");

            migrationBuilder.DropTable(
                name: "board");
        }
    }
}
