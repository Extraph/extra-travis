using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmCourse
    {
        public TbmCourse()
        {
            TbtIjoinScanQrHis = new HashSet<TbtIjoinScanQrHi>();
            TbtIjoinScanQrs = new HashSet<TbtIjoinScanQr>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameTh { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbtIjoinScanQrHi> TbtIjoinScanQrHis { get; set; }
        public virtual ICollection<TbtIjoinScanQr> TbtIjoinScanQrs { get; set; }
    }
}
