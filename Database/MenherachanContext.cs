using Microsoft.EntityFrameworkCore;
using Menhera.Models;

namespace Menhera.Database
{
    public partial class MenherachanContext : DbContext
    {
        public MenherachanContext()
        {
        }

        public MenherachanContext(DbContextOptions<MenherachanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Ban> Ban { get; set; }
        public virtual DbSet<Board> Board { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Thread> Thread { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.AdminId)
                    .HasColumnName("admin_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CanBanUsers)
                    .IsRequired()
                    .HasColumnName("can_ban_users")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.CanCloseThreads)
                    .IsRequired()
                    .HasColumnName("can_close_threads")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.CanDeletePosts)
                    .IsRequired()
                    .HasColumnName("can_delete_posts")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.HasAccessToPanel)
                    .IsRequired()
                    .HasColumnName("has_access_to_panel")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.NicknameColorCode)
                    .HasColumnName("nickname_color_code")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci")
                    .HasDefaultValueSql("'#FFFFFF'");
            });

            modelBuilder.Entity<Ban>(entity =>
            {
                entity.ToTable("ban");

                entity.HasIndex(e => e.AdminId)
                    .HasName("fkAdminId_Ban");

                entity.Property(e => e.BanId)
                    .HasColumnName("ban_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AdminId)
                    .HasColumnName("admin_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnonIpHash)
                    .IsRequired()
                    .HasColumnName("anon_ip_hash")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.BanTimeInUnixSeconds)
                    .HasColumnName("ban_time_in_unix_seconds")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnName("reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Term)
                    .HasColumnName("term")
                    .HasColumnType("bigint(20)");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Ban)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminBan");
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.ToTable("board");

                entity.HasIndex(e => e.Prefix)
                    .HasName("prefix")
                    .IsUnique();

                entity.Property(e => e.BoardId)
                    .HasColumnName("board_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnonHasNoName)
                    .IsRequired()
                    .HasColumnName("anon_has_no_name")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.AnonName)
                    .HasColumnName("anon_name")
                    .HasColumnType("varchar(20)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FileLimit)
                    .HasColumnName("file_limit")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("'4'");

                entity.Property(e => e.FilesAreAllowed)
                    .IsRequired()
                    .HasColumnName("files_are_allowed")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.HasSubject)
                    .IsRequired()
                    .HasColumnName("has_subject")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.IsHidden)
                    .IsRequired()
                    .HasColumnName("is_hidden")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Postfix)
                    .IsRequired()
                    .HasColumnName("postfix")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasColumnName("prefix")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable("file");

                entity.HasIndex(e => e.BoardId)
                    .HasName("fkBoardId_File");

                entity.HasIndex(e => e.PostId)
                    .HasName("fkPostId_File");

                entity.HasIndex(e => e.ThreadId)
                    .HasName("fkThreadId_File");

                entity.Property(e => e.FileId)
                    .HasColumnName("file_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BoardId)
                    .HasColumnName("board_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Info)
                    .HasColumnName("info")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PostId)
                    .HasColumnName("post_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ThreadId)
                    .HasColumnName("thread_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ThumbnailName)
                    .IsRequired()
                    .HasColumnName("thumbnail_name")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.Board)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.BoardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardFile");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostFile");

                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.ThreadId)
                    .HasConstraintName("FK_ThreadFile");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.HasIndex(e => e.BoardId)
                    .HasName("fkBoardId_Post");

                entity.HasIndex(e => e.ThreadId)
                    .HasName("fkThreadId_Post");

                entity.HasIndex(e => e.AdminId)
                    .HasName("fkAdminId_Post");

                entity.Property(e => e.PostId)
                    .HasColumnName("post_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnonIpHash)
                    .IsRequired()
                    .HasColumnName("anon_ip_hash")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.AnonName)
                    .HasColumnName("anon_name")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.BoardId)
                    .HasColumnName("board_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.IsPinned)
                    .IsRequired()
                    .HasColumnName("is_pinned")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ThreadId)
                    .HasColumnName("thread_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TimeInUnixSeconds)
                    .HasColumnName("time_in_unix_seconds")
                    .HasColumnType("bigint(20)")
                    .HasDefaultValueSql("'unix_timestamp(current_timestamp())'");

                entity.Property(e => e.AdminId)
                    .HasColumnName("admin_id")
                    .HasColumnType("int(11)")
                    .IsRequired(false);

                entity.HasOne(p => p.Admin)
                    .WithMany(p => p.Post)
                    .HasForeignKey(p => p.AdminId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_AdminPost");

                entity.HasOne(d => d.Board)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.BoardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardPost");

                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.ThreadId)
                    .HasConstraintName("FK_ThreadPost");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report");

                entity.HasIndex(e => e.BoardId)
                    .HasName("fkBoardId_Report");

                entity.HasIndex(e => e.ThreadId)
                    .HasName("fkThreadId_Report");

                entity.Property(e => e.ReportId)
                    .HasColumnName("report_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BoardId)
                    .HasColumnName("board_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostId)
                    .HasColumnName("post_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ReportTimeInUnixSeconds)
                    .HasColumnName("report_time_in_unix_seconds")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.ThreadId)
                    .HasColumnName("thread_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Board)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.BoardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardReport");

                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.ThreadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThreadReport");
            });

            modelBuilder.Entity<Thread>(entity =>
            {
                entity.ToTable("thread");

                entity.HasIndex(e => e.BoardId)
                    .HasName("fkBoardId_Thread");

                entity.Property(e => e.ThreadId)
                    .HasColumnName("thread_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnonName)
                    .HasColumnName("anon_name")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.BoardId)
                    .HasColumnName("board_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BumpInUnixTime)
                    .HasColumnName("bump_in_unix_time")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.IsClosed)
                    .IsRequired()
                    .HasColumnName("is_closed")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.OpIpHash)
                    .IsRequired()
                    .HasColumnName("op_ip_hash")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.Board)
                    .WithMany(p => p.Thread)
                    .HasForeignKey(d => d.BoardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardThread");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}