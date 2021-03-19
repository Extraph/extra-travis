using System;
using System.Collections.Generic;

namespace Ema.IJoin.MySql.Api.EfAdminModels
{
    public partial class TbmSession
    {
        public int FileId { get; set; }
        public string CourseType { get; set; }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CourseOwnerEmail { get; set; }
        public string CourseOwnerContactNo { get; set; }
        public string Venue { get; set; }
        public string Instructor { get; set; }
        public string CourseCreditHoursInit { get; set; }
        public string PassingCriteriaExceptionInit { get; set; }
        public string CourseCreditHours { get; set; }
        public string PassingCriteriaException { get; set; }
        public string IsCancel { get; set; }
        public string CoverPhotoName { get; set; }
        public string CoverPhotoUrl { get; set; }
        public DateTime Createdatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
