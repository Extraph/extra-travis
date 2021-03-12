using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfAdminModels
{
    public partial class TbUserCompany
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public char IsDefault { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
