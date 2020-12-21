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
        public virtual DbSet<TbmKlcFileImport> TbmKlcFileImports { get; set; }

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

            modelBuilder.Entity<TbmKlcFileImport>(entity =>
            {
                entity.ToTable("TBM_KLC_FILE_IMPORT");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdatetime)
                    .HasColumnName("createdatetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasColumnName("filename");

                entity.Property(e => e.Guidname)
                    .IsRequired()
                    .HasColumnName("guidname");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasComment("upload, import success, import failed, file not valid\n");
            });

            modelBuilder.HasSequence("TBM_KLC_FILE_IMPORT_id_seq").HasMax(2147483647);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
