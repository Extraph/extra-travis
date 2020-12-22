using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.Ijoins.Api.EfModels
{
    public partial class TbmKlcFileImport
    {
        public TbmKlcFileImport()
        {
            TbKlcDataMasterHis = new HashSet<TbKlcDataMasterHi>();
            TbKlcDataMasters = new HashSet<TbKlcDataMaster>();
        }

        public int Id { get; set; }
        public string Filename { get; set; }
        public string Status { get; set; }
        public string Guidname { get; set; }
        public string Importby { get; set; }
        public string Totalrecords { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbKlcDataMasterHi> TbKlcDataMasterHis { get; set; }
        public virtual ICollection<TbKlcDataMaster> TbKlcDataMasters { get; set; }
    }
}
