using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfModels
{
    public partial class TbmSegment
    {
        public TbmSegment()
        {
            TbmSegmentUserHis = new HashSet<TbmSegmentUserHi>();
            TbmSegmentUsers = new HashSet<TbmSegmentUser>();
        }

        public int Id { get; set; }
        public int FileId { get; set; }
        public int CourseTypeId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
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

        public virtual TbmCourse Course { get; set; }
        public virtual TbmCourseType CourseType { get; set; }
        public virtual TbmKlcFileImport File { get; set; }
        public virtual TbmSession Session { get; set; }
        public virtual ICollection<TbmSegmentUserHi> TbmSegmentUserHis { get; set; }
        public virtual ICollection<TbmSegmentUser> TbmSegmentUsers { get; set; }
    }
}
