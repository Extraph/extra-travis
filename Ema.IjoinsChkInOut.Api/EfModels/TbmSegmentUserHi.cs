﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfModels
{
    public partial class TbmSegmentUserHi
    {
        public int SegmentId { get; set; }
        public string UserId { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual TbmSegment Segment { get; set; }
    }
}