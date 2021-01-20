using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmSegmentUser
    {
        public int SegmentId { get; set; }
        public string UserId { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime Createdatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual TbmSegment Segment { get; set; }
    }
}
