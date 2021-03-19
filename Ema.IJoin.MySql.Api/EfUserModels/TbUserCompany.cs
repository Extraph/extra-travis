using System;
using System.Collections.Generic;

namespace Ema.IJoin.MySql.Api.EfUserModels
{
    public partial class TbUserCompany
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public string IsDefault { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
