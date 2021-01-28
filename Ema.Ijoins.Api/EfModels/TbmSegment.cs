using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmSegment
    {
        public string SessionId { get; set; }
        public string SegmentNo { get; set; }
        public string SegmentName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Venue { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual TbmSession Session { get; set; }
    }
}
