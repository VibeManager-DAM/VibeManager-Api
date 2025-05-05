using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibeManager_Api.DTO
{
    public class TicketCreateDTO
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int? NumCol { get; set; }
        public int? NumRow { get; set; }
        public int IdEvent { get; set; }
        public int IdUser { get; set; }
    }

}