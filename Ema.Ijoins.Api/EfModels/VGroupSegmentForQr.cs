using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class VGroupSegmentForQr
    {
        public string SessionId { get; set; }
        public string SegmentNo { get; set; }
        public string SegmentName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? Createdatetime { get; set; }
    }
}
