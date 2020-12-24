using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmSession
    {
        public TbmSession()
        {
            TbtIjoinScanQrHis = new HashSet<TbtIjoinScanQrHi>();
            TbtIjoinScanQrs = new HashSet<TbtIjoinScanQr>();
        }

        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbtIjoinScanQrHi> TbtIjoinScanQrHis { get; set; }
        public virtual ICollection<TbtIjoinScanQr> TbtIjoinScanQrs { get; set; }
    }
}
