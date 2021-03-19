using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ema.IJoin.MySql.Api.EfUserModels
{
    public partial class userijoin_databaseContext : DbContext
    {
        public userijoin_databaseContext()
        {
        }

        public userijoin_databaseContext(DbContextOptions<userijoin_databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbUserRegistration> TbUserRegistration { get; set; }
        public virtual DbSet<TbmUserSegment> TbmUserSegment { get; set; }
        public virtual DbSet<TbmUserSession> TbmUserSession { get; set; }
        public virtual DbSet<TbmUserSessionUser> TbmUserSessionUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=127.0.0.1;Port=3306;Database=userijoin_database;Username=root;Password=password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TbUserRegistration>(entity =>
            {
                entity.HasKey(e => e.SessionId);

                entity.ToTable("TB_USER_REGISTRATION", "userijoin_database");

                entity.HasIndex(e => e.CheckInDate)
                    .HasName("idx_check_in_date");

                entity.HasIndex(e => e.CheckInDatetime)
                    .HasName("idx_check_in_datetime");

                entity.HasIndex(e => e.SessionId)
                    .HasName("idx_session_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("idx_user_id");

                entity.HasIndex(e => new { e.CheckInDate, e.CheckInDatetime })
                    .HasName("idx_com");

                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CheckInBy)
                    .HasColumnName("check_in_by")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CheckInDate)
                    .HasColumnName("check_in_date")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CheckInDatetime)
                    .HasColumnName("check_in_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CheckInTime).HasColumnName("check_in_time");

                entity.Property(e => e.CheckOutBy)
                    .HasColumnName("check_out_by")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CheckOutDate)
                    .HasColumnName("check_out_date")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CheckOutDatetime)
                    .HasColumnName("check_out_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CheckOutTime).HasColumnName("check_out_time");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsCheckIn)
                    .IsRequired()
                    .HasColumnName("is_check_in")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsCheckOut)
                    .IsRequired()
                    .HasColumnName("is_check_out")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.StartDateQr)
                    .HasColumnName("start_date_qr")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbmUserSegment>(entity =>
            {
                entity.HasKey(e => e.SessionId);

                entity.ToTable("TBM_USER_SEGMENT", "userijoin_database");

                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnName("end_date")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasColumnName("end_time")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SegmentName)
                    .HasColumnName("segment_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnName("start_date")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasColumnName("start_time")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Venue)
                    .HasColumnName("venue")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Session)
                    .WithOne(p => p.TbmUserSegment)
                    .HasForeignKey<TbmUserSegment>(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_USER_SESSION_TBM_USER_SEGMENT_session_id");
            });

            modelBuilder.Entity<TbmUserSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);

                entity.ToTable("TBM_USER_SESSION", "userijoin_database");

                entity.HasIndex(e => e.CourseId)
                    .HasName("idx_course_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("idx_session_id");

                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CourseCreditHours)
                    .HasColumnName("course_credit_hours")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CourseCreditHoursInit)
                    .HasColumnName("course_credit_hours_init")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("course_id")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CourseNameTh)
                    .HasColumnName("course_name_th")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CourseOwnerContactNo)
                    .HasColumnName("course_owner_contact_no")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CourseOwnerEmail)
                    .IsRequired()
                    .HasColumnName("course_owner_email")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CoverPhotoName)
                    .HasColumnName("cover_photo_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CoverPhotoUrl)
                    .HasColumnName("cover_photo_url")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.Instructor)
                    .HasColumnName("instructor")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsCancel)
                    .IsRequired()
                    .HasColumnName("is_cancel")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PassingCriteriaException)
                    .HasColumnName("passing_criteria_exception")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PassingCriteriaExceptionInit)
                    .HasColumnName("passing_criteria_exception_init")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SessionName)
                    .HasColumnName("session_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnName("update_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Venue)
                    .HasColumnName("venue")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbmUserSessionUser>(entity =>
            {
                entity.HasKey(e => e.SessionId);

                entity.ToTable("TBM_USER_SESSION_USER", "userijoin_database");

                entity.HasIndex(e => e.UserId)
                    .HasName("idx_session_id");

                entity.HasIndex(e => new { e.SessionId, e.UserId })
                    .HasName("idx_session_id_user_id");

                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Session)
                    .WithOne(p => p.TbmUserSessionUser)
                    .HasForeignKey<TbmUserSessionUser>(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_SESSION_TBM_SESSION_USER_session_id");
            });
        }
    }
}
