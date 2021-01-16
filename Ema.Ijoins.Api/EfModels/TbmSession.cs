using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmSession
    {
        public TbmSession()
        {
            TbmSegments = new HashSet<TbmSegment>();
        }

        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbmSegment> TbmSegments { get; set; }
    }
}
