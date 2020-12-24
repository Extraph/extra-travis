using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmRegistrationStatus
    {
        public int Id { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime Createdatetime { get; set; }
    }
}
