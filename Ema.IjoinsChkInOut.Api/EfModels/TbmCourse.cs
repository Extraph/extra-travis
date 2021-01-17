using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfModels
{
    public partial class TbmCourse
    {
        public TbmCourse()
        {
            TbmSegments = new HashSet<TbmSegment>();
        }

        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbmSegment> TbmSegments { get; set; }
    }
}
