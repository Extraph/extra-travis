using System;
using System.Collections.Generic;

namespace Ema.IJoin.MySql.Api.EfUserModels
{
    public partial class TbmUserSessionUser
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }

        public virtual TbmUserSession Session { get; set; }
    }
}
