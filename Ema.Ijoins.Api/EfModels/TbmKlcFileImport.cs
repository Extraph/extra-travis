using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmKlcFileImport
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public DateTime Createdatetime { get; set; }
        public string Status { get; set; }
        public string Guidname { get; set; }
    }
}
