using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfModels
{
    public partial class TbmRegistrationStatus
    {
        public int Id { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime Createdatetime { get; set; }
    }
}
