using System;
using System.Collections.Generic;

#nullable disable

namespace Ema.IjoinsChkInOut.Api.EfUserModels
{
    public partial class TbUserRegistration
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string StartDateQr { get; set; }
        public char IsCheckIn { get; set; }
        public char IsCheckOut { get; set; }
        public DateTime CheckInDatetime { get; set; }
        public string CheckInDate { get; set; }
        public int CheckInTime { get; set; }
        public DateTime CheckOutDatetime { get; set; }
        public string CheckOutDate { get; set; }
        public int CheckOutTime { get; set; }
        public string CheckInBy { get; set; }
        public string CheckOutBy { get; set; }
    }
}
