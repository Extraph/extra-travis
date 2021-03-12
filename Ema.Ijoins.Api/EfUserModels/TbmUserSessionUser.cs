using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfUserModels
{
    public partial class TbmUserSessionUser
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }

        public virtual TbmUserSession Session { get; set; }
    }
}
