using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmCourseType
    {
        public TbmCourseType()
        {
            TbmSegments = new HashSet<TbmSegment>();
        }

        public int Id { get; set; }
        public string CourseType { get; set; }
        public string CourseId { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbmSegment> TbmSegments { get; set; }
    }
}
