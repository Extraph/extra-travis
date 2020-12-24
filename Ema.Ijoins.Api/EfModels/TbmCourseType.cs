using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmCourseType
    {
        public TbmCourseType()
        {
            TbtIjoinScanQrHis = new HashSet<TbtIjoinScanQrHi>();
            TbtIjoinScanQrs = new HashSet<TbtIjoinScanQr>();
        }

        public int Id { get; set; }
        public string CourseType { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbtIjoinScanQrHi> TbtIjoinScanQrHis { get; set; }
        public virtual ICollection<TbtIjoinScanQr> TbtIjoinScanQrs { get; set; }
    }
}
