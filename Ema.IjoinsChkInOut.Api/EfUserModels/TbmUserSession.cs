using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfUserModels
{
    public partial class TbmUserSession
    {
        public TbmUserSession()
        {
            TbmUserSegments = new HashSet<TbmUserSegment>();
            TbmUserSessionUsers = new HashSet<TbmUserSessionUser>();
        }

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
        public char IsCancel { get; set; }
        public DateTime Createdatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual ICollection<TbmUserSegment> TbmUserSegments { get; set; }
        public virtual ICollection<TbmUserSessionUser> TbmUserSessionUsers { get; set; }
    }
}
