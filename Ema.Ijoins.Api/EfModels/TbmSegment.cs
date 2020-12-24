using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmSegment
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public string CourseOwnerEmail { get; set; }
        public string CourseOwnerContactNo { get; set; }
        public string Venue { get; set; }
        public string Instructor { get; set; }
        public string CourseCreditHours { get; set; }
        public string PassingCriteriaException { get; set; }
        public DateTime Createdatetime { get; set; }
    }
}
