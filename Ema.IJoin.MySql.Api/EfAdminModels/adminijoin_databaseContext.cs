using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ema.IJoin.MySql.Api.EfAdminModels
{
    public partial class adminijoin_databaseContext : DbContext
    {
        public adminijoin_databaseContext()
        {
        }

        public adminijoin_databaseContext(DbContextOptions<adminijoin_databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbUserCompany> TbUserCompany { get; set; }
        public virtual DbSet<TbmCompany> TbmCompany { get; set; }
        public virtual DbSet<TbmSession> TbmSession { get; set; }

        // Unable to generate entity type for table 'adminijoin_database.TBM_COURSE'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_COURSE_TYPE'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_KLC_FILE_IMPORT'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_REGISTRATION_STATUS'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_ROLE'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_SEGMENT'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_SESSION_USER'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_SESSION_USER_HIS'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TBM_USER'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TB_KLC_DATA_MASTER'. Please see the warning messages.
        // Unable to generate entity type for table 'adminijoin_database.TB_KLC_DATA_MASTER_HIS'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=127.0.0.1;Port=3306;Database=adminijoin_database;Username=root;Password=password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TbUserCompany>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("TB_USER_COMPANY", "adminijoin_database");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDatetime)
                    .HasColumnName("create_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDefault)
                    .IsRequired()
                    .HasColumnName("is_default")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbmCompany>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("TBM_COMPANY", "adminijoin_database");

                entity.HasIndex(e => e.CompanyCode)
                    .HasName("company_code_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CompanyCode)
                    .IsRequired()
                    .HasColumnName("company_code")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .HasColumnName("create_by")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDatetime)
                    .HasColumnName("create_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnName("update_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<TbmSession>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("TBM_SESSION", "adminijoin_database");

                entity.HasIndex(e => e.CourseId)
                    .HasName("idx_course_id");

                entity.HasIndex(e => e.FileId)
                    .HasName("fk_SESSION_TO_TBM_KLC_FILE_IMPORT_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("idx_session_id");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyCode)
                    .IsRequired()
                    .HasColumnName("company_code")
                    .HasMaxLength(500)
                    .IsUnicode(false);

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

                entity.Property(e => e.CourseType)
                    .IsRequired()
                    .HasColumnName("course_type")
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

                entity.Property(e => e.FileId).HasColumnName("file_id");

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

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasColumnName("session_id")
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
        }
    }
}
