using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmCompany
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
