﻿// <auto-generated />
using System;
using Menhera.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Menhera.Migrations
{
    [DbContext(typeof(MenherachanContext))]
    [Migration("20210327204142_Ban-Table-Update")]
    partial class BanTableUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Menhera.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("admin_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("AdminIpHash")
                        .IsRequired()
                        .HasColumnName("admin_ip_hash")
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<bool>("CanBanUsers")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("can_ban_users")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<bool>("CanCloseThreads")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("can_close_threads")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<bool>("CanDeletePosts")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("can_delete_posts")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'1'");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<bool>("HasAccessToPanel")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("has_access_to_panel")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("login")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("password_hash")
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("AdminId");

                    b.ToTable("admin");
                });

            modelBuilder.Entity("Menhera.Models.Ban", b =>
                {
                    b.Property<int>("BanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ban_id")
                        .HasColumnType("int(11)");

                    b.Property<int>("AdminId")
                        .HasColumnName("admin_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("AnonIpHash")
                        .IsRequired()
                        .HasColumnName("anon_ip_hash")
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<long>("BanTimeInUnixSeconds")
                        .HasColumnName("ban_time_in_unix_seconds")
                        .HasColumnType("bigint");

                    b.Property<int?>("BoardId")
                        .HasColumnType("int(11)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnName("reason")
                        .HasColumnType("text")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<long>("Term")
                        .HasColumnName("term")
                        .HasColumnType("bigint(20)");

                    b.HasKey("BanId");

                    b.HasIndex("AdminId")
                        .HasName("fkAdminId_Ban");

                    b.HasIndex("BoardId");

                    b.ToTable("ban");
                });

            modelBuilder.Entity("Menhera.Models.Board", b =>
                {
                    b.Property<int>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("board_id")
                        .HasColumnType("int(11)");

                    b.Property<bool>("AnonHasNoName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("anon_has_no_name")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<string>("AnonName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("anon_name")
                        .HasColumnType("varchar(20)")
                        .HasDefaultValueSql("''")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<short>("FileLimit")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("file_limit")
                        .HasColumnType("smallint(6)")
                        .HasDefaultValueSql("'4'");

                    b.Property<bool>("FilesAreAllowed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("files_are_allowed")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'1'");

                    b.Property<bool>("HasSubject")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("has_subject")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'1'");

                    b.Property<bool>("IsHidden")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("is_hidden")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<string>("Postfix")
                        .IsRequired()
                        .HasColumnName("postfix")
                        .HasColumnType("varchar(15)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnName("prefix")
                        .HasColumnType("varchar(15)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(30)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("BoardId");

                    b.HasIndex("Prefix")
                        .IsUnique()
                        .HasName("prefix");

                    b.ToTable("board");
                });

            modelBuilder.Entity("Menhera.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("file_id")
                        .HasColumnType("int(11)");

                    b.Property<int>("BoardId")
                        .HasColumnName("board_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnName("file_name")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("Info")
                        .HasColumnName("info")
                        .HasColumnType("text")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("PostId")
                        .HasColumnName("post_id")
                        .HasColumnType("int(11)");

                    b.Property<int>("ThreadId")
                        .HasColumnName("thread_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("ThumbnailName")
                        .IsRequired()
                        .HasColumnName("thumbnail_name")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("FileId");

                    b.HasIndex("BoardId")
                        .HasName("fkBoardId_File");

                    b.HasIndex("PostId")
                        .HasName("fkPostId_File");

                    b.HasIndex("ThreadId")
                        .HasName("fkThreadId_File");

                    b.ToTable("file");
                });

            modelBuilder.Entity("Menhera.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("post_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("AnonIpHash")
                        .IsRequired()
                        .HasColumnName("anon_ip_hash")
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("AnonName")
                        .HasColumnName("anon_name")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("BoardId")
                        .HasColumnName("board_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnName("comment")
                        .HasColumnType("text")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<bool>("IsPinned")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("is_pinned")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<string>("Subject")
                        .HasColumnName("subject")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("ThreadId")
                        .HasColumnName("thread_id")
                        .HasColumnType("int(11)");

                    b.Property<long>("TimeInUnixSeconds")
                        .HasColumnName("time_in_unix_seconds")
                        .HasColumnType("bigint");

                    b.HasKey("PostId");

                    b.HasIndex("BoardId")
                        .HasName("fkBoardId_Post");

                    b.HasIndex("ThreadId")
                        .HasName("fkThreadId_Post");

                    b.ToTable("post");
                });

            modelBuilder.Entity("Menhera.Models.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("report_id")
                        .HasColumnType("int(11)");

                    b.Property<int>("BoardId")
                        .HasColumnName("board_id")
                        .HasColumnType("int(11)");

                    b.Property<int>("PostId")
                        .HasColumnName("post_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("Reason")
                        .HasColumnName("reason")
                        .HasColumnType("text")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<long>("ReportTimeInUnixSeconds")
                        .HasColumnName("report_time_in_unix_seconds")
                        .HasColumnType("bigint");

                    b.Property<int>("ThreadId")
                        .HasColumnName("thread_id")
                        .HasColumnType("int(11)");

                    b.HasKey("ReportId");

                    b.HasIndex("BoardId")
                        .HasName("fkBoardId_Report");

                    b.HasIndex("ThreadId")
                        .HasName("fkThreadId_Report");

                    b.ToTable("report");
                });

            modelBuilder.Entity("Menhera.Models.Thread", b =>
                {
                    b.Property<int>("ThreadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("thread_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("AnonName")
                        .HasColumnName("anon_name")
                        .HasColumnType("varchar(45)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.Property<int>("BoardId")
                        .HasColumnName("board_id")
                        .HasColumnType("int(11)");

                    b.Property<bool>("IsClosed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("is_closed")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("b'0'");

                    b.Property<string>("OpIpHash")
                        .IsRequired()
                        .HasColumnName("op_ip_hash")
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_general_ci");

                    b.HasKey("ThreadId");

                    b.HasIndex("BoardId")
                        .HasName("fkBoardId_Thread");

                    b.ToTable("thread");
                });

            modelBuilder.Entity("Menhera.Models.Ban", b =>
                {
                    b.HasOne("Menhera.Models.Admin", "Admin")
                        .WithMany("Ban")
                        .HasForeignKey("AdminId")
                        .HasConstraintName("FK_AdminBan")
                        .IsRequired();

                    b.HasOne("Menhera.Models.Board", "Board")
                        .WithMany("Ban")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("FK_BoardBan");
                });

            modelBuilder.Entity("Menhera.Models.File", b =>
                {
                    b.HasOne("Menhera.Models.Board", "Board")
                        .WithMany("File")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("FK_BoardFile")
                        .IsRequired();

                    b.HasOne("Menhera.Models.Post", "Post")
                        .WithMany("File")
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostFile")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Menhera.Models.Thread", "Thread")
                        .WithMany("File")
                        .HasForeignKey("ThreadId")
                        .HasConstraintName("FK_ThreadFile")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Menhera.Models.Post", b =>
                {
                    b.HasOne("Menhera.Models.Board", "Board")
                        .WithMany("Post")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("FK_BoardPost")
                        .IsRequired();

                    b.HasOne("Menhera.Models.Thread", "Thread")
                        .WithMany("Post")
                        .HasForeignKey("ThreadId")
                        .HasConstraintName("FK_ThreadPost")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Menhera.Models.Report", b =>
                {
                    b.HasOne("Menhera.Models.Board", "Board")
                        .WithMany("Report")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("FK_BoardReport")
                        .IsRequired();

                    b.HasOne("Menhera.Models.Thread", "Thread")
                        .WithMany("Report")
                        .HasForeignKey("ThreadId")
                        .HasConstraintName("FK_ThreadReport")
                        .IsRequired();
                });

            modelBuilder.Entity("Menhera.Models.Thread", b =>
                {
                    b.HasOne("Menhera.Models.Board", "Board")
                        .WithMany("Thread")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("FK_BoardThread")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
