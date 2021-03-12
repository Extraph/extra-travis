using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfAdminModels
{
    public partial class TbmUser
    {
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserOrganization { get; set; }
        public string UserCompany { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
