using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmCourseType
    {
        public int Id { get; set; }
        public string CourseType { get; set; }
        public string CompletionStatus { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
