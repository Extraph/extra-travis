using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbKlcDataMasterHi
    {
        public long Id { get; set; }
        public int FileId { get; set; }
        public string CourseType { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public string SegmentNo { get; set; }
        public string SegmentName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CourseOwnerEmail { get; set; }
        public string CourseOwnerContactNo { get; set; }
        public string Venue { get; set; }
        public string Instructor { get; set; }
        public string CourseCreditHours { get; set; }
        public string PassingCriteriaException { get; set; }
        public string UserCompany { get; set; }
        public string UserId { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual TbmKlcFileImport File { get; set; }
    }
}
