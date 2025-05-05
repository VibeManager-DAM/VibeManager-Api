using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibeManager_Api.DTO
{
    public class EventCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Image { get; set; }
        public int Capacity { get; set; }
        public bool Seats { get; set; }
        public int? NumRows { get; set; }
        public int? NumColumns { get; set; }
        public int IdOrganizer { get; set; }
    }

}