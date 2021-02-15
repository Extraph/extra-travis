using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmUser
    {
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
