using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmSession
    {
        public TbmSession()
        {
            TbmSegments = new HashSet<TbmSegment>();
            TbmSessionUserHis = new HashSet<TbmSessionUserHi>();
            TbmSessionUsers = new HashSet<TbmSessionUser>();
        }

        public int FileId { get; set; }
        public int CourseTypeId { get; set; }
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

        public virtual ICollection<TbmSegment> TbmSegments { get; set; }
        public virtual ICollection<TbmSessionUserHi> TbmSessionUserHis { get; set; }
        public virtual ICollection<TbmSessionUser> TbmSessionUsers { get; set; }
    }
}
