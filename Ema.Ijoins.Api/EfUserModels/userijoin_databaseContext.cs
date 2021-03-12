using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ema.Ijoins.Api.EfUserModels
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

        public virtual DbSet<TbUserRegistration> TbUserRegistrations { get; set; }
        public virtual DbSet<TbmUserSegment> TbmUserSegments { get; set; }
        public virtual DbSet<TbmUserSession> TbmUserSessions { get; set; }
        public virtual DbSet<TbmUserSessionUser> TbmUserSessionUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5433;Database=userijoin_database;Username=userijoin_user;Password=userijoin_password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<TbUserRegistration>(entity =>
            {
                entity.ToTable("TB_USER_REGISTRATION");

                entity.HasIndex(e => e.CheckInDatetime, "idx_tb_regis_checkin");

                entity.HasIndex(e => new { e.SessionId, e.UserId, e.CheckInDate, e.CheckInDatetime }, "idx_tb_regis_com");

                entity.HasIndex(e => e.SessionId, "idx_tb_regis_session");

                entity.HasIndex(e => e.UserId, "idx_tb_regis_user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CheckInBy).HasColumnName("check_in_by");

                entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");

                entity.Property(e => e.CheckInDatetime)
                    .HasColumnName("check_in_datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.CheckInTime).HasColumnName("check_in_time");

                entity.Property(e => e.CheckOutBy).HasColumnName("check_out_by");

                entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date");

                entity.Property(e => e.CheckOutDatetime)
                    .HasColumnName("check_out_datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.CheckOutTime).HasColumnName("check_out_time");

                entity.Property(e => e.IsCheckIn)
                    .HasMaxLength(1)
                    .HasColumnName("is_check_in")
                    .HasDefaultValueSql("'0'::bpchar");

                entity.Property(e => e.IsCheckOut)
                    .HasMaxLength(1)
                    .HasColumnName("is_check_out")
                    .HasDefaultValueSql("'0'::bpchar");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasColumnName("session_id");

                entity.Property(e => e.StartDateQr).HasColumnName("start_date_qr");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<TbmUserSegment>(entity =>
            {
                entity.HasKey(e => new { e.SessionId, e.StartDateTime, e.EndDateTime })
                    .HasName("TBM_USER_SEGMENT_pkey");

                entity.ToTable("TBM_USER_SEGMENT");

                entity.HasIndex(e => e.SessionId, "fki_fk_TBM_SESSION_session_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnName("end_date");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasColumnName("end_time");

                entity.Property(e => e.SegmentName).HasColumnName("segment_name");

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnName("start_date");

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasColumnName("start_time");

                entity.Property(e => e.Venue).HasColumnName("venue");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.TbmUserSegments)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_SESSION_session_id");
            });

            modelBuilder.Entity<TbmUserSession>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("TBM_USER_SESSION_pkey");

                entity.ToTable("TBM_USER_SESSION");

                entity.HasIndex(e => e.CourseId, "idx_TBM_SESSION_course_id");

                entity.HasIndex(e => e.SessionId, "idx_TBM_SESSION_session_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.CourseCreditHours).HasColumnName("course_credit_hours");

                entity.Property(e => e.CourseCreditHoursInit).HasColumnName("course_credit_hours_init");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name");

                entity.Property(e => e.CourseNameTh).HasColumnName("course_name_th");

                entity.Property(e => e.CourseOwnerContactNo).HasColumnName("course_owner_contact_no");

                entity.Property(e => e.CourseOwnerEmail)
                    .IsRequired()
                    .HasColumnName("course_owner_email");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.Instructor).HasColumnName("instructor");

                entity.Property(e => e.IsCancel)
                    .HasMaxLength(1)
                    .HasColumnName("is_cancel")
                    .HasDefaultValueSql("'0'::bpchar");

                entity.Property(e => e.PassingCriteriaException).HasColumnName("passing_criteria_exception");

                entity.Property(e => e.PassingCriteriaExceptionInit).HasColumnName("passing_criteria_exception_init");

                entity.Property(e => e.SessionName).HasColumnName("session_name");

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnName("update_datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Venue).HasColumnName("venue");
            });

            modelBuilder.Entity<TbmUserSessionUser>(entity =>
            {
                entity.HasKey(e => new { e.SessionId, e.UserId })
                    .HasName("TBM_USER_SESSION_USER_pkey");

                entity.ToTable("TBM_USER_SESSION_USER");

                entity.HasIndex(e => e.UserId, "fki_TBM_SESSION_USER_user_id");

                entity.HasIndex(e => e.SessionId, "fki_fk_TBM_SESSION_id");

                entity.HasIndex(e => new { e.UserId, e.SessionId }, "idx_TBM_SESSION_USER_session_user_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.TbmUserSessionUsers)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_SESSION_id");
            });

            modelBuilder.HasSequence("TB_USER_REGISTRATION_id_seq").HasMax(2147483647);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
