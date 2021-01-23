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
            TbmSessions = new HashSet<TbmSession>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public string GuidName { get; set; }
        public string ImportBy { get; set; }
        public string ImportType { get; set; }
        public string ImportTotalrecords { get; set; }
        public string ImportMessage { get; set; }
        public DateTime Createdatetime { get; set; }

        public virtual ICollection<TbKlcDataMasterHi> TbKlcDataMasterHis { get; set; }
        public virtual ICollection<TbKlcDataMaster> TbKlcDataMasters { get; set; }
        public virtual ICollection<TbmSession> TbmSessions { get; set; }
    }
}
