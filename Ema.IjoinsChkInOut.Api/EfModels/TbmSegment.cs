using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfModels
{
    public partial class TbmSegment
    {
        public int Id { get; set; }
        public string CourseId { get; set; }
        public string SessionId { get; set; }
        public string SegmentNo { get; set; }
        public string SegmentName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual TbmSession Session { get; set; }
    }
}
