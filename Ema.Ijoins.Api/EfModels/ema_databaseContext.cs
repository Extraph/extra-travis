using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class ema_databaseContext : DbContext
    {
        public ema_databaseContext()
        {
        }

        public ema_databaseContext(DbContextOptions<ema_databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbKlcDataMaster> TbKlcDataMasters { get; set; }
        public virtual DbSet<TbKlcDataMasterHi> TbKlcDataMasterHis { get; set; }
        public virtual DbSet<TbmCourse> TbmCourses { get; set; }
        public virtual DbSet<TbmCourseType> TbmCourseTypes { get; set; }
        public virtual DbSet<TbmKlcFileImport> TbmKlcFileImports { get; set; }
        public virtual DbSet<TbmRegistrationStatus> TbmRegistrationStatuses { get; set; }
        public virtual DbSet<TbmSegment> TbmSegments { get; set; }
        public virtual DbSet<TbmSegmentUser> TbmSegmentUsers { get; set; }
        public virtual DbSet<TbmSegmentUserHi> TbmSegmentUserHis { get; set; }
        public virtual DbSet<TbmSession> TbmSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ema_database;Username=ema_user;Password=magical_password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<TbKlcDataMaster>(entity =>
            {
                entity.ToTable("TB_KLC_DATA_MASTER");

                entity.HasIndex(e => e.FileId, "fki_fk_tbm_klc_file_import_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseCreditHours).HasColumnName("course_credit_hours");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name");

                entity.Property(e => e.CourseNameTh).HasColumnName("course_name_th");

                entity.Property(e => e.CourseOwnerContactNo).HasColumnName("course_owner_contactNo");

                entity.Property(e => e.CourseOwnerEmail)
                    .IsRequired()
                    .HasColumnName("course_owner_email");

                entity.Property(e => e.CourseType).HasColumnName("course_type");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnName("end_date");

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasColumnName("end_time");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Instructor).HasColumnName("instructor");

                entity.Property(e => e.PassingCriteriaException).HasColumnName("passing_criteria_exception");

                entity.Property(e => e.RegistrationStatus)
                    .IsRequired()
                    .HasColumnName("registration_status");

                entity.Property(e => e.SegmentName).HasColumnName("segment_name");

                entity.Property(e => e.SegmentNo)
                    .IsRequired()
                    .HasColumnName("segment_no");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasColumnName("session_id");

                entity.Property(e => e.SessionName)
                    .IsRequired()
                    .HasColumnName("session_name");

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnName("start_date");

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasColumnName("start_time");

                entity.Property(e => e.UserCompany).HasColumnName("user_company");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.Venue).HasColumnName("venue");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.TbKlcDataMasters)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbm_klc_file_import_id");
            });

            modelBuilder.Entity<TbKlcDataMasterHi>(entity =>
            {
                entity.ToTable("TB_KLC_DATA_MASTER_HIS");

                entity.HasIndex(e => e.FileId, "fki_fk_tbm_klc_file_import_id_his");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CourseCreditHours).HasColumnName("course_credit_hours");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name");

                entity.Property(e => e.CourseNameTh).HasColumnName("course_name_th");

                entity.Property(e => e.CourseOwnerContactNo).HasColumnName("course_owner_contactNo");

                entity.Property(e => e.CourseOwnerEmail)
                    .IsRequired()
                    .HasColumnName("course_owner_email");

                entity.Property(e => e.CourseType).HasColumnName("course_type");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnName("end_date");

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasColumnName("end_time");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Instructor).HasColumnName("instructor");

                entity.Property(e => e.PassingCriteriaException).HasColumnName("passing_criteria_exception");

                entity.Property(e => e.RegistrationStatus)
                    .IsRequired()
                    .HasColumnName("registration_status");

                entity.Property(e => e.SegmentName).HasColumnName("segment_name");

                entity.Property(e => e.SegmentNo)
                    .IsRequired()
                    .HasColumnName("segment_no");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasColumnName("session_id");

                entity.Property(e => e.SessionName)
                    .IsRequired()
                    .HasColumnName("session_name");

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnName("start_date");

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasColumnName("start_time");

                entity.Property(e => e.UserCompany).HasColumnName("user_company");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.Venue).HasColumnName("venue");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.TbKlcDataMasterHis)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbm_klc_file_import_id_his");
            });

            modelBuilder.Entity<TbmCourse>(entity =>
            {
                entity.HasKey(e => e.CourseId)
                    .HasName("TBM_COURSE_pkey");

                entity.ToTable("TBM_COURSE");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name");

                entity.Property(e => e.CourseNameTh).HasColumnName("course_name_th");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<TbmCourseType>(entity =>
            {
                entity.ToTable("TBM_COURSE_TYPE");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseType)
                    .IsRequired()
                    .HasColumnName("course_type");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<TbmKlcFileImport>(entity =>
            {
                entity.ToTable("TBM_KLC_FILE_IMPORT");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name");

                entity.Property(e => e.GuidName)
                    .IsRequired()
                    .HasColumnName("guid_name");

                entity.Property(e => e.ImportBy)
                    .IsRequired()
                    .HasColumnName("import_by");

                entity.Property(e => e.ImportMessage).HasColumnName("import_message");

                entity.Property(e => e.ImportTotalrecords).HasColumnName("import_totalrecords");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasComment("upload success, import success, import failed");
            });

            modelBuilder.Entity<TbmRegistrationStatus>(entity =>
            {
                entity.ToTable("TBM_REGISTRATION_STATUS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.RegistrationStatus)
                    .IsRequired()
                    .HasColumnName("registration_status");
            });

            modelBuilder.Entity<TbmSegment>(entity =>
            {
                entity.ToTable("TBM_SEGMENT");

                entity.HasIndex(e => e.CourseTypeId, "fki_fk_TBM_COURSE_TYPE_id");

                entity.HasIndex(e => e.CourseId, "fki_fk_TBM_COURSE_course_id");

                entity.HasIndex(e => e.FileId, "fki_fk_TBM_KLC_FILE_IMPORT_id");

                entity.HasIndex(e => e.SessionId, "fki_fk_TBM_SESSION_session_id");

                entity.HasIndex(e => new { e.StartDateTime, e.EndDateTime, e.SessionId, e.CourseId }, "uni_TBM_SEGMENT")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseCreditHours).HasColumnName("course_credit_hours");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseName).HasColumnName("course_name");

                entity.Property(e => e.CourseNameTh).HasColumnName("course_name_th");

                entity.Property(e => e.CourseOwnerContactNo).HasColumnName("course_owner_contactNo");

                entity.Property(e => e.CourseOwnerEmail)
                    .IsRequired()
                    .HasColumnName("course_owner_email");

                entity.Property(e => e.CourseTypeId).HasColumnName("course_type_id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EndDateTime).HasColumnName("end_date_time");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Instructor).HasColumnName("instructor");

                entity.Property(e => e.PassingCriteriaException).HasColumnName("passing_criteria_exception");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasColumnName("session_id");

                entity.Property(e => e.SessionName).HasColumnName("session_name");

                entity.Property(e => e.StartDateTime).HasColumnName("start_date_time");

                entity.Property(e => e.Venue).HasColumnName("venue");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TbmSegments)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_COURSE_course_id");

                entity.HasOne(d => d.CourseType)
                    .WithMany(p => p.TbmSegments)
                    .HasForeignKey(d => d.CourseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_COURSE_TYPE_id");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.TbmSegments)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_KLC_FILE_IMPORT_id");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.TbmSegments)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_SESSION_session_id");
            });

            modelBuilder.Entity<TbmSegmentUser>(entity =>
            {
                entity.HasKey(e => new { e.SegmentId, e.UserId })
                    .HasName("TBM_SEGMENT_USER_pkey");

                entity.ToTable("TBM_SEGMENT_USER");

                entity.HasIndex(e => e.UserId, "fki_TBM_SEGMENT_USER_user_id");

                entity.HasIndex(e => e.SegmentId, "fki_fk_TBM_SEGMENT_id");

                entity.Property(e => e.SegmentId).HasColumnName("segment_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.RegistrationStatus)
                    .IsRequired()
                    .HasColumnName("registration_status");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.TbmSegmentUsers)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_SEGMENT_id");
            });

            modelBuilder.Entity<TbmSegmentUserHi>(entity =>
            {
                entity.HasKey(e => new { e.SegmentId, e.UserId })
                    .HasName("TBM_SEGMENT_USER_HIS_pkey");

                entity.ToTable("TBM_SEGMENT_USER_HIS");

                entity.HasIndex(e => e.UserId, "fki_TBM_SEGMENT_USER_HIS_user_id");

                entity.HasIndex(e => e.SegmentId, "fki_fk_TBM_SEGMENT_HIS_id");

                entity.Property(e => e.SegmentId).HasColumnName("segment_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.RegistrationStatus)
                    .IsRequired()
                    .HasColumnName("registration_status");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.TbmSegmentUserHis)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TBM_SEGMENT_HIS_id");
            });

            modelBuilder.Entity<TbmSession>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("TBM_SESSION_pkey");

                entity.ToTable("TBM_SESSION");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SessionName).HasColumnName("session_name");
            });

            modelBuilder.HasSequence("TB_KLC_DATA_MASTER_id_seq");

            modelBuilder.HasSequence("TBM_COURSE_TYPE_id_seq").HasMax(2147483647);

            modelBuilder.HasSequence("TBM_KLC_FILE_IMPORT_id_seq").HasMax(2147483647);

            modelBuilder.HasSequence("TBM_REGISTRATION_STATUS_id_seq").HasMax(2147483647);

            modelBuilder.HasSequence("TBM_SEGMENT_id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
