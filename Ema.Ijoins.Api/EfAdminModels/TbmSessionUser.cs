using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfAdminModels
{
    public partial class TbmSessionUser
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime Createdatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual TbmSession Session { get; set; }
    }
}
