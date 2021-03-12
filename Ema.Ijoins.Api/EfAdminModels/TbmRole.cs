using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfAdminModels
{
    public partial class TbmRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
