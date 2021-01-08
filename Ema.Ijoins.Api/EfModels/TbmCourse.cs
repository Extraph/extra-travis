﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmCourse
    {
        public TbmCourse()
        {
            TbmSegments = new HashSet<TbmSegment>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbmSegment> TbmSegments { get; set; }
    }
}
