﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbtIjoinScanQrAddMore
    {
        public long Id { get; set; }
        public int CourseId { get; set; }
        public int SessionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string UserId { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime? CheckInDateTime { get; set; }
        public DateTime? CheckOutDateTime { get; set; }
        public DateTime Createdatetime { get; set; }
        public DateTime Updatedatetime { get; set; }

        public virtual TbmCourse Course { get; set; }
        public virtual TbmSession Session { get; set; }
    }
}