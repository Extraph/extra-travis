using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfAdminModels
{
    public partial class VSegmentGenQr
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public string Venue { get; set; }
        public string SegmentName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
