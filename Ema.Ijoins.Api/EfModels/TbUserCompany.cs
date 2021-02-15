using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbUserCompany
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
