using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VibeManager_Api.DTO;
using VibeManager_Api.Models;

namespace VibeManager_Api.Controllers
{
    public class EVENTSController : ApiController
    {
        private VibeEntities db = new VibeEntities();

        // GET: api/events
        [HttpGet]
        [Route("api/events")]
        public async Task<IHttpActionResult> GetAllEvents()
        {
            var events = await db.EVENTS
                .Include(e => e.id_organizer)
                .Select(e => new
                {
                    e.id,
                    e.title,
                    e.description,
                    e.date,
                    e.time,
                    e.image,
                    e.capacity,
                    e.seats,
                    e.num_rows,
                    e.num_columns,
                    Organizer = new
                    {
                        e.id_organizer,
                        e.USERS.fullname,
                    }
                })
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet]
        [Route("api/events/{id}")]
        public async Task<IHttpActionResult> GetEventDetails(int id)
        {
            var ev = await db.EVENTS
                .Include(e => e.id_organizer)
                .Where(e => e.id == id)
                .Select(e => new
                {
                    e.id,
                    e.title,
                    e.description,
                    e.date,
                    e.time,
                    e.image,
                    e.capacity,
                    e.seats,
                    e.num_rows,
                    e.num_columns,
                    Organizer = new
                    {
                        e.USERS.id,
                        e.USERS.fullname
                    }
                })
                .FirstOrDefaultAsync();

            if (ev == null)
                return NotFound();

            return Ok(ev);
        }

        // GET: api/events/{id}/seats
        [HttpGet]
        [Route("api/events/{id}/seats")]
        public async Task<IHttpActionResult> GetEventSeats(int id)
        {
            var ev = await db.EVENTS.FindAsync(id);
            if (ev == null || !ev.seats)
                return NotFound();

            var takenSeats = await db.TICKETS
                .Where(t => t.id_event == id)
                .Select(t => new { t.num_row, t.num_col })
                .ToListAsync();

            var seats = new List<object>();
            for (int row = 1; row <= ev.num_rows; row++)
            {
                for (int col = 1; col <= ev.num_columns; col++)
                {
                    bool isTaken = takenSeats.Any(s => s.num_row == row && s.num_col == col);
                    seats.Add(new
                    {
                        Row = row,
                        Column = col,
                        IsTaken = isTaken
                    });
                }
            }

            return Ok(seats);
        }


        // POST: api/events
        [HttpPost]
        [Route("api/events")]
        public async Task<IHttpActionResult> CreateEvent(EventCreateDTO dto)
        {
            var ev = new EVENTS
            {
                title = dto.Title,
                description = dto.Description,
                date = dto.Date,
                time = dto.Time,
                image = dto.Image,
                capacity = dto.Capacity,
                seats = dto.Seats,
                num_rows = dto.NumRows,
                num_columns = dto.NumColumns,
                id_organizer = dto.IdOrganizer
            };

            db.EVENTS.Add(ev);
            await db.SaveChangesAsync();


            var chat = new CHAT
            {
                id_event = ev.id,
                id_user = dto.IdOrganizer
            };

            db.CHAT.Add(chat);
            await db.SaveChangesAsync();

            return Ok(new { ev.id, ev.title, ev.date });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}